using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private Re_Inventory inventoryDB;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Re_Inventory _inventoryDB)
    {
        inventoryDB = _inventoryDB;
    }

    private BaseitemDB GetItemInformation(int _itemID)
    {
        return ItemDataManager.Instance.GetBaseitemDB(_itemID);
    }

    public bool isPossibleGetItem(int _itemID, int _amount)
    {
        BaseitemDB Item = GetItemInformation(_itemID); // 인벤토리에 집어넣을 아이템

        BaseitemDB.ItemType itemTypeName = Item.itemType;
        //Debug.Log("BaseitemDB.ItemType" + itemTypeName);

        //타입에 따라 아이템넣기
        switch (itemTypeName)
        {
            //장비
            case BaseitemDB.ItemType.Equipment:
                return !inventoryDB.isFullEquipItem(_amount);

            //소비
            case BaseitemDB.ItemType.Consumable:
                return !inventoryDB.isFullConsumerItem(_itemID, _amount);
            //기타
            case BaseitemDB.ItemType.Etc:
                break;
        }

        return true;
    }


    public void AddItemInInventory(int _itemID, int _amount)
    {
        BaseitemDB Item = GetItemInformation(_itemID); // 인벤토리에 집어넣을 아이템

        BaseitemDB.ItemType itemTypeName = Item.itemType;
        Debug.Log("BaseitemDB.ItemType" + itemTypeName);
        
        //타입에 따라 아이템넣기
        switch(itemTypeName)
        {
            //장비
            case BaseitemDB.ItemType.Equipment:
                if (inventoryDB.AddEquipmentItem(_itemID, _amount))
                    InventoryEvent.RaiseOwnedItemCountChanged(_itemID, GetItemCount(_itemID));
                break;

            //소비
            case BaseitemDB.ItemType.Consumable:
                Debug.Log("InventoryManager : MonoBehaviour : 소비아이템을 획득하였습니다");
                if(inventoryDB.AddConsumerItem(_itemID, _amount))
                    InventoryEvent.RaiseOwnedItemCountChanged(_itemID, GetItemCount(_itemID));
                break;

            //기타
            case BaseitemDB.ItemType.Etc:
                break;
        }
    }

    public bool AddItemInInventory(EquipmentItemInstance _equipItem, int _amount)
    {
        if (!inventoryDB.AddEquipmentItem(_equipItem, _amount))
        {
            return false;
        }

        InventoryEvent.RaiseOwnedItemCountChanged(_equipItem.setting.itemID, GetItemCount(_equipItem.setting.itemID));
        return true;
    }

    public void EquipItem(int _slotIndex)
    {
        EquipmentItemInstance item = 
            inventoryDB.equipmentItemSlotList[_slotIndex].item;

        if (!inventoryDB.EquipItem(_slotIndex))
            return;

        InventoryEvent.RaiseOwnedItemCountChanged(item.setting.itemID, 
            GetItemCount(item.setting.itemID));
        EquipSpaceManager.Instance.EquipItem(item);
    }

    public bool IsFullEquipSlot(int _amount)
    {
        return inventoryDB.isFullEquipItem(_amount);
    }

    public int GetItemCount(int _itemID)
    {
        return inventoryDB.GetItemCount(_itemID);
    }
}
