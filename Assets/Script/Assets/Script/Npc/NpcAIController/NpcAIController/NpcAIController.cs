using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;

public abstract class NpcAIController : MonoBehaviour
{
    protected Transform targetTransform;

    protected NpcState npcState = NpcState.Move; // NPC ����

    protected Node root; // ��Ʈ���

    protected NavMeshAgent agent;

    //private WanderingTraderNav wanderingTraderNav;

    protected Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public abstract void ChangeNpcState(NpcState _newState);

    public void NpcInteraction(Transform _transform)
    {
        targetTransform = _transform;
        ChangeNpcState(NpcState.Interact);
    }

    public void NpcExitInteraction()
    {
        ChangeNpcState(NpcState.Move);
    }
}
