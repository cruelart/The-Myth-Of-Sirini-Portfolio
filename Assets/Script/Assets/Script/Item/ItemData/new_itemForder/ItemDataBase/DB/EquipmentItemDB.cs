using UnityEngine;

public enum WeaponStyle
{
    None = 0,
    Unarmed = 1,
    Sword = 2,
    Gun = 3
}

[CreateAssetMenu(fileName = "EquipmentItemDB", menuName = "Scriptable Objects/Item/Equipment")]
public class EquipmentItemDB : BaseitemDB
{
    public enum EquipmentType { Weapon, Armor, Glove, Boots, Hat, Shield, SecondaryWeapon } // 장비부위 종류

    [Header("Equipment Specific")]
    public EquipmentType type; // 장비 부위

    [Header("Weapon Specific")]
    public WeaponStyle weaponStyle;
}
