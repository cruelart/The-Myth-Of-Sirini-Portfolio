using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance { get; private set; }
    //플레이어의 고정 데이터
    public PlayerStatData playerStatDB { get; private set; }

    //플레이어의 유동 데이터
    //public int currentHp { get; private set; }
    //public int currentMp { get; private set; }

    public long maxExp { get; private set; }
    //public long currentExp { get; private set; }

    public int final_attack { get; private set; }

    //public int stat_upgradePossibleValue { get; private set; } // 스탯 업그레이트 가능 횟수

    //이벤트
    public event Action<int> OnLevelUp;
    public event Action<int, int> OnHpChanged;
    public event Action<long, long> OnExpChanged;
    public event Action<float> OnMoveSpeedChanged;
    public event Action OnChangeStat;

    private List<Stat> final_statList; // 최종 스탯 (연산 다 끝낸거)
    private Dictionary<StatType, Stat> final_statDict; // 편의를 위한 해시작성

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Init(PlayerStatData _data)
    {
        playerStatDB = _data;

        //깊은 복사시키기
        final_statList = playerStatDB.stats.Select(s => new Stat { type = s.type, value = s.value }).ToList();
        final_statDict = final_statList.ToDictionary(s => s.type);

        playerStatDB.stat_upgradePossibleValue = 5;

        RefreshStats();

        playerStatDB.currentHp = (int)final_statDict[StatType.HP].value;
        playerStatDB.currentMp = (int)final_statDict[StatType.MP].value;

        maxExp = playerStatDB.level * 100;
    }

    public string GetPlayerName()
    {
        return playerStatDB.playerName;
    }
    public int GetPlayerLv()
    {
        return playerStatDB.level;
    }

    public Stat GetStat(StatType _type)
    {
        final_statDict.TryGetValue(_type, out var stat);
        return stat;
    }

    public float GetStatValue(StatType _type)
    {
        final_statDict.TryGetValue(_type, out var stat);
        return stat.value;
    }

    public PlayerStatData GetPlayerSaveStatData()
    {
        return playerStatDB;
    }


    public void OnDamaged(int _damage)
    {
        playerStatDB.currentHp -= _damage;
        OnHpChanged?.Invoke(playerStatDB.currentHp, 
            (int)GetStatValue(StatType.HP)); // hp 변동사항 알림

        Debug.Log("PlayerStatManager에 있는 OnDamage함수 호출");

        //if(playerHp <= 0)
        //{
        //    Die();
        //}
    }

    public void GetExp(long _exp)
    {
        playerStatDB.currentExp += _exp;
        //경험치가 너무많이 오르면
        if (maxExp < playerStatDB.currentExp)
        {
            playerStatDB.currentExp -= maxExp; // 레벨업했으니 경험치 다시 초기화
            LevelUp();
        }

        OnExpChanged?.Invoke(playerStatDB.currentExp, maxExp);
    }

    public void LevelUp()
    {
        playerStatDB.level += 1; // 일단 레벨업하고

        OnLevelUp?.Invoke(playerStatDB.level); // 이벤트호출 (확성기 날림)

        //대충 레벨업하면 일어나는 일들
        PlusStat(StatType.STR,1); // 테스트용 힘 상승
        playerStatDB.stat_upgradePossibleValue += 5;

        RefreshStats();

        playerStatDB.currentHp = (int)final_statDict[StatType.HP].value; // 풀피로 변경
        playerStatDB.currentMp = (int)final_statDict[StatType.MP].value;

        OnHpChanged?.Invoke(playerStatDB.currentHp, (int)final_statDict[StatType.MP].value);
    }

    public void UpgradeStatusStat(StatType _statType, int _value)
    {
        if(playerStatDB.stat_upgradePossibleValue <= 0)
        {
            return;
        }
        playerStatDB.stat_upgradePossibleValue--;
        playerStatDB.statDict[_statType].value += _value;
        RefreshStats();
    }

    public void PlusStat(StatType _statType, int _value)
    {
        playerStatDB.statDict[_statType].value += _value;
        RefreshStats();
    }

    public void MinusStat(StatType _statType, int _value)
    {
        playerStatDB.statDict[_statType].value -= _value;
        RefreshStats();
    }

    public void RefreshStats()
    {
        CalculateStat();
        //대충 스텟이 변했다는 걸 알려줄 수도 있는 것들을 적는 자리
        OnMoveSpeedChanged?.Invoke(final_statDict[StatType.MoveSpeed].value);
        OnChangeStat?.Invoke();
    }

    public void CalculateStat()
    {
        final_statDict[StatType.Attack].value = (int)(playerStatDB.statDict[StatType.Attack].value + playerStatDB.statDict[StatType.STR].value * 2 + playerStatDB.statDict[StatType.DEX].value);
        final_statDict[StatType.Defense].value = (int)(playerStatDB.statDict[StatType.Defense].value + playerStatDB.statDict[StatType.DEX].value * 2 + playerStatDB.statDict[StatType.STR].value);
        final_statDict[StatType.Mental].value = (int)(playerStatDB.statDict[StatType.Mental].value + playerStatDB.statDict[StatType.INT].value * 2 + playerStatDB.statDict[StatType.LUK].value);
        final_statDict[StatType.Accuracy].value = (int)(playerStatDB.statDict[StatType.Accuracy].value + playerStatDB.statDict[StatType.DEX].value * 1);
        final_statDict[StatType.CriticalPercent].value = (int)(playerStatDB.statDict[StatType.CriticalPercent].value + playerStatDB.statDict[StatType.LUK].value * 1);
        final_statDict[StatType.MoveSpeed].value = (playerStatDB.statDict[StatType.MoveSpeed].value + playerStatDB.statDict[StatType.DEX].value * 0.2f);
        final_statDict[StatType.Avoidance].value = (int)(playerStatDB.statDict[StatType.Avoidance].value + playerStatDB.statDict[StatType.LUK].value * 1);
        final_statDict[StatType.HP].value = (int)(playerStatDB.statDict[StatType.HP].value + playerStatDB.statDict[StatType.STR].value * 30);
        final_statDict[StatType.MP].value = (int)(playerStatDB.statDict[StatType.MP].value + playerStatDB.statDict[StatType.INT].value * 30);


        final_statDict[StatType.STR].value = (int)(playerStatDB.statDict[StatType.STR].value);
        final_statDict[StatType.DEX].value = (int)(playerStatDB.statDict[StatType.DEX].value);
        final_statDict[StatType.INT].value = (int)(playerStatDB.statDict[StatType.INT].value);
        final_statDict[StatType.LUK].value = (int)(playerStatDB.statDict[StatType.LUK].value);

        maxExp = playerStatDB.level * 100;
    }
}
