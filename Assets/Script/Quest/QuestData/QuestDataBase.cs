using System.Collections.Generic;
using UnityEngine;

public class QuestDataBase
{
    private Dictionary<int, QuestData> questDataTable = new();

    public IReadOnlyDictionary<int, QuestData> QuestDataTable => questDataTable;

    public void LoadData()
    {
        QuestData[] datas = Resources.LoadAll<QuestData>("Quest");

        foreach(var data in datas)
        {
            questDataTable[data.questID] = data;
        }
    }

    public QuestData GetQuestData(int questID)
    {
        if(questDataTable.TryGetValue(questID, out QuestData questData))
        {
            return questData;
        }
        else
        {
            Debug.LogError($"QuestData with ID {questID} not found.");
            return null;
        }
    }
}
