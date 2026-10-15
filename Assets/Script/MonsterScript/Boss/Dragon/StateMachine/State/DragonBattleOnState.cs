using UnityEngine;

public class DragonBattleOnState : DragonState
{
    public DragonBattleOnState(DragonAIController _aiController,DragonStateMachine _stateMachine)
        : base(_aiController, _stateMachine)
    {
    }

    public override void Enter()
    {
        aiController.AnimationEvents.RoarFinished += HandleStartBattle; // 포효가 끝나면 본격적인 배틀시작
        aiController.PlayAnimation("BattleOn", 0.1f);
    }

    public override void Tick()
    {
        // 다음 단계에서 포효 완료 처리를 연결
    }

    public override void Exit()
    {
        aiController.AnimationEvents.RoarFinished -= HandleStartBattle;
    }
    private void HandleStartBattle()
    {
        stateMachine.ChangeState(stateMachine.stateFactory.groundCombatState);
    }
}
