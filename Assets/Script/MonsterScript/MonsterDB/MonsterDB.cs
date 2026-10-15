using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDB", menuName = "Scriptable Objects/MonsterDB")]
public class MonsterDB : ScriptableObject
{
    public int monsterID;

    public MonsterType monsterType;

    public Sprite monsterImage;

    public List<Stat> stats;

    public int exp_value;
    public int gold_value;

    public float viewAngle;
    public float detectionRange;
    public float attackRange;

    [Header("드랍테이블")]
    public List<PublicDropTable> publicdropTables = new List<PublicDropTable>(); // 공용 드랍
    public List<DropTableEntry> personalDropItems = new List<DropTableEntry>(); // 개인 드랍
}

public enum MonsterType
{
    Slime,
    Golem,
}
