using UnityEngine;

public class RunState : PlayerState
{
    private readonly PlayerInputReader inputReader;
    public RunState(PlayerAnimationEvents _animationEvents, Animator _animator, PlayerStateAction _playerStateAction, PlayerInputReader _inputReader)
        : base(_animationEvents, _animator, _playerStateAction)
    {
        inputReader = _inputReader;
    }

    public override void Enter()
    {
        playerStateAction.PlayerMoveAnimation("Run");
    }

    public override void Tick()
    {
        //Action();
    }

    public override void FixedTick()
    {
        playerStateAction.RunAction(inputReader.MoveInput);
    }

    public override void Exit()
    {
        playerStateAction.StopMoveAction();
    }

    public override void AnimationAction() // 리펙토링으로 제거 예정 실제 행동에서 처리할 것
    {

    }

    protected override void SetPossibleChangeState()
    {
        //possibleRequestTypes.Add(PlayerRequestType.NormalAttack);
        possibleRequestTypes.Add(PlayerRequestType.Rolling);
    }
}
