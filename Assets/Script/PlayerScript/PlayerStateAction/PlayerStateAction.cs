using UnityEngine;

public enum PlayerStateActionType
{
    MoveAction,
    CombatAction,
    SkillAction
}

public class PlayerStateAction : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform; // 행동의 기준 Transform

    private Animator animator;
    private Rigidbody rigid;
    private bool useRootMotion;

    private PlayerMoveAction moveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init() // 무조건 한번만 호출
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        if (cameraTransform == null)
        {
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        if (moveAction != null)
        {
            return;
        }
        
        moveAction = new PlayerMoveAction(animator, rigid, cameraTransform, PlayerStatManager.Instance);
    }

    public void SetRootMotion(bool value)
    {
        useRootMotion = value;
    }

    private void OnAnimatorMove()
    {
        if (!useRootMotion || animator == null || rigid == null)
        {
            return;
        }

        Vector3 nextPosition = rigid.position + animator.deltaPosition;

        Quaternion nextRotation = rigid.rotation * animator.deltaRotation;

        rigid.MovePosition(nextPosition);
        rigid.MoveRotation(nextRotation);
    }

    //----------움직이는 명령----------------------------
    public void PlayerMoveAnimation(string _animationName)
    {
        if (moveAction == null)
        {
            return;
        }
        moveAction.PlayAnimation(_animationName);
    }

    public void WalkAction(Vector2 _inputValue)
    {
        if (moveAction == null)
        {
            return;
        }
        moveAction.WalkMove(_inputValue);
    }

    public void RunAction(Vector2 _inputValue)
    {
        if (moveAction == null)
        {
            return;
        }
        moveAction.RunMove(_inputValue);
    }

    public void RollingAction()
    {
        if(moveAction == null)
        {
            return;
        }
        moveAction.RollingMove();
    }

    public void StopMoveAction()
    {
        if (moveAction == null)
        {
            return;
        }
        moveAction.StopMove();
    }

    //--------------------------------------------------

    //-----------공격하는 명령------------------------

    public void CombatAction()
    {

    }

    //----------------------------------------------
}
