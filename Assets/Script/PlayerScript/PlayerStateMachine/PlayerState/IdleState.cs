using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerAnimationEvents _animationEvents, Animator _animator, PlayerStateAction _playerStateAction) : base(_animationEvents, _animator, _playerStateAction)
    {
        
    }

    public override void Enter()
    {
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
        animator.CrossFadeInFixedTime("Idle", 0.3f, 0, 0.0f);
    }

    protected override void SetPossibleChangeState()
    {
        possibleRequestTypes.Add(PlayerRequestType.NormalAttack);
        possibleRequestTypes.Add(PlayerRequestType.Walk);
        possibleRequestTypes.Add(PlayerRequestType.Run);
        possibleRequestTypes.Add(PlayerRequestType.Rolling); 
        possibleRequestTypes.Add(PlayerRequestType.Skill);
        possibleRequestTypes.Add(PlayerRequestType.WeaponToggle);
    }
}
