using UnityEngine;

public class DeathState : PlayerState
{
    public DeathState(PlayerAnimationEvents _animationEvents, Animator _animator, PlayerStateAction _playerStateAction) : base(_animationEvents, _animator, _playerStateAction)
    {

    }

    public override void Enter()
    {
        IsPossibleChangeState = false;

        //animationEvents.OnEndAnimation += HandleAnimationEnd;

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
        //animationEvents.OnEndAnimation -= HandleAnimationEnd;
        playerStateAction.StopMoveAction();
    }

    public override void AnimationAction()
    {
        animator.CrossFadeInFixedTime("Death", 0.1f, 0, 0.0f);
    }

    protected override void SetPossibleChangeState()
    {

    }
    private void HandleAnimationEnd()
    {
        EndAnimation();
    }
}
