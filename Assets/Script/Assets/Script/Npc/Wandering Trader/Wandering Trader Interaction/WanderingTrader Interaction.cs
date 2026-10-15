using UnityEngine;

public class WanderingTraderInteraction : NpcTalkInteraction
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DecideTextIndex();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact(Transform _targetTransform)
    {
        base.Interact(_targetTransform);
    }

    public override void ExitInteraction()
    {
        base .ExitInteraction();
    }

    protected override void DecideTextIndex()
    {
        int questID = 3;

        QuestState questState = QuestManager.Instance.GetQuestState(questID);

        switch (questState)
        {
            case QuestState.Available:
                textIndex = 0; // 퀘스트 제안
                break;

            case QuestState.InProgress:
                textIndex = QuestManager.Instance.CanCompleteQuest(questID)
                    ? 2   // 완료 조건 달성
                    : 1;  // 아직 진행 중
                break;

            case QuestState.Completed:
                textIndex = 3; // 이미 완료한 뒤의 일반 대화
                break;
        }
    }
}
