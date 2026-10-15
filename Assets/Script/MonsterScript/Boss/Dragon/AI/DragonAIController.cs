using UnityEngine;
using BehaviorTree;
public class DragonAIController : MonsterAIController, IMonsterDamageable
{
    private DragonStateMachine DragonStateMachine;
    public DragonAnimationEvents AnimationEvents { get; private set; }
    public DragonRigController RigController { get; private set; }

    private Node idle_rootNode;
    private Node walk_rootNode;
    private Node attack_rootNode;
    private Node react_rootNode;
    private Node die_rootNode;
    private Node flying_rootNode;

    [SerializeField]
    private Transform player;

    [SerializeField, Min(0f)]
    private float battleStartRange = 20.0f;

    [SerializeField, Min(0f)]
    private float bodyTurnSpeed = 45.0f;

    [SerializeField, Min(0f)]
    private float chaseDistance = 3.0f;

    [SerializeField, Min(0f)]
    private float moveSpeed = 2.0f;

    [SerializeField, Range(0f, 90f)]
    private float biteAngle = 30f;

    protected override void Awake()
    {
        base.Awake();

        AnimationEvents = GetComponent<DragonAnimationEvents>();
        RigController = GetComponent<DragonRigController>();

        DragonStateMachine = new DragonStateMachine(this);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        RigController.SetViewTarget(player);
        DragonStateMachine.Init(); // 상태 초기화
        
    }

    // Update is called once per frame
    void Update()
    {
        DragonStateMachine.Tick();
    }

    void FixedUpdate()
    {
        DragonStateMachine.FixedTick();
    }

    public void OnDamaged(int _damage, Vector3 _attackerPos)
    {

    }

    public void PlayAnimation(string _animeName, float _crossFade)
    {
        PlayAnime(_animeName, _crossFade);
    }

    public bool IsFindPlayer()
    {
        if (player == null)
            return false;

        float distance = (player.transform.position - transform.position).sqrMagnitude;

        return distance <= battleStartRange * battleStartRange;
    }

    private bool TryGetPlayerDirection(out Vector3 direction)
    {
        direction = Vector3.zero; // 일단 무방향

        if (player == null)
            return false; // 플레이어가 존재하지않으면 바로 false처리

        direction = player.position - RigController.ViewOriginPosition;
        direction.y = 0.0f; // 몸통 돌리기용이니까 Y는 제거처리하고

        return direction.sqrMagnitude > 0.0001f; // 방향이 정해지면 true를 뱉고 방향도 같이 전달
    }

    public bool NeedBodyTurn(float _allowedAngle)
    {
        if (!TryGetPlayerDirection(out Vector3 direction))
            return false;

        Vector3 forward = rigid.rotation * Vector3.forward;

        float angle = Vector3.Angle(forward, direction);

        return angle > _allowedAngle;  // 쳐다보는 각도가 설정값보다 넘어가면 몸통 회전이 필요함
    }

    public void TurnBody()
    {
        if (!TryGetPlayerDirection(out Vector3 direction))
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        Quaternion nextRotation = Quaternion.RotateTowards(rigid.rotation, targetRotation, bodyTurnSpeed * Time.fixedDeltaTime);

        rigid.MoveRotation(nextRotation);
    }

    //---------------------추적---------------------
    public bool NeedChase()
    {
        if (!TryGetPlayerDirection(out Vector3 direction))
            return false;

        return direction.sqrMagnitude > chaseDistance * chaseDistance;
    }

    public void MoveToPlayer()
    {
        Vector3 forward = rigid.rotation * Vector3.forward;

        Vector3 velocity = forward * moveSpeed;

        velocity.y = rigid.linearVelocity.y;

        rigid.linearVelocity = velocity;
    }

    public void StopMove()
    {
        Vector3 velocity = rigid.linearVelocity;

        velocity.x = 0f;
        velocity.z = 0f;

        rigid.linearVelocity = velocity;
    }

    //-------------------물기---------------
    public bool CanBite()
    {
        if (!TryGetPlayerDirection(out Vector3 direction))
            return false;

        if (direction.sqrMagnitude > chaseDistance * chaseDistance)
            return false;

        Vector3 forward = rigid.rotation * Vector3.forward;

        float angle = Vector3.Angle(forward, direction);

        return angle <= biteAngle;
    }
}
