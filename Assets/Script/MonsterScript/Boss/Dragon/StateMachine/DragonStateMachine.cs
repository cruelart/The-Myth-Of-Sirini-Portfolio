using UnityEngine;

//public enum DragonStateType
//{
//    BattleOff,
//    BattleOn,
//    GroundCombat,
//    TakeOff,
//    AirCombat,
//    Landing,
//    Recovery,
//    Die
//}
public class DragonStateMachine
{
    public DragonState currentState { get; private set; }
    public DragonStateFactory stateFactory { get; }

    public DragonStateMachine(DragonAIController _aiController)
    {
        stateFactory = new DragonStateFactory(_aiController, this);
    }

    public void Init()
    {
        ChangeState(stateFactory.battleOffState);
    }

    public void ChangeState(DragonState _nextState)
    {
        if (_nextState == null || _nextState == currentState)
            return;

        currentState?.Exit();

        currentState = _nextState;

        currentState?.Enter();
    }

    public void Tick()
    {
        currentState?.Tick();
    }

    public void FixedTick()
    {
        currentState?.FixedTick();
    }
}
