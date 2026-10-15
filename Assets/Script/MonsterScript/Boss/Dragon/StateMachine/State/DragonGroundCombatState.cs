using UnityEngine;
using BehaviorTree;

public class DragonGroundCombatState : DragonState
{
    private readonly Node rootNode;

    private bool turnRequested; 
    private bool chaseRequested;
    private bool isBite;

    private float turnTime = 0.0f; // 일정 시간동안 계속 움직인다면 하늘로 올라가기 위한 변수
    public DragonGroundCombatState(DragonAIController _aiController, DragonStateMachine _stateMachine)
       : base(_aiController, _stateMachine)
    {
        SelectorNode selector = new SelectorNode();

        //ConditionNode turnNode = new ConditionNode(TryTurnBody, new ActionNode(Turn));

        ConditionNode chaseNode = new ConditionNode(aiController.NeedChase, new ActionNode(Chase));
        ConditionNode biteNode = new ConditionNode(()=> isBite || aiController.CanBite(), new ActionNode(Bite));

        //selector.ListAdd(turnNode);
        selector.ListAdd(biteNode);
        selector.ListAdd(chaseNode);

        selector.ListAdd(new ActionNode(Wait));

        rootNode = selector;
    }

    public override void Enter()
    {
        isBite = false;
        turnRequested = false;
        chaseRequested = false;

        aiController.PlayAnimation("Walk", 0.1f);
        aiController.RigController.SetTurnWeight(1.0f); // 드래곤이 땅에 있으면 드래곤의 전체적인 몸이 플레이어 따라가게 설정 -> 공중상태는 0으로할예정

        aiController.AnimationEvents.BiteFinished += HandleBiteEnd;
        aiController.AnimationEvents.BiteStart += HandleBiteStart;
    }

    public override void Tick()
    {
        rootNode.Evaluate();
    }

    public override void FixedTick()
    {
        if (chaseRequested) // 추적 이동이 요청됐다면
        {
            aiController.TurnBody();
            aiController.MoveToPlayer();
        }
        else // 추적하지 않는다면
            aiController.StopMove(); // 이전 수평 속도를 제거
    }

    public override void Exit()
    {
        isBite = false;
        turnRequested = false;
        chaseRequested = false;

        aiController.RigController.SetTurnWeight(0.0f);
        aiController.PlayAnimation("IdleSimple", 0.3f); // 완료 후 대기로 전환한다.

        aiController.AnimationEvents.BiteFinished -= HandleBiteEnd;
        aiController.AnimationEvents.BiteStart -= HandleBiteStart;
    }

    private bool TryTurnBody()
    {
        float maxAngle = aiController.RigController.MaxViewAngle;

        float possibleAngle = turnRequested ? maxAngle * 0.6f : maxAngle; // 멈췄다가 다시 돌고 또 멈췄다가 다시돌고 이런 부자연스러운 움직임 제어하기 위해 추가

        return aiController.NeedBodyTurn(possibleAngle);
    }
    //private tasks Turn()
    //{
    //    turnRequested = true;

    //    return tasks.Success;
    //}

    private void HandleBiteStart()
    {
        isBite = true;
    }
    private void HandleBiteEnd()
    {
        isBite = false;

        aiController.RigController.SetTurnWeight(1f); 
    }

    private tasks Chase()
    {
        turnRequested = true;
        chaseRequested = true;

        aiController.RigController.SetTurnWeight(1.0f);

        aiController.PlayAnimation("Walk", 0.1f);

        return tasks.Success;
    }

    private tasks Wait()
    {
        turnRequested = false; // 회전 요청 해제
        chaseRequested = false; // 이동 요청 해제

        aiController.PlayAnimation("IdleSimple", 0.1f); // 대기 동작으로 전환

        return tasks.Success;
    }

    private tasks Bite()
    {
        if(isBite)
        {
            return tasks.Running;
        }

        turnRequested = false; 
        chaseRequested = false;

        aiController.StopMove();

        aiController.PlayAnimation("Bite", 0.1f); // 물기 전환

        return tasks.Running;
    }
}
