using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerQuestData
{
    private Dictionary<int, QuestProgressData> playerQuestProgressTable = new(); // 모든 퀘스트에 대한 플레이어의 진행상황 데이터

    private HashSet<int> available_quests = new(); // 플레이어가 시작가능한 퀘스트들
    private HashSet<int> inProgress_quests = new(); // 플레이어가 진행중인 퀘스트들
    private HashSet<int> completed_quests = new(); // 플레이어가 완료한 퀘스트들

    //읽는 것만 허용
    public IReadOnlyDictionary<int, QuestProgressData> PlayerQuestProgressTable => playerQuestProgressTable;
    public IReadOnlyCollection<int> AvailableQuests => available_quests;
    public IReadOnlyCollection<int> InProgressQuests => inProgress_quests;  
    public IReadOnlyCollection<int> CompleteQuests => completed_quests;

    //퀘스트 요구사항 빠른 접근을 위한 변수선언
    private Dictionary<int, List<QuestRequirementRef>> monsterKillQuestMap = new(); // (몬스터id, 그 몬스터를 잡으라는 퀘스트 id + 대응하는 인덱스값  모음집)
    private Dictionary<int, List<QuestRequirementRef>> itemCollectQuestMap = new(); // (아이템id, 해당 아이템을 가져오라는 퀘스트 id 모음집)

    //저장된 데이터가 없다면 이 함수를 호출ㄱ
    public void Init(IEnumerable<QuestData> _allQuestDatas)
    {
        //전체적으로 비우고 (어쩌피 초기화할거니까)
        playerQuestProgressTable.Clear();
        available_quests.Clear();
        inProgress_quests.Clear();
        completed_quests.Clear();

        foreach (QuestData questData in _allQuestDatas)
        {
            QuestProgressData progress = new QuestProgressData(questData);
            playerQuestProgressTable.Add(questData.questID,progress);

            available_quests.Add(questData.questID);
        }
    }

    //퀘스트 로드
    public void LoadQuestProgressData(List<QuestProgressData> _questProgressDataList)
    {
        playerQuestProgressTable = _questProgressDataList.ToDictionary(x => x.questID);

        available_quests.Clear();
        inProgress_quests.Clear();
        completed_quests.Clear();

        foreach(var questProgressData in _questProgressDataList)
        {
            switch(questProgressData.questState)
            {
                case QuestState.Available:
                    available_quests.Add(questProgressData.questID);
                    break;
                case QuestState.InProgress:
                    inProgress_quests.Add(questProgressData.questID);
                    break;
                case QuestState.Completed:
                    completed_quests.Add(questProgressData.questID);
                    break;
            }
        }
    }

    public List<QuestRequirementRef> UpdateQuestInProgress(QuestRequirementType _requireType, QuestTargetType _targetType, int _targetID, int _value)
    {
        switch (_requireType)
        {
            case QuestRequirementType.Kill:
                switch (_targetType)
                {
                    case QuestTargetType.Monster:
                        if(UpdateInProgress(QuestProgressUpdateMode.Add, monsterKillQuestMap, _targetID, _value))
                            return monsterKillQuestMap[_targetID];
                        break;

                    case QuestTargetType.Npc:
                        break;
                }
                break;

            case QuestRequirementType.CollectItem:
                if (UpdateInProgress(QuestProgressUpdateMode.Set, itemCollectQuestMap, _targetID, _value))
                    return itemCollectQuestMap[_targetID];
                break;

        }

        return new List<QuestRequirementRef>();
    }

    private bool UpdateInProgress(QuestProgressUpdateMode _udpateMode, Dictionary<int, List<QuestRequirementRef>> _map, int _targetID, int _value)
    {
        if(!_map.TryGetValue(_targetID, out List<QuestRequirementRef> _questRequirements))
        {
            Debug.Log($"해당 타겟ID {_targetID}에 대한 진행중인 퀘스트가 존재하지 않습니다.");
            return false;
        }

        foreach (var questRequirementRef in _questRequirements)
        {
            int requireCount = playerQuestProgressTable[questRequirementRef.questID].requirementProgresses[questRequirementRef.requirementIndex].requireCount;
            int currentCount = playerQuestProgressTable[questRequirementRef.questID].requirementProgresses[questRequirementRef.requirementIndex].currentCount;
            bool isCompleted = playerQuestProgressTable[questRequirementRef.questID].requirementProgresses[questRequirementRef.requirementIndex].isCompleted;

            int extraCount = 0;

            switch (_udpateMode)
            {
                case QuestProgressUpdateMode.Add:

                    extraCount = currentCount + _value;
                    break;
                case QuestProgressUpdateMode.Set:
                    extraCount = _value;
                    break;
            }

            if (extraCount >= requireCount)
            {
                extraCount = requireCount;
                isCompleted = true;
            }
            else
            {
                isCompleted = false;
            }

            playerQuestProgressTable[questRequirementRef.questID].requirementProgresses[questRequirementRef.requirementIndex].currentCount = extraCount;
        }

        return true;
    }

    //-> 이 함수가 monsterkill, npc대화등 접근을 빠르게 하기 위한 해시를 채워넣을 예정(Accept Quest로 퀘스트를 수락하게되면 등록하기위함)
    private void RegisterQuestRequirements(QuestData _questData)
    {
        //퀘스트가 요구하는 것들을 전부 순회
        for(int i = 0; i < _questData.requirements.Count; i++)
        {
            QuestRequirement questRequirement = _questData.requirements[i];

            switch(questRequirement.requireType)
            {
                case QuestRequirementType.Kill:
                    //if(questRequirement.targetType == QuestTargetType.Monster) // 죽이고자 하는 것이 몬스터라면
                    //{
                    //    if (!monsterKillQuestMap.TryGetValue(questRequirement.targetID, out List<QuestRequirementRef> refs))
                    //    {
                    //        refs = new List<QuestRequirementRef>(); // 없으면 새롭게 할당하고

                    //        monsterKillQuestMap[questRequirement.targetID] = refs;
                    //    }

                    //    refs.Add(new QuestRequirementRef(_questData.questID, i)); // 할당한 곳에다가 퀘스트 아이디와, 해당 요구사항 인덱스 넣어주기
                    //} -> case별로 호춯할 것이므로 RegisterQuestRequirement 함수를 생성해서 호출할 예정
                    if (questRequirement.targetType == QuestTargetType.Monster) // 죽이고자 하는 것이 몬스터라면
                    {
                        RegisterQuestRequirement(monsterKillQuestMap, questRequirement.targetID, _questData.questID, i);
                    }
                    break;

                case QuestRequirementType.CollectItem:
                    //if(questRequirement.targetType == QuestTargetType.Item) // 퀘스트가 요구하는 타겟이 아이템이라면 -> 이건 Kill에서만 체크할 예정
                    
                    RegisterQuestRequirement(itemCollectQuestMap, questRequirement.targetID, _questData.questID, i);
                    
                    break;
            }
        }
    }

    private void RegisterQuestRequirement(Dictionary<int,List<QuestRequirementRef>> _map, int _targetID, int _questID, int _requireIndex)
    {
        if(!_map.TryGetValue(_targetID, out List<QuestRequirementRef> _questRequirements))
        {
            _questRequirements = new List<QuestRequirementRef>(); // 없으면 새로 할당 후

            _map.Add(_targetID, _questRequirements); // 넣어줌
        }

        _questRequirements.Add(new QuestRequirementRef(_questID, _requireIndex)); // 해당 퀘스트에 index에 필요한것이 있다고 추가
    }

    //-> 이 함수가 monsterkill, npc대화등 접근을 빠르게 하기 위한 해시에서 빠르게 제거 예정(Complete Quest와 GiveUp Quest(퀘스트 완료, 퀘스트 포기)로 퀘스트를 수락하게되면 등록하기위함)
    private void RemoveQuestRequirements(QuestData _questData)
    {
        for (int i = 0; i < _questData.requirements.Count; i++)
        {
            QuestRequirement requirement = _questData.requirements[i];

            switch (requirement.requireType)
            {
                case QuestRequirementType.Kill:
                    if (requirement.targetType == QuestTargetType.Monster)
                    {
                        UnregisterRequirement(monsterKillQuestMap, requirement.targetID,_questData.questID,i);
                    }
                    break;

                case QuestRequirementType.CollectItem:
                    UnregisterRequirement(itemCollectQuestMap,requirement.targetID,_questData.questID,i);
                    break;

            }
        }

    }

    private void UnregisterRequirement(Dictionary<int, List<QuestRequirementRef>> map,int targetID,int questID,int requirementIndex)
    {
        if (!map.TryGetValue(targetID,out List<QuestRequirementRef> refs))
        {
            Debug.LogError($"진행중인 퀘스트가 존재하지 않습니다 오류발생");
            return; // 애초에 있지도 않은 것을 지우려 했으니 return하고 오류발생 메세지 띄우자
        }

        refs.RemoveAll(r =>r.questID == questID && r.requirementIndex == requirementIndex);

        // 해당 targetID를 사용하는 퀘스트가 이제 하나도 없으면
        // Dictionary Key 자체도 제거
        if (refs.Count == 0)
        {
            map.Remove(targetID);
        }
    }

    //퀘스트 수락
    public void AcceptQuest(QuestData _questData)
    {
        int questID = _questData.questID;

        if (!playerQuestProgressTable.TryGetValue(questID, out QuestProgressData questProgressData))
        {
            Debug.LogError($"퀘스트 ID {questID}에 대한 진행 데이터가 없습니다.");
            return;
        }

        if(questProgressData.questState != QuestState.Available)
        {
            Debug.LogError($"해당 퀘스트는 시작가능한 퀘스트가 아니기 때문에 퀘스트를 수락할 수 없습니다,");
            return;
        }

        available_quests.Remove(questID); // 시작가능한 퀘스트에서 제거 처리

        questProgressData.questState = QuestState.InProgress; // 퀘스트 상태를 진행중으로 변경
        //나머지는 그대로 전달받음 -> 진행도는 0으로 시작했기때문

        inProgress_quests.Add(questID); // 진행중인 퀘스트에 추가 처리

        RegisterQuestRequirements(_questData);

    }

    //퀘스트 포기
    public void GiveUpQuest(QuestData _questData)
    {
        int questID = _questData.questID;

        if(!playerQuestProgressTable.TryGetValue(questID, out QuestProgressData questProgressData))
        {
            Debug.LogError($"퀘스트 ID {questID}에 대한 진행 데이터가 없습니다.");
            return;
        }
        if(questProgressData.questState != QuestState.InProgress)
        {
            Debug.LogError($"해당 퀘스트는 진행중이 아니기 때문에 포기할 수 없습니다.");
            return;
        }
        inProgress_quests.Remove(questID); // 진행중인 퀘스트에서 제거 처리

        questProgressData.questState = QuestState.Available; // 퀘스트 상태를 시작가능으로 변경

        foreach(var questRequireProgress in questProgressData.requirementProgresses)
        {
            questRequireProgress.isCompleted = false; // 완료가능해도 포기한다는거니까 취소
            questRequireProgress.currentCount = 0; // 다시 원상 복구
        }
        //questProgressData.currentCount = 0; // 진행상태 0으로 초기화

        available_quests.Add(questID); // 시작가능한 퀘스트에 추가 처리 -> 와.. ㅋㅋ

        RemoveQuestRequirements(_questData);
    }

    //퀘스트 완료
    public void CompleteQuest(QuestData _questData)
    {
        int questID = _questData.questID;

        if (!playerQuestProgressTable.TryGetValue(questID, out QuestProgressData questProgressData))
        {
            Debug.LogError($"퀘스트 ID {questID}에 대한 진행 데이터가 없습니다.");
            return;
        }
        if(questProgressData.questState != QuestState.InProgress)
        {
            Debug.LogError($"해당 퀘스트는 진행중이 아니기 때문에 완료할 수 없습니다.");
            return;
        }
        inProgress_quests.Remove(questID); // 진행중인 퀘스트에서 제거 처리

        questProgressData.questState = QuestState.Completed; // 퀘스트 상태를 완료로 변경
        //questProgressData.currentCount = 0; // 진행상태 0으로 초기화

        completed_quests.Add(questID); // 완료한 퀘스트에 추가 처리

        RemoveQuestRequirements(_questData);
    }

    public QuestState GetQuestState(int _questID)
    {
        return playerQuestProgressTable[_questID].questState;
    }

    ////퀘스트 요구사항 관련 함수 (게임 확성기 채널에서 띄울 목적)
    //public IReadOnlyCollection<int> GetQuestByMonsterID(int _monsterID)
    //{
    //    if(monsterKillQuestMap.TryGetValue(_monsterID, out var questIDs))
    //    {
    //        return questIDs;
    //    }

    //    return Array.Empty<int>();
    //}
}
