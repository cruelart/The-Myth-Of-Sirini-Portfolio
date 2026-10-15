using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSpaceUIManager : UIBase
{ 
    [Header("장비 이미지 세팅")]
    [SerializeField]
    private EquipSpaceSlot weapon_slot;

    [SerializeField]
    private EquipSpaceSlot secondary_weapon_slot;

    [SerializeField]
    private EquipSpaceSlot hat_slot;

    [SerializeField]
    private EquipSpaceSlot shoes_slot;

    [SerializeField]
    private EquipSpaceSlot glove_slot;

    [SerializeField]
    private EquipSpaceSlot shield_slot;

    [SerializeField]
    private EquipSpaceSlot armor_slot;

    private EquipSpace equipSpace;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(EquipSpace _equipSpace)
    {
        equipSpace = _equipSpace;

        equipSpace.OnChangeEquipSpaceUI += EquipItem;

        weapon_slot.OnInventoryRequest += TakeOffItem;
        hat_slot.OnInventoryRequest += TakeOffItem;
        shoes_slot.OnInventoryRequest += TakeOffItem;
        glove_slot.OnInventoryRequest += TakeOffItem;
        shield_slot.OnInventoryRequest += TakeOffItem;
        armor_slot.OnInventoryRequest += TakeOffItem;
        secondary_weapon_slot.OnInventoryRequest += TakeOffItem;
    }

    public void EquipItem(EquipmentItemInstance _item)
    {
        EquipmentItemDB.EquipmentType equipType = _item.setting.type;

        //타입에 따라 아이템넣기
        switch (equipType)
        {
            //무기
            case EquipmentItemDB.EquipmentType.Weapon:
                weapon_slot.SetItem(_item);
                break;

            case EquipmentItemDB.EquipmentType.SecondaryWeapon:
                secondary_weapon_slot.SetItem(_item);
                break;

            //방패
            case EquipmentItemDB.EquipmentType.Hat:
                hat_slot.SetItem(_item);
                break;

            //장갑
            case EquipmentItemDB.EquipmentType.Glove:
                glove_slot.SetItem(_item);
                break;

            case EquipmentItemDB.EquipmentType.Shield:
                shield_slot.SetItem(_item);
                break;

            case EquipmentItemDB.EquipmentType.Armor:
                armor_slot.SetItem(_item);
                break;

            case EquipmentItemDB.EquipmentType.Boots:
                shoes_slot.SetItem(_item);
                break;
        }
    }

    public void TakeOffItem(EquipmentItemInstance _item)
    {
        EquipmentItemDB.EquipmentType equipType = _item.setting.type;

        if (InventoryManager.Instance.AddItemInInventory(_item, 1))
        {
            switch (equipType)
            {
                //무기
                case EquipmentItemDB.EquipmentType.Weapon:
                    weapon_slot.ClearSlot();
                    EquipSpaceManager.Instance.TakeOffItem(_item);
                    break;

                case EquipmentItemDB.EquipmentType.SecondaryWeapon:
                    secondary_weapon_slot.ClearSlot();
                    EquipSpaceManager.Instance.TakeOffItem(_item);
                    break;

                //모자
                case EquipmentItemDB.EquipmentType.Hat:
                    hat_slot.ClearSlot();
                    EquipSpaceManager.Instance.TakeOffItem(_item);
                    break;

                //장갑
                case EquipmentItemDB.EquipmentType.Glove:
                    glove_slot.ClearSlot();
                    EquipSpaceManager.Instance.TakeOffItem(_item);
                    break;

                case EquipmentItemDB.EquipmentType.Shield:
                    shield_slot.ClearSlot();
                    EquipSpaceManager.Instance.TakeOffItem(_item);
                    break;

                case EquipmentItemDB.EquipmentType.Armor:
                    armor_slot.ClearSlot();
                    EquipSpaceManager.Instance.TakeOffItem(_item);
                    break;

                case EquipmentItemDB.EquipmentType.Boots:
                    shoes_slot.ClearSlot();
                    EquipSpaceManager.Instance.TakeOffItem(_item);
                    break;
            }
        }
    }

}
