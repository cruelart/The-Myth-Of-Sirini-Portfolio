using System;
using UnityEngine;

public class MonsterDeadInfo
{
    public int monsterID;
    public GameObject monsterObj;
    public MonsterSpawnZone spawnZone;

    public PlayerStatManager killerPlayer;

    public MonsterDeadInfo(int _monsterID, GameObject _monsterObj, MonsterSpawnZone _monsterSpawnZone, PlayerStatManager _killerPlayer)
    {
        monsterID = _monsterID;
        spawnZone = _monsterSpawnZone;
        monsterObj = _monsterObj;

        killerPlayer = _killerPlayer;
    }
}
public static class MonsterEvent
{
    public static event Action<MonsterDeadInfo> OnMonsterDead; // 죽은 몬스터의 키 int 값을 받기 ㄱ

    public static void RaiseMonsterDead(MonsterDeadInfo info)
    {
        OnMonsterDead?.Invoke(info);
    }
}
