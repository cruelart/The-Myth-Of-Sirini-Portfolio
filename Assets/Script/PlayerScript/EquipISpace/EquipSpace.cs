using UnityEngine;
using System;

public class EquipSpace : MonoBehaviour
{
    public event Action<EquipmentItemInstance> OnChangeEquipSpaceUI;

    public event Action<WeaponStyle> OnMainWeaponChanged;
    public event Action<WeaponStyle> OnSecondaryWeaponChanged;

    //무기
    private EquipmentItemInstance main_weapon;
    public EquipmentItemInstance CurrentMainWeapon => main_weapon;

    //보조무기
    private EquipmentItemInstance secondary_weapon;
    public EquipmentItemInstance CurrentSecondaryWeapon => secondary_weapon;

    //방패
    private EquipmentItemInstance hat;

    //장갑
    private EquipmentItemInstance glove;

    //신발
    private EquipmentItemInstance shoes;

    //견장
    private EquipmentItemInstance armor;

    //귀걸이
    private EquipmentItemInstance shield;

    private PlayerStatManager playerStatManager;

    public void Init(PlayerStatManager _playerStatManager)
    {
        playerStatManager = _playerStatManager;
    }

    public void Setitem(EquipmentItemInstance _newItem, 
        ref EquipmentItemInstance _usingItem)
    {
        //장착중인 장비가 있었다면
        if(_usingItem != null)
        {
            if(!InventoryManager.Instance.AddItemInInventory(_usingItem, 1)) // -> 어처피 여기서 공지 띄울거임
            {
                //GameEventChannel.OnNotify?.Invoke("장비 인벤토리가 꽉 찼습니다. 한칸이상 비워주세요");
                return;
            }

            MinusStatItem(_usingItem);
        }

        _usingItem = _newItem;
        OnChangeEquipSpaceUI?.Invoke(_usingItem);
        GameEventChannel.OnNotify?.Invoke(_usingItem.setting.itemName + "을 장착하였습니다");

        //무기에 담긴 모든 스텟을 더해주기
        PlusStatItem(_newItem);
    }

    void Update()
    {
    }

    public void EquipItem(EquipmentItemInstance _item)
    {
        EquipmentItemDB.EquipmentType item_type = _item.setting.type;
        Debug.Log("EquipSpace" + item_type);

        switch (item_type)
        {
            //장착부위 무기
            case EquipmentItemDB.EquipmentType.Weapon:

                EquipmentItemInstance previousWeapon = main_weapon;

                //Debug.Log("무기를 장착하셨습니다");
                Setitem(_item, ref main_weapon);

                if (main_weapon != previousWeapon)
                {
                    OnMainWeaponChanged?.Invoke(main_weapon.setting.weaponStyle); // 무기 교체 이벤트 발생
                }

                break;

            case EquipmentItemDB.EquipmentType.SecondaryWeapon:

                EquipmentItemInstance previousSecondaryWeapon = secondary_weapon;

                Setitem(_item, ref secondary_weapon);

                if (secondary_weapon != previousSecondaryWeapon)
                {
                    OnSecondaryWeaponChanged?.Invoke(secondary_weapon.setting.weaponStyle);
                }

                break;

            //방패
            case EquipmentItemDB.EquipmentType.Hat:

                Setitem(_item, ref hat);

                break;
            
            //장갑
            case EquipmentItemDB.EquipmentType.Glove:

                Setitem(_item, ref glove);

                break;

            //신발
            case EquipmentItemDB.EquipmentType.Boots:

                Setitem(_item, ref shoes);

                break;

            //귀걸이
            case EquipmentItemDB.EquipmentType.Shield:

                Setitem(_item, ref shield);

                break;

            //갑옷
            case EquipmentItemDB.EquipmentType.Armor:

                Setitem(_item, ref armor);

                break;
        }
    }

    public void TakeOffItem(EquipmentItemInstance _item)
    {
        EquipmentItemDB.EquipmentType item_type = _item.setting.type;
        Debug.Log("EquipSpace" + item_type);

        switch (item_type)
        {
            //장착부위 무기
            case EquipmentItemDB.EquipmentType.Weapon:
                main_weapon = null;
                OnMainWeaponChanged?.Invoke(WeaponStyle.None);
                break;

            //보조무기해제
            case EquipmentItemDB.EquipmentType.SecondaryWeapon:
                secondary_weapon = null;
                OnSecondaryWeaponChanged?.Invoke(WeaponStyle.None);
                break;

            //방패
            case EquipmentItemDB.EquipmentType.Hat:

                hat = null;

                break;

            //장갑
            case EquipmentItemDB.EquipmentType.Glove:

                glove = null;

                break;

            //신발
            case EquipmentItemDB.EquipmentType.Boots:

                shoes = null;

                break;

            //귀걸이
            case EquipmentItemDB.EquipmentType.Shield:

                shield = null;

                break;

            //갑옷
            case EquipmentItemDB.EquipmentType.Armor:

                armor = null;

                break;
        }

        MinusStatItem(_item);
    }

    //스텟증가
    public void PlusStatItem(EquipmentItemInstance _item)
    {
        //무기에 담긴 모든 스텟을 더해주기
        foreach (var stats in _item.setting.stats)
        {
            playerStatManager.PlusStat(stats.type, (int)stats.value);
        }
    }

    //스텟감소
    public void MinusStatItem(EquipmentItemInstance _item)
    {
        //무기에 담긴 모든 스텟을 더해주기
        foreach (var stats in _item.setting.stats)
        {
            playerStatManager.MinusStat(stats.type, (int)stats.value);
        }
    }

}
