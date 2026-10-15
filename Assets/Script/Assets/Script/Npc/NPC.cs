using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    protected NPCData npcData;

    protected NpcAIController npcAIController;
    protected NpcTalkInteraction npcTalkInteraction;

    protected virtual void Awake()
    {
        npcAIController = GetComponent<NpcAIController>();
        npcTalkInteraction = GetComponent<NpcTalkInteraction>();

        npcTalkInteraction.Init(npcData.npcTalkDatas);
    }

    protected virtual void OnEnable()
    {
        npcTalkInteraction.OnNpcInteraction += npcAIController.NpcInteraction;
        npcTalkInteraction.OffNpcInteraction += npcAIController.NpcExitInteraction;
    }

    protected virtual void OnDisable()
    {
        npcTalkInteraction.OnNpcInteraction -= npcAIController.NpcInteraction;
        npcTalkInteraction.OffNpcInteraction -= npcAIController.NpcExitInteraction;
    }

    public Transform npcTransform { get; private set; }
    
    protected void NpcInit()
    {
        npcTransform = this.transform;
        //NpcManager.Instance.AddNpc(npcData.npcName, this);
    }

}
