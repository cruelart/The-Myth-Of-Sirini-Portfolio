using UnityEngine;

public class RollingState : PlayerState
{
    public RollingState(PlayerAnimationEvents _animationEvents, Animator _animator, PlayerStateAction _playerStateAction) : base(_animationEvents, _animator, _playerStateAction)
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
        playerStateAction.RollingAction();
    }

    public override void Exit()
    {
        animationEvents.OnEndAnimation -= HandleAnimationEnd;
        playerStateAction.StopMoveAction();
    }

    public override void AnimationAction()
    {
        animator.CrossFadeInFixedTime("Rolling", 0.1f, 0, 0.0f);
    }

    protected override void SetPossibleChangeState()
    {
        possibleRequestTypes.Add(PlayerRequestType.NormalAttack);
        possibleRequestTypes.Add(PlayerRequestType.Walk);
        possibleRequestTypes.Add(PlayerRequestType.Run);
        possibleRequestTypes.Add(PlayerRequestType.None);
    }
    private void HandleAnimationEnd()
    {
        EndAnimation();
    }
}
