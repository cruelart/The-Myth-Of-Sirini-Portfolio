using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IQuestStateReader
{
    QuestState GetQuestState(int _questID);

    bool CanCompleteQuest(int _questID);
}

[System.Serializable]
public class QuestProgressDataList
{
    public List<QuestProgressData> questProgressDatas;
}

public class QuestManager : MonoBehaviour, IQuestStateReader
{
    public static QuestManager Instance { get; private set; }

    //데이터 관련
    public QuestDataBase questDB { get; private set; } = new QuestDataBase(); // 전체 퀘스트 목록
    public PlayerQuestData playerQuestData { get; private set; } = new PlayerQuestData(); // 플레이어 퀘스트 진행 현황

    //private Dictionary<int, QuestProgressData> questProgressDataTable = new(); // (퀘스트 아이디(키), 모든 퀘스트 진행 현황) -> 플레이어 전용

    //private Dictionary<int, QuestData> questDataTable = new(); // 퀘스트 아이디(키), 해당 퀘스트 데이터 -> 모든 플레이어 공용 Complit성능괜찮네 ㄷ

    //이벤트
    public event Action<int, int> OnQuestProgressChanged;
    public event Action<QuestState, int> OnQuestStateChanged; // 변경된 퀘스트 상태, 퀘스트 id

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        questDB.LoadData(); // 전체 퀘스트 목록 불러오기

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        MonsterEvent.OnMonsterDead += HandleMonsterDead;
        InventoryEvent.OnOwnedItemCountChanged += HandleItemChanged;
    }

    private void OnDisable()
    {
        MonsterEvent.OnMonsterDead -= HandleMonsterDead;
        InventoryEvent.OnOwnedItemCountChanged -= HandleItemChanged;
    }

    //LoadManager에서 퀘스트를 받아옴
    public void InitLoadData(List<QuestProgressData> _questProgressDataList)
    {
        if(_questProgressDataList.Count == 0) // 로드했는데 데이터가 비어있다면 -> 파일이 없다는거겠쬬?
        {
            playerQuestData.Init(questDB.QuestDataTable.Values);
            return;
        }
        playerQuestData.LoadQuestProgressData(_questProgressDataList);
        //-> 여기까지 했으면 퀘스트의 전체적인 것들은 모두 받아온 셈
    }

    public QuestProgressDataList GetQuestProgressDataList()
    {
        QuestProgressDataList data = new QuestProgressDataList();

        data.questProgressDatas = new List<QuestProgressData>(playerQuestData.PlayerQuestProgressTable.Values);

        return data;
    }

    public void AcceptQuest(int _questID)
    {         
        if(!playerQuestData.AvailableQuests.Contains(_questID))
        {
            Debug.LogError($"시작가능한 퀘스트가 아닙니다, 수락불가능합니다");
            return; // 시작가능한 퀘스트에 존재하지도 않는데 어딜 감히
        }
        playerQuestData.AcceptQuest(questDB.GetQuestData(_questID));
        OnQuestStateChanged?.Invoke(QuestState.InProgress, _questID);
        GameEventChannel.OnNotify?.Invoke($"퀘스트 수락 : {questDB.GetQuestData(_questID).questName}");
        //OnQuestChangeNotify?.Invoke(QuestState.InProgress, _questID);
    }

    public void CompleteQuest(int _questID)
    {
        if (!playerQuestData.InProgressQuests.Contains(_questID))
        {
            Debug.LogError($"진행중인 퀘스트가 아닙니다, 완료 불가능합니다");
            return;
        }

        QuestData questData = questDB.GetQuestData(_questID);

        for(int i = 0; i < questData.requirements.Count; i++) // 퀘스트가 요구하는 모든 요구사항 갯수만큼
        {
            if(questData.requirements[i].requiredCount != playerQuestData.PlayerQuestProgressTable[_questID].requirementProgresses[i].currentCount)
            {
                GameEventChannel.OnNotify?.Invoke($"퀘스트 조건을 만족하지 못하여 퀘스트를 완료할 수 없습니다.");
                return;
            }
        }
        playerQuestData.CompleteQuest(questDB.GetQuestData(_questID));
        OnQuestStateChanged?.Invoke(QuestState.Completed, _questID);
        //OnQuestChangeNotify?.Invoke(QuestState.Completed, _questID);
        GameEventChannel.OnNotify?.Invoke($"퀘스트 완료 : {questDB.GetQuestData(_questID).questName}");

        //보상 제공
        RewardManager.Instance.GetMoneyReward(questData.ClearGold);
        RewardManager.Instance.GetItemReward(questData.ClearItem.itemID, 1);
        RewardManager.Instance.GetExpReward(questData.ClearExp);
    }

    public void GiveUpQuest(int _questID)
    {
        if (!playerQuestData.InProgressQuests.Contains(_questID))
        {
            Debug.LogError($"진행중인 퀘스트가 아닙니다, 포기 불가능합니다");
            return;
        }
        playerQuestData.GiveUpQuest(questDB.GetQuestData(_questID));
        //OnQuestChangeNotify?.Invoke(QuestState.Available, _questID);
    }

    //--------------------------------인터페이스 구현----------------------------

    public QuestState GetQuestState(int _questID)
    {
        return playerQuestData.GetQuestState(_questID);
    }

    public bool CanCompleteQuest(int _questID) // 해당 퀘스트가 클리어가 가능한 상태인가 함수
    {
        if(GetQuestState(_questID) != QuestState.InProgress) // 진행중이 아니면 바로 return
        {
            return false;
        }

        QuestData questData = questDB.GetQuestData(_questID);

        if(questData == null) // 해당 퀘스트 ID가 존재조차 하지 않는다면 return
        {
            return false;
        }

        if(!playerQuestData.PlayerQuestProgressTable.TryGetValue(_questID, out var progressTable)) // 퀘스트 진행도쪽에 퀘스트 존재하지 않으면 return
        {
            return false;
        }

        for(int i = 0; i < questData.requirements.Count; i++)
        {
            int requiredCount = questData.requirements[i].requiredCount;

            int currentCount = progressTable.requirementProgresses[i].currentCount;

            if(currentCount < requiredCount)
            {
                return false;
            }
        }

        return true;
    }

    //------------------------------------------------------------------------

    private void HandleMonsterDead(MonsterDeadInfo info)
    {
        ApplyQuestProgress(QuestRequirementType.Kill, QuestTargetType.Monster, info.monsterID,1);
    }

    private void HandleItemChanged(int _itemID, int _amount)
    {
        ApplyQuestProgress(QuestRequirementType.CollectItem, QuestTargetType.Item, _itemID, _amount);
    }

    //private void HandleNpcTalkCompleted(NpcTalkInfo info)
    //{
    //    ApplyQuestProgress(
    //        QuestRequirementType.TalkNpc,
    //        info.npcID,
    //        1,
    //        QuestProgressUpdateMode.Set);
    //}

    private void ApplyQuestProgress(QuestRequirementType _requireType, QuestTargetType _targetType, int _targetID, int _value)
    {
        IReadOnlyList<QuestRequirementRef> changed = playerQuestData.UpdateQuestInProgress(_requireType, _targetType, _targetID, _value); // 교체된 리스트

        foreach (QuestRequirementRef requirementRef in changed)
        {
            OnQuestProgressChanged?.Invoke(requirementRef.questID,requirementRef.requirementIndex); // 해당 퀘스트 요구사항 requirementIndex번째에 있는 것이 변동사항 있다.
        }
    }
}
