using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class InventoryUIManager : UIBase
{
    [Header("상위 부모 설정")]
    [SerializeField]
    private Transform equip_content;

    [SerializeField]
    private Transform consumer_content;

    [SerializeField]
    private Transform etx_content;

    [Header("인벤토리 장비 슬롯 프리팹 투입")]
    [SerializeField]
    private EquipSlotUI equip_slotPrefab;

    [Header("인벤토리 소비 슬롯 프리팹 투입")]
    [SerializeField]
    private ConsumerSlotUI consumer_slotPrefab;

    private Re_Inventory inventory;

    private List<EquipSlotUI> equip_slotList = new List<EquipSlotUI>();
    private List<ConsumerSlotUI> consumer_slotList = new List<ConsumerSlotUI>();


    // Update is called once per frame
    void Update()
    {

        //ShowInventory();
    }

    public void Init(Re_Inventory _inventory)
    {
        inventory = _inventory;

        inventory.OnChangeConsumerInventory += AddConsumer_In_Inventory;
        inventory.OnChangeEquipInventory += AddEquip_In_Inventory;

        //ShowInventory();

        int equipIndex = 0;
        int consumerIndex = 0;

        foreach (var itemSlot in inventory.equipmentItemSlotList)
        {
            EquipSlotUI slot = Instantiate(equip_slotPrefab, equip_content);

            slot.OnEquipRequest += InventoryManager.Instance.EquipItem;

            //slot.SetItem(itemSlot.item);
            slot.Init(equipIndex++);

            equip_slotList.Add(slot);
        }

        foreach (var itemSlot in inventory.consumerItemSlotList)
        {
            ConsumerSlotUI slot = Instantiate(consumer_slotPrefab, consumer_content);

            slot.OnConsumerRequest += InventoryManager.Instance.EquipItem;

            //slot.SetItem(itemSlot.item);
            slot.Init(consumerIndex++);
            consumer_slotList.Add(slot);
        }
    }

    private void ClearEquipSlots()
    {
        foreach(var slot in equip_slotList) // 남아있는 ui찌꺼기들 싹다 제거
        {
            Destroy(slot.gameObject);
        }

        equip_slotList.Clear(); // 다 비워버리기
    }

    private void ClearConsumerSlots()
    {
        foreach (var slot in consumer_slotList) // 남아있는 ui찌꺼기들 싹다 제거
        {
            Destroy(slot.gameObject);
        }

        consumer_slotList.Clear(); // 다 비워버리기
    }

    public void AddEquip_In_Inventory(int _slotIndex)
    {
        EquipInventorySlot itemSlot = inventory.equipmentItemSlotList[_slotIndex];

        equip_slotList[_slotIndex].SetItem(itemSlot.item);
    }

    public void AddConsumer_In_Inventory(int _slotIndex, bool _isNeedSlot)
    {
        var itemSlot = inventory.consumerItemSlotList[_slotIndex];
        consumer_slotList[_slotIndex].SetItem(itemSlot.item);
    }

    //정렬용
    public void ShowSortedEquipInventory()
    {
        ClearEquipSlots();

        //Debug.Log("ShowInventory가 호출됐습니다");
        foreach (var itemSlot in inventory.equipmentItemSlotList)
        {
            EquipSlotUI slot = Instantiate(equip_slotPrefab, equip_content);

            slot.SetItem(itemSlot.item);

            equip_slotList.Add(slot);
        }
    }

    public void ShowSortedConsumerInventory()
    {
        ClearConsumerSlots();

        //int slotNum = 1;

        //Debug.Log("ShowInventory가 호출됐습니다");
        foreach (var itemSlot in inventory.consumerItemSlotList)
        {
            ConsumerSlotUI slot = Instantiate(consumer_slotPrefab, consumer_content);

            slot.SetItem(itemSlot.item);

            consumer_slotList.Add(slot);
        }
    }

    private void EquipItem(int _slotIndex)
    {
        //장착할 아이템
        EquipmentItemInstance item = inventory.equipmentItemSlotList[_slotIndex].item;

        if(item == null)
        {
            Debug.Log("InventoryUI에서 장착할 아이템을 찾는데 실패했습니다");
            return;
        }
    }

    private void UseConsumerItem(int _slotIndex)
    {
        //장착할 아이템
        ConsumerItemInstance item = inventory.consumerItemSlotList[_slotIndex].item;

        if (item == null)
        {
            Debug.Log("InventoryUI에서 장착할 아이템을 찾는데 실패했습니다");
            return;
        }
    }
}
