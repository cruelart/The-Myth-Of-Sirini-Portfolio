using System;
using System.Reflection;
using UnityEngine;

public sealed class NormalAttackState : PlayerState
{
    private int comboIndex = 1; // 실행할 콤보 인덱스
    private bool nextAttackQueued;
    public NormalAttackState(PlayerAnimationEvents _animationEvents, Animator _animator, PlayerStateAction _playerStateAction) : base(_animationEvents, _animator, _playerStateAction)
    {
    }

    public override void Enter()
    {
        comboIndex = 1;
        nextAttackQueued = false;
        IsPossibleChangeState = false;

        animationEvents.OnPossibleChangeState += PossibleChangeState;
        animationEvents.OnImPossibleChangeState += ImPossibleChangeState;
        animationEvents.OnEndAnimation += HandleAttackAnimationEnd;

        AnimationAction();
    }

    public override void Tick()
    {
        //Action();
    }

    public override void FixedTick()
    {
        
    }

    public override void Exit()
    {
        animationEvents.OnPossibleChangeState -= PossibleChangeState;
        animationEvents.OnImPossibleChangeState -= ImPossibleChangeState;
        animationEvents.OnEndAnimation -= HandleAttackAnimationEnd;

        comboIndex = 1; 
        nextAttackQueued = false;
        IsPossibleChangeState = false;
    }

    public override void AnimationAction()
    {
        switch (comboIndex)
        {
            case 1:
                animator.CrossFade("NormalAttack1", 0.01f);
                break;

            case 2:
                animator.CrossFade("NormalAttack2", 0.1f);
                break;

            case 3:
                animator.CrossFade("NormalAttack3", 0.1f);
                break;

            case 4:
                animator.CrossFade("NormalAttack4", 0.1f);
                break;

            case 5:
                animator.CrossFade("NormalAttack5", 0.1f);
                break;

            case 6:
                animator.CrossFade("NormalAttack6", 0.1f);
                break;

        }
    }

    private void InitComboIndex()
    {
        comboIndex = 1;
    }

    protected override void SetPossibleChangeState()
    {
        possibleRequestTypes.Add(PlayerRequestType.None);
        possibleRequestTypes.Add(PlayerRequestType.NormalAttack);
    }

    private void HandleAttackAnimationEnd()
    {
        if (nextAttackQueued && comboIndex < 6)
        {
            comboIndex++;

            nextAttackQueued = false;
            IsPossibleChangeState = false;

            AnimationAction();
            return;
        }
        Debug.Log("endAnimation 호출");
        EndAnimation();
    }

    public override bool TryHandleSameStateRequest(PlayerRequestType _type)
    {
        if (_type != PlayerRequestType.NormalAttack)
        {
            return false;
        }

        if (!IsPossibleChangeState)
        {
            return false;
        }

        if (nextAttackQueued)
        {
            return false;
        }

        if (comboIndex >= 6)
        {
            return true;
        }

        nextAttackQueued = true;

        // 같은 입력 여러 번 예약 불가능 처리
        IsPossibleChangeState = false;

        return true;
    }

}
