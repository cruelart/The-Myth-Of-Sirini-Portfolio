using UnityEngine;

public class DragonBattleOffState : DragonState
{
    public DragonBattleOffState(DragonAIController _aiController, DragonStateMachine _dragonStateMachine) : base(_aiController, _dragonStateMachine)
    {
    }

    public override void Enter()
    {
        aiController.PlayAnimation("IdleSimple", 0.1f);
    }

    public override void Tick()
    {
        if (!aiController.IsFindPlayer()) // 플레이어를 찾았다면
            return;

        stateMachine.ChangeState(stateMachine.stateFactory.battleOnState); // 배틀시작 폼 전환
    }

    public override void Exit()
    {
        
    }
}
