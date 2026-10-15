using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DragonRigController : MonoBehaviour
{
    [SerializeField]
    private Transform targetAnchor;

    [Header("플레이어 기준 드래곤 몸 자체 회전")]
    [SerializeField]
    private Rig turnRig;

    [SerializeField, Min(0.01f)]
    private float weightSpeed = 3.0f;

    private float targetTurnWeight;

    [SerializeField]
    private Transform turnTarget;

    [SerializeField, Range(0f, 90f)]
    private float maxViewAngle = 45.0f; // 최대 좌우 각도
    public float MaxViewAngle => maxViewAngle;

    [SerializeField, Min(0.01f)]
    private float viewSpeed = 90.0f;

    private Transform viewTarget; // 현재 코드에서는 플레이어 타겟
    private Vector3 defaultTurnLocalPosition;
    private float currentViewAngle; // 현재 기준점에서의 각도

    public Vector3 ViewOriginPosition => targetAnchor.position;

    private void Awake()
    {
        targetTurnWeight = 0.0f;
        turnRig.weight = 0.0f;

        defaultTurnLocalPosition = turnTarget.localPosition; // 얘를 기점으로 회전
        currentViewAngle = 0.0f;
    }

// Start is called once before the first execution of Update after the MonoBehaviour is created
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTurnTarget();
        turnRig.weight = Mathf.MoveTowards(turnRig.weight, targetTurnWeight, weightSpeed * Time.deltaTime);
    }

    public void SetTurnWeight(float _value)
    {
        //if(_value < 0.0f)
        //{
        //    _value = 0.0f;
        //}
        //else if(_value > 1.0f)
        //{
        //    _value = 1.0f;
        //}

        //targetTurnWeight = _value;

        targetTurnWeight = Mathf.Clamp01(_value);
    }

    public void SetViewTarget(Transform _target)
    {
        viewTarget = _target; // 아마 플레이어만 일단 받아올예정이지만 target으로 일단
    }

    private void UpdateTurnTarget()
    {
        Vector3 defaultTargetAnchorWordOffset = targetAnchor.TransformVector(defaultTurnLocalPosition); //지역좌표 월드로 변환

        float desiredAngle = 0f; // 목표 각도

        if(viewTarget != null && targetTurnWeight > 0f) // 플레이어위치를 알고 애니메이션 리깅이 적용중이라면
        {
            Vector3 defaultForward = Vector3.ProjectOnPlane(defaultTargetAnchorWordOffset, Vector3.up); // 좌우 높이 계산을 위해 높이 제거 -> 평면투영

            Vector3 targetDir = Vector3.ProjectOnPlane(viewTarget.position - targetAnchor.position, Vector3.up); // 높이 제거한 방향 획득

            if(defaultForward.sqrMagnitude > 0.0001f && targetDir.sqrMagnitude > 0.0001f)
            {
                float angle = Vector3.SignedAngle(defaultForward, targetDir, Vector3.up); // 타겟이 좌우각도가 어디에 있는지 확인하고

                desiredAngle = Mathf.Clamp(angle, -maxViewAngle, maxViewAngle); // 목표 각도설정 체크
            }
        }

        currentViewAngle = Mathf.MoveTowards(currentViewAngle, desiredAngle, viewSpeed * Time.deltaTime);
        Vector3 targetAnchorWorldOffsetAfterRotation = Quaternion.AngleAxis(currentViewAngle, Vector3.up) * defaultTargetAnchorWordOffset;

        turnTarget.localPosition = targetAnchor.InverseTransformVector(targetAnchorWorldOffsetAfterRotation); // 월드 벡터 지역벡터로 변환
    }
}
