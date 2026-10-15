using UnityEngine;

public class QuestUI : UIBase
{
    //퀘스트 설명 패널 적는 곳

    //시작가능 퀘스트
    [SerializeField]
    private QuestAvailableUI questAvailableUI;

    //진행중 퀘스트

    [SerializeField]
    private QuestInProgressUI questInProgressUI;

    //완료 퀘스트
    [SerializeField]
    private QuestCompleteUI questCompletedUI;

    private void Awake()
    {
        //QuestManager.Instance.OnQuestChangeNotify += (state, questID) =>
        //{
        //    switch (state)
        //    {
        //        case QuestState.Available: // 퀘스트를 포기한 상태(진행중 ->시작가능)
        //            //진행중 퀘스트 UI에서 제거
        //            questInProgress.RemoveInProgressQuest(questID);
        //            questAvailable.AddAvailableQuest(QuestManager.Instance.questDB.QuestDataTable[questID], QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[questID]);
        //            break;
        //        case QuestState.InProgress: // 퀘스트를 수락한 상태(시작가능 -> 진행중)
        //            questAvailable.RemoveAvailableQuest(questID);
        //            questInProgress.AddInProgressQuest(QuestManager.Instance.questDB.QuestDataTable[questID], QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[questID]);
        //            break;
        //        case QuestState.Completed: //  퀘스트를 완료한 상태(진행중 -> 완료)
        //            questInProgress.RemoveInProgressQuest(questID);
        //            questCompleted.AddCompleteQuest(QuestManager.Instance.questDB.QuestDataTable[questID], QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[questID]);
        //            break;
        //    }
        //};
        Init(); // 퀘스트 목록들 전부다 UI형식으로 불러오고

        //진행도 변경 구독해서 진행중인 퀘스트UI는 지속적으로 업데이트 예정
        QuestManager.Instance.OnQuestProgressChanged += (_questID, _index) =>
        {
            QuestState questState = QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[_questID].questState;

            switch (questState)
            {
                case QuestState.Available:
                    break;
                case QuestState.InProgress: // 퀘스트를 수락한 상태(시작가능 -> 진행중)
                    questInProgressUI.UpdateInProgressQuest(QuestManager.Instance.questDB.GetQuestData(_questID), QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[_questID]);
                    //questAvailableUI.RemoveAvailableQuest(_questID);
                    //questInProgressUI.AddInProgressQuest(QuestManager.Instance.questDB.QuestDataTable[_questID], QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[_questID]);
                    break;
                case QuestState.Completed:
                    break;
            }
        };

        //퀘스트 진행도 변경
        QuestManager.Instance.OnQuestStateChanged += (_questState, _questID) =>
        {
            switch (_questState)
            {
                case QuestState.Available: // 퀘스트를 포기한 상태(진행중 ->시작가능)
                    //진행중 퀘스트 UI에서 제거
                    questInProgressUI.RemoveInProgressQuest(_questID);
                    questAvailableUI.AddAvailableQuest(QuestManager.Instance.questDB.QuestDataTable[_questID], QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[_questID]);
                    break;
                case QuestState.InProgress: // 퀘스트를 수락한 상태(시작가능 -> 진행중)
                    questAvailableUI.RemoveAvailableQuest(_questID);
                    questInProgressUI.AddInProgressQuest(QuestManager.Instance.questDB.QuestDataTable[_questID], QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[_questID]);
                    break;
                case QuestState.Completed: //  퀘스트를 완료한 상태(진행중 -> 완료)
                    questInProgressUI.RemoveInProgressQuest(_questID);
                    questCompletedUI.AddCompleteQuest(QuestManager.Instance.questDB.QuestDataTable[_questID], QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[_questID]);
                    break;
            }
        };
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        //시작가능 퀘스트 UI 초기화
        foreach(var questID in QuestManager.Instance.playerQuestData.AvailableQuests)
        {
            questAvailableUI.AddAvailableQuest(QuestManager.Instance.questDB.QuestDataTable[questID], QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[questID]);
        }

        //진행중 퀘스트 UI 초기화
        foreach(var questID in QuestManager.Instance.playerQuestData.InProgressQuests)
        {
            questInProgressUI.AddInProgressQuest(QuestManager.Instance.questDB.QuestDataTable[questID], QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[questID]);
        }

        //완료 퀘스트 UI 초기화
        foreach(var questID in QuestManager.Instance.playerQuestData.CompleteQuests)
        {
            questCompletedUI.AddCompleteQuest(QuestManager.Instance.questDB.QuestDataTable[questID], QuestManager.Instance.playerQuestData.PlayerQuestProgressTable[questID]);
        }
    }
}
