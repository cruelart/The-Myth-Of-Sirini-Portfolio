using UnityEngine;

public class MoveState : PlayerState
{
    private readonly PlayerInputReader inputReader;
    public MoveState(PlayerAnimationEvents _animationEvents, Animator _animator, PlayerStateAction _playerStateAction, PlayerInputReader _inputReader) 
        : base(_animationEvents, _animator, _playerStateAction)
    {
        inputReader = _inputReader;
    }

    public override void Enter()
    {
        playerStateAction.PlayerMoveAnimation("Walk");
    }

    public override void Tick()
    {
        //Action();
    }

    public override void FixedTick()
    {
        playerStateAction.WalkAction(inputReader.MoveInput);
    }

    public override void Exit()
    {
        playerStateAction.StopMoveAction();
    }

    public override void AnimationAction() // 리펙토링으로 제거 예정 실제 행동에서 처리할 것
    {
        animator.CrossFade("Walk", 0.2f);
    }

    protected override void SetPossibleChangeState()
    {
        possibleRequestTypes.Add(PlayerRequestType.NormalAttack);
        possibleRequestTypes.Add(PlayerRequestType.Run);
        possibleRequestTypes.Add(PlayerRequestType.Rolling);
        possibleRequestTypes.Add(PlayerRequestType.WeaponToggle);
    }
}
