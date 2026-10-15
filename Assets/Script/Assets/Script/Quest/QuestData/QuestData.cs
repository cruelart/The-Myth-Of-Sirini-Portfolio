using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public int questID; // 퀘스트의 고유 아이디

    [Header("퀘스트 정보")]
    public string questName; // 퀘스트 이름
    [TextArea]
    public string questDescription; // 퀘스트 설명

    [Header("퀘스트 제공 NPC")]
    public string StartNpcName;
    public Sprite StartNpcImage;
    public Vector3 StartNpcTransform;

    [Header("퀘스트 마감 NPC")]
    public string EndNpcName;
    public Sprite EndNpcImage;
    public Vector3 EndNpcTransform;

    [Header("클리어 보상")]
    public BaseitemDB ClearItem;
    public long ClearGold;
    public long ClearExp;

    public List<QuestRequirement> requirements = new(); // 퀘스트 요구사항
}

//퀘스트 진행 상태
public enum QuestState
{
    Available,
    InProgress,
    Completed
}

//------------------------------------퀘스트 요구 관련 클래스-------------------------------------
public enum QuestRequirementType
{
    Kill,
    CollectItem,
    TalkNpc
}

public enum QuestTargetType
{
    Monster,
    Npc,
    Item
}

[Serializable]
public class  QuestRequirement
{
    public QuestRequirementType requireType;

    public QuestTargetType targetType;
    public int targetID; // 타겟 id
    public int requiredCount; // 요구 갯수
    public string questText; // 퀘스트 내용 -> ex 슬라임처치 (5 / 10) 에서 '슬라임 처치'담당
}
//---------------------------------------------------------------------------------------------


//--------------------------------퀘스트 요구사항에 대한 진행도 관련 클래스--------------------------------
[Serializable]
public class QuestRequirementProgress
{
    public int requireCount; // 요구 갯수
    public int currentCount; // 현재 갯수
    public bool isCompleted; // 퀘스트 완료 가능 확인
}

public readonly struct QuestRequirementRef
{
    public int questID { get; } // 해당 퀘스트 id에
    public int requirementIndex { get; } // 요구사항 index칸에 있는것을 바꿔야 됩니다.

    public QuestRequirementRef(int _questID, int _requirementIndex)
    {
        this.questID = _questID;
        this.requirementIndex = _requirementIndex;
    }
}
public enum QuestProgressUpdateMode
{
    Add,
    Set // 아이템을 땅에 버려서 0개 이렇게 되면 한방에 처리하기 위함
}

[Serializable]
public class QuestProgressData
{
    public int questID; // 어떤 퀘스트? -> 있어야 되나? 고민중

    public QuestState questState; // 진행 상태

    //public int currentCount; // 진행상황 -> QuestRequirment로 대체예정
    public List<QuestRequirementProgress> requirementProgresses = new();

    public QuestProgressData(QuestData _questData)
    {
        questID = _questData.questID;
        questState = QuestState.Available;

        foreach(var require in _questData.requirements)
        {
            requirementProgresses.Add(new QuestRequirementProgress() { requireCount = require.requiredCount, currentCount = 0, isCompleted = false });
        }
    }

}
