using UnityEngine;

public class DragonStateFactory
{
    public DragonBattleOffState battleOffState { get; }
    public DragonBattleOnState battleOnState { get; }
    public DragonGroundCombatState groundCombatState { get; }

    public DragonStateFactory(DragonAIController _aiController, DragonStateMachine _stateMachine)
    {
        battleOffState = new DragonBattleOffState(_aiController, _stateMachine);
        battleOnState = new DragonBattleOnState(_aiController, _stateMachine);
        groundCombatState = new DragonGroundCombatState(_aiController, _stateMachine);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
