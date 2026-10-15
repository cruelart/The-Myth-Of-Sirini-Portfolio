using UnityEngine;

public class KnockDownState : PlayerState
{
    public KnockDownState(PlayerAnimationEvents _animationEvents, Animator _animator, PlayerStateAction _playerStateAction) : base(_animationEvents, _animator, _playerStateAction)
    {

    }

    public override void Enter()
    {
        IsPossibleChangeState = false;

        animationEvents.OnEndAnimation += HandleAnimationEnd;

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
    }

    public override void AnimationAction()
    {
        animator.CrossFadeInFixedTime("KnockDown", 0.1f, 0, 0.0f);
    }

    protected override void SetPossibleChangeState()
    {
    }
    private void HandleAnimationEnd()
    {
        EndAnimation();
    }
}
