using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

//[System.Serializable]
public class PlayerStatData
{

    //-----------------------------플레이어 기본 정보-------------------------------
    public string playerName; // 이름

    public int level;
    public long currentExp;

    public int currentHp;
    public int currentMp;

    public long gold;

    //플레이어 위치
    public string currentMap; // 플레이어가 현재 위치하고 있는 맵
    public float posX;
    public float posY;
    public float posZ;

    public List<Stat> stats;
    public Dictionary<StatType, Stat> statDict = new Dictionary<StatType, Stat>();

    public int stat_upgradePossibleValue;

    public PlayerStatData()
    {
        playerName = "New Player";
        level = 1;
        currentExp = 0;
        gold = 100;

        currentMap = "tutorialMap";
        posX = 0;
        posY = 1.0f;
        posZ = 0;

        stats = new List<Stat>();

        //초기 스텟
        InitStat();
        statDict = stats.ToDictionary(s => s.type);

        stat_upgradePossibleValue = 5;
    }

    private void AddStat(StatType _type, float _value)
    {
        stats.Add(new Stat()
        {
            type = _type,
            value = _value
        });
    }

    private void InitStat()
    {
        AddStat(StatType.Attack, 60);
        AddStat(StatType.Defense, 10);
        AddStat(StatType.MoveSpeed, 2);
        AddStat(StatType.Mental, 10);
        AddStat(StatType.CriticalPercent, 0);
        AddStat(StatType.Avoidance, 1);
        AddStat(StatType.HP, 100);
        AddStat(StatType.MP, 100);
        AddStat(StatType.Accuracy, 10);
        AddStat(StatType.STR, 5);
        AddStat(StatType.DEX, 5);
        AddStat(StatType.INT, 5);
        AddStat(StatType.LUK, 5);
    }

    public void SetAddStat(StatType _type, float value)
    {
        statDict[_type].value += value;
    }

    public void SetRemoveStat(StatType _type, float value)
    {
        statDict[_type].value -= value;
    }

}
