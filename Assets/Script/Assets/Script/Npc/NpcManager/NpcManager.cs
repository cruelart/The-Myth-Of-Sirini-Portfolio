using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager Instance { get; private set; }

    private Dictionary<string, NPC> npcDataTable = new(); // NPC의 이름, NPC의 정보들

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddNpc(string _npcName, NPC _npc)
    {
        npcDataTable[_npcName] = _npc;
    }

    public NPC GetNPC(string _npcName)
    {
        npcDataTable.TryGetValue(_npcName, out NPC npc);
        return npc;
    }

    public void ClearNpc()
    {
        npcDataTable.Clear(); // 새로운 씬으로 가면 모든 npc 다시 받아오기위해 Clear처리하기
    }
}
