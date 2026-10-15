using System.Collections.Generic;
using UnityEngine;

//고정 데이터

[CreateAssetMenu(fileName = "New_itemDB", menuName = "Scriptable Objects/New_itemDB")]
public abstract class BaseitemDB : ScriptableObject
{
    [Header("item information")]
    public int itemID; // 아이템 고유 번호
    public string itemName; // 아이템 이름
    public Sprite itemIcon; // 아이템 아이콘 이미지

    [TextArea]
    public string description; // 아이템 설명란

    public enum ItemType { Equipment, Consumable, Etc } // 장비, 소비, 기타아이템 종류
    public ItemType itemType;

    [Header("Stat")]
    public List<Stat> stats = new List<Stat>(); // 스텟 관리

    [Header("Public character")]
    public int sell_gold; // 판매 가격
    public int buy_gold; // 구매 가격

    [Header("아이템 매쉬 정보")] 
    public Mesh dropMesh;
    public Material dropMaterial;

    [Header("아이템 Transform 정보")]
    public Vector3 dropScale = Vector3.one;
    public Vector3 dropRotation;
}
