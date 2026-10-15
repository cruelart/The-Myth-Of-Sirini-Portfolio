using System.Xml.Linq;
using UnityEngine;

public class WanderingTrader : NPC
{
    private WanderingTraderAIController wanderingTraderAIController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        NpcInit();
        wanderingTraderAIController = GetComponent<WanderingTraderAIController>();
    }

    //public override void Interact(Transform _targetTransform)
    //{
    //    base.Interact(_targetTransform);

    //    wanderingTraderAIController.SetTargetTransform(_targetTransform.transform); // 상호작용하는 플레이어를 타겟으로 설정
    //    wanderingTraderAIController.ChangeNpcState(NpcState.Interact); // 해당 Npc 상호작용 모드로 변경
    //}

    //public override void ExitInteraction()
    //{
    //    base.ExitInteraction();
    //    wanderingTraderAIController.ChangeNpcState(NpcState.Move); // 떠돌이 상인 Npc 이동 모드로 변경
    //}

    //protected override void DecideTextIndex()
    //{
    //    //questIndex = 0;
    //    //QuestState questState0 = QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[npcData.npcQuestIDs[0]].questState;

    //    //if(questState0 == QuestState.Available)
    //    //{
    //    //    textIndex = 0; // 아직 퀘스트를 받기 전이니까 0으로
    //    //}
    //    //else if(questState0 == QuestState.InProgress)
    //    //{
    //    //    textIndex = 1; //받은 상태니까 1로
    //    //}
    //    //else if(questState0 == QuestState.Completed)
    //    //{
    //    //    textIndex = 2; // 완료했으니까 2로
    //    //}
    //}
}
