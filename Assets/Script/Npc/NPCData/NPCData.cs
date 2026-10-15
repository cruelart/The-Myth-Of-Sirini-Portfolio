using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NpcTextData
{
    public string npcName;

    [TextArea]
    public string npcText;

    public List<TalkButtonType> buttonTypes; // 해당 텍스트가 어떤 버튼들을 들고있을 것인지
}

public enum NPCType
{
    Normal,
    WanderingTraderShop
}

[Serializable]
public class NpcTalkData
{
    public int questID; // 해당 대화가 진행되는 퀘스트ID
    public List<NpcTextData> npcTexts = new(); // ncp 대화내용
}

[CreateAssetMenu(fileName = "NPCData", menuName = "Scriptable Objects/NPCData")]
public class NPCData : ScriptableObject
{
    public int npcID;
    public string npcName;

    //[Header("Npc가 제공할 퀘스트ID들")]
    //public List<int> npcQuestIDs = new(); // npc가 제공할 퀘스트ID들

    [Header("NPC의 정보")]
    public List<NpcTalkData> npcTalkDatas = new(); // npc 대화
}
