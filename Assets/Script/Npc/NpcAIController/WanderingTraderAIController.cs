using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

//Read Me

//--> 구성요소 애니메이션(상태 전환별 한번 호출용) , 행동트리(지속 호출)

public enum NpcState
{
    Idle,
    Move,
    Interact
}

public class WanderingTraderAIController : NpcAIController
{

    [SerializeField]
    private NavigationNode startNode; // 시작점 노드 설정

    private WanderingTraderNav wanderingTraderNav;

    protected override void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        wanderingTraderNav = new WanderingTraderNav(startNode, agent);
    }
    void Start()
    {
        this.transform.position = startNode.transform.position; // Npc의 시작 위치 설정

        SelectorNode rootSelector = new SelectorNode();

        ConditionNode interactionNode = new ConditionNode(
            () => npcState == NpcState.Interact, new ActionNode(Interaction));

        ConditionNode moveNode = new ConditionNode(
            () => npcState == NpcState.Move, new ActionNode(Move));

        rootSelector.ListAdd(interactionNode);
        rootSelector.ListAdd(moveNode);

        root = rootSelector;

        wanderingTraderNav.MoveNextNode();
        //ChangeNpcState(NpcState.Move);
    }

    // Update is called once per frame
    void Update()
    {
        root.Evaluate();
    }

    public override void ChangeNpcState(NpcState _newState)
    {
        if (npcState == _newState)
            return;

        npcState = _newState;
        
        PlayAnime(npcState);
    }

    private void PlayAnime(NpcState _newState)
    {
        switch(_newState)
        {
            case NpcState.Interact:
                agent.isStopped = true;
                animator.CrossFade("Interaction", 0.2f);
                break;

            case NpcState.Move:
                agent.isStopped = false;
                animator.CrossFade("Move", 0.2f);
                break;

            case NpcState.Idle:
                agent.isStopped = false;
                animator.CrossFade("Move", 0.2f);
                break;
        }
    }

    public void SetTargetTransform(Transform _targetTransform)
    {
        targetTransform = _targetTransform;
    }

    tasks Move()
    {
        if(wanderingTraderNav.IsArrived())
        {
            wanderingTraderNav.currentNode = wanderingTraderNav.nextNode;
            wanderingTraderNav.MoveNextNode();
            return tasks.Success;
        }
        return tasks.Running;
    }

    tasks Interaction()
    {
        //플레이어를 쳐다보기
        Quaternion targetRotation = Quaternion.LookRotation(targetTransform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);

        return tasks.Running;
    }
}
