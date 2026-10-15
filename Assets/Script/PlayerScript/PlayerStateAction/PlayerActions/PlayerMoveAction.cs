using UnityEngine;

public class PlayerMoveAction : PlayerAction
{
    private float moveSpeed = 2.0f; // 일반 속도
    private float runRateSpeed = 1.5f; // 달리기 속도 비율
    private float rotationSpeed = 720.0f;
    private readonly Transform cameraTransform;

    //애니메이션 속도 조절
    private static readonly int MoveAnimationSpeedHash = Animator.StringToHash("MoveAnimationSpeed");
    private const float baseAnimeSpeed = 40.0f;

    public PlayerMoveAction(Animator _animator, Rigidbody _rigidbody, Transform _cameraTransform, PlayerStatManager _statManager)
    {
        this.animator = _animator;
        this.rigid = _rigidbody;

        cameraTransform = _cameraTransform;

        moveSpeed = _statManager.GetStatValue(StatType.MoveSpeed);
        _statManager.OnMoveSpeedChanged += ChangeMoveSpeed;
        ChangeMoveSpeed(moveSpeed);
    }

    //public void Init(PlayerStatManager _statManager)
    //{
    //    _statManager.OnMoveSpeedChanged += ChangeMoveSpeed;
    //}

    //----------------------------------------걷기---------------------------------------------------------------

    public override void PlayAnimation(string _animationName)
    {
        animator.CrossFadeInFixedTime(_animationName, 0.1f, 0,  0.0f);
    }

    public void WalkMove(Vector2 _moveInput)
    {
        //카메라를 기준으로 방향이동 ㄱ
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0.0f;
        right.y = 0.0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * _moveInput.y + right * _moveInput.x; // 2차원 키보드 입력을 3ㅊ차원 변ㄱ뎡
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);

            Quaternion nextRotation = Quaternion.RotateTowards(rigid.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            rigid.MoveRotation(nextRotation);
        }

        Vector3 currentVelocity = rigid.linearVelocity;

        rigid.linearVelocity = new Vector3(moveDir.x * moveSpeed, currentVelocity.y, moveDir.z * moveSpeed);
    }

    public void RunMove(Vector2 _moveInput)
    {
        //카메라를 기준으로 방향이동 ㄱ
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0.0f;
        right.y = 0.0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * _moveInput.y + right * _moveInput.x; // 2차원 키보드 입력을 3ㅊ차원 변ㄱ뎡
        moveDir = Vector3.ClampMagnitude(moveDir, 1f);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);

            Quaternion nextRotation = Quaternion.RotateTowards(rigid.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            rigid.MoveRotation(nextRotation);
        }

        Vector3 currentVelocity = rigid.linearVelocity;

        rigid.linearVelocity = new Vector3(moveDir.x * moveSpeed * runRateSpeed, currentVelocity.y, moveDir.z * moveSpeed * runRateSpeed);
    }

    public void RollingMove()
    {
        Vector3 rollingDirection = rigid.transform.forward;

        rollingDirection.y = 0f;
        rollingDirection.Normalize();

        Vector3 currentVelocity = rigid.linearVelocity;

        rigid.linearVelocity = new Vector3(rollingDirection.x * moveSpeed * 1.5f, currentVelocity.y, rollingDirection.z * moveSpeed * 1.5f);
    }

    public void StopMove()
    {
        Vector3 currentVelocity = rigid.linearVelocity;

        rigid.linearVelocity = new Vector3(0f,currentVelocity.y,0f);
    }

    //--------------------------------------------------------------------------------------------------------

    private void ChangeMoveSpeed(float _moveSpeed)
    {
        moveSpeed = _moveSpeed;
        float animationSpeed = Mathf.Min(2.2f, (0.1f + ((baseAnimeSpeed + moveSpeed) / baseAnimeSpeed)));

        animator.SetFloat(MoveAnimationSpeedHash,animationSpeed);
    }
}
