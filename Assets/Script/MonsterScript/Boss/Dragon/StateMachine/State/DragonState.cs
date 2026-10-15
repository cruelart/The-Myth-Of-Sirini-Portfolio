using UnityEngine;

public abstract class DragonState
{
    protected readonly DragonAIController aiController;
    protected readonly DragonStateMachine stateMachine;

    protected DragonState(DragonAIController _aiController, DragonStateMachine _dragonStateMachine)
    {
        this.aiController = _aiController;
        this.stateMachine = _dragonStateMachine;
    }

    public abstract void Enter();

    public abstract void Tick();

    public virtual void FixedTick() { }

    public abstract void Exit();
}
