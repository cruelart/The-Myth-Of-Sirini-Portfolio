using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public class PlayerInteract : MonoBehaviour
{
    private PlayerInputReader playerInputReader; // 플레이어 입력을 처리하는 컴포넌트

    [Header("인지범위 조절")]
    [SerializeField]
    private float interactionRange = 3f; // 상호작용 인지범위 조절

    [SerializeField]
    private float interactionAngle = 120f; // 상호작용 인지각도 조절

    [Header("상호작용 레이어 설정")]
    [SerializeField] 
    private LayerMask interactionLayer;

    private readonly Collider[] interactionResults = new Collider[10]; // 상호작용 결과를 저장할 배열

    private ITalkInteractable target; // 상호작용 대상

    private void Awake()
    {
        playerInputReader = GetComponent<PlayerInputReader>(); // PlayerInputReader 컴포넌트 가져오기
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInputReader.OnInteractPressed += TryInteract; // 플레이어 입력 이벤트에 상호작용 시도 메서드 등록
        UIManager.Instance.TalkUI.OnHide += HandleTalkUIHide; // TalkUI 숨김 이벤트에 처리 메서드 등록
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TryInteract()
    {
        target = FindNearInteractionCollider();
        target?.Interact(transform);
    }

    private bool IsInteractionViewAngle(Vector3 _directionToTarget, Vector3 _playerPos)
    {
        float halfViewAngle = interactionAngle * 0.5f;
        float minimumDot = Mathf.Cos(halfViewAngle * Mathf.Deg2Rad); // 내적 비교에 사용할 값으로 변환

        float dot = Vector3.Dot(_playerPos, _directionToTarget);

        if (dot < minimumDot)
        {
            return false; // 시야각에 보이지 않음
        }

        return true; // 시야각에 보임
    }

    private ITalkInteractable FindNearInteractionCollider()
    {
        // 플레이어의 위치에서 interactionRange 범위 내의 모든 Collider를 감지
        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, interactionRange, interactionResults, interactionLayer);

        ITalkInteractable nearestTarget = null; // 가장 가까운 상호작용 가능한 객체를 찾기 위한 변수
        float nearestDistance = float.MaxValue; // 가장 가까운 객체까지의 거리 초기화

        for (int i = 0; i < hitCount; i++)
        {
            Collider collider = interactionResults[i];

            ITalkInteractable talkTarget = collider.GetComponent<ITalkInteractable>();

            if (talkTarget == null) // 상호작용 불가능한 객체는 무시
            {
                continue;
            }

            //상호작용이 가능한 객체라면

            // 1. 시야각에 보이는지를 먼저 판단
            Vector3 directionToTarget = collider.bounds.center - transform.position;
            directionToTarget.y = 0; // 수평 방향만 고려

            if (!IsInteractionViewAngle(directionToTarget, transform.forward))
            {
                continue; // 시야각에 보이지 않으면 무시
            }

            //보이는 것 중에서 가장 가까운 객체찾기
            float distance = (collider.ClosestPoint(transform.position) - transform.position).sqrMagnitude; // 제곱 거리 계산

            if (nearestDistance > distance)
            {
                nearestDistance = distance;
                nearestTarget = talkTarget;
            }
        }

        return nearestTarget; // 가장 가까운 상호작용 가능한 객체 반환
    }

    private void HandleTalkUIHide()
    {
        target?.ExitInteraction();
        target = null; // 초기화
    }
}
