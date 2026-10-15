using System;
using UnityEngine;

public class MonsterDeadEventData
{
    public int monsterID;
    public Vector3 monserPos;
}

public static class GameEventChannel
{
    //public static Action<MonsterDeadEventData> OnMonsterDead;

    public static Action<string> OnNotify;
    public static Action<bool> OnLockCamera;
}
