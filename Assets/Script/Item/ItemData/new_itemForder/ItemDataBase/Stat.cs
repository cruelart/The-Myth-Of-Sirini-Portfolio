using UnityEngine;
using System.Collections.Generic;

public enum StatType 
{
    Attack, // 공격력
    Defense, // 방어력
    MoveSpeed, // 이동속도
    RotationSpeed, // 회전 속도
    CriticalPercent, // 치명타확률
    HP, // 체력
    MP, // 마나
    Accuracy, // 명중률
    STR, // 힘
    DEX, // 민첩
    INT, // 지력
    LUK, // 행운
    Mental, // 정신력
    Avoidance // 회피율
}

[System.Serializable]
public class Stat
{
    public StatType type; // 스텟의 종류
    public float value; // 해당 스텟 값
}
