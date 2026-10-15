using System;
using UnityEngine;

[Serializable]
public class DropTableEntry
{
    public int itemID; // 드랍할 아이템 아이디

    [Range(0f, 100f)]
    public float dropPercent; // 드랍 확률

    public int minCount = 1;
    public int maxCount = 1;
}
