using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipInventorySlot
{
    public EquipmentItemInstance item;

    public bool IsEmpty()
    {
        return item == null;
    }
}
public class ConsumerInventorySlot
{
    public ConsumerItemInstance item;

    public bool IsEmpty()
    {
        return item == null;
    }
}
public class Re_Inventory : MonoBehaviour
{
    //아이템이 추가됐음을 알리는 이벤트 -> UI변경용
    public event Action<int, bool> OnChangeConsumerInventory; // 슬롯 번호, 교체할 것인지 여부
    public event Action<int> OnChangeEquipInventory;
    //public event Action OnChangeEtcInventory;

    //장비 아이템
    public List<EquipInventorySlot> equipmentItemSlotList = new List<EquipInventorySlot>(); // 인벤토리 나열용
    private int equip_ListMaxNum = 40;
    public int current_equipNum { get; private set; }
    public bool isEquipFull { get; private set; }

    //소비 아이템
    public List<ConsumerInventorySlot> consumerItemSlotList = new List<ConsumerInventorySlot>(); // 인벤토리 나열용
                                                                                                 //public Dictionary<long, int> consumerItemMap = new Dictionary<long, int>(); // 아이템 고유id가 인벤토리내에 총 몇개가 있는지 확인하는 용도(퀘스트 체크용도 등등)

    public Dictionary<int, int> ItemCountMap = new Dictionary<int, int>(); // 인벤토리내 해당 아이템id를 가진 아이템이 몇개있는지 확인하는 용도 -> 퀘스트에 쓰일 예정

    private int consumer_ListMaxNum = 40;
    public int current_consumerNum { get; private set; }
    public bool isConsumerFull { get; private set; }

    //소비아이템 합치는 용도 -> ex) (하얀포션 키값, 하얀포션들의 집합체) -> 여기서 하얀포션이 99개가 아닌것을 찾고 전부 99개다? 그럼 새롭게 추가하는 형식
    public Dictionary<long, List<int>> consumerItemSearchItemID = new();

    //기타아이템


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AddEquipmentItem(ItemDataManager.Instance.CreateEquipmentItemInstance(10001)); // 테스트용 나무검 생성
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("구매 금액:" + equipmentItemList[0].settings.buy_gold);
        //Debug.Log("판매 금액:" + equipmentItemList[0].settings.sell_gold);
    }

    public void Init()
    {
        for (int i = 0; i < equip_ListMaxNum; i++)
        {
            equipmentItemSlotList.Add(new EquipInventorySlot());
        }

        for (int i = 0; i < consumer_ListMaxNum; i++)
        {
            consumerItemSlotList.Add(new ConsumerInventorySlot());
        }
    }
    private void UpdateItemCount(int _itemID, int _amount)
    {
        if (!ItemCountMap.TryGetValue(_itemID, out var count))
        {
            ItemCountMap.Add(_itemID, _amount);
        }
        else
        {
            ItemCountMap[_itemID] += _amount;
        }

        if (ItemCountMap[_itemID] <= 0)
        {
            ItemCountMap.Remove(_itemID);
        }
    }

    //장비아이템 인벤토리 추가 함수
    public bool AddEquipmentItem(int _itemID, int _amount)
    {
        int addedAmount = 0;

        string notify_str = "";

        //장비창이 꽉찼을 경우
        if (isFullEquipItem(_amount))
        {
            notify_str = $"아이템이 가득찼습니다";
            GameEventChannel.OnNotify?.Invoke(notify_str);
            return false;
        }

        Debug.Log("여긴 아직 버그가 아님");

        EquipmentItemDB new_equipmentItemDB = ItemDataManager.Instance.GetEquipmentItemDB(_itemID);

        notify_str = $"{new_equipmentItemDB.itemName}을(를) {_amount}개 획득하였습니다.";

        while (_amount > 0)
        {
            int emptySlotIndex = FindEmptyEquipSlot();

            if(emptySlotIndex == -1)
            {
                notify_str = $"인벤토리가 비어있지 않아 {new_equipmentItemDB.itemName}을(를) {_amount}개를 획득하지 못하였습니다.";
                break; // 빈자리없으니 더 이상 획득은 불가능
            }
            EquipmentItemInstance newEquipmentItem = ItemDataManager.Instance.CreateEquipmentItemInstance(_itemID);
            equipmentItemSlotList[emptySlotIndex].item = newEquipmentItem;
            current_equipNum++;

            OnChangeEquipInventory?.Invoke(emptySlotIndex);
            _amount--;
            addedAmount++;
        }
        UpdateItemCount(_itemID, addedAmount);

        GameEventChannel.OnNotify?.Invoke(notify_str);
        //equipmentItemList.Add(_item);
        //equipmentItemMap.Add(_item.instanceID, _item);

        return addedAmount > 0;

    }

    public bool AddEquipmentItem(EquipmentItemInstance _item, int _amount)
    {
        int addedAmount = 0;

        string notify_str = "";

        //장비창이 꽉찼을 경우
        if (isFullEquipItem(_amount))
        {
            notify_str = $"장비 인벤토리를 1칸이상 비워주세요";
            GameEventChannel.OnNotify?.Invoke(notify_str);
            return false;
        }

        //Debug.Log("여긴 아직 버그가 아님");

        //EquipmentItemDB new_equipmentItemDB = ItemDataManager.Instance.GetEquipmentItemDB(_itemID);

        //notify_str = $"{new_equipmentItemDB.itemName}을(를) {_amount}개 획득하였습니다.";

        while (_amount > 0)
        {
            int emptySlotIndex = FindEmptyEquipSlot();
            Debug.Log("emptySlotIndex" + emptySlotIndex);

            if (emptySlotIndex == -1)
            {
                notify_str = $"장비 인벤토리를 1칸이상 비워주세요.";
                GameEventChannel.OnNotify?.Invoke(notify_str);
                return false; // 빈자리없으니 더 이상 획득은 불가능
            }

            equipmentItemSlotList[emptySlotIndex].item = _item;
            current_equipNum++;

            OnChangeEquipInventory?.Invoke(emptySlotIndex);
            addedAmount++;
            _amount--;
        }

        UpdateItemCount(_item.setting.itemID, addedAmount);
        return addedAmount > 0;
        //equipmentItemList.Add(_item);
        //equipmentItemMap.Add(_item.instanceID, _item);

    }

    //소비아이템 인벤토리 추가 함수
    public bool AddConsumerItem(int _itemID, int _amount)
    {
        int addedAmount = 0;

        string notify_str = "";
        
        //걸러내기 -> 그냥 인벤토리가 꽉찬 상태면 무시
        if (isFullConsumerItem(_itemID, _amount))
        {
            notify_str = $"아이템이 가득찼습니다, 인벤토리를 비워주세요";
            GameEventChannel.OnNotify?.Invoke(notify_str);
            return false;
        }

        ConsumerItemDB new_consumerItemDB = ItemDataManager.Instance.GetConsumerItemDB(_itemID);

        //consumerItemList.Add(_item);
        //consumerItemMap.Add(_item.instanceID, _item);

        //알림판에 알릴 문자열 미리 받아놓기
        notify_str = $"{new_consumerItemDB.itemName}을(를) {_amount}개 획득하였습니다.";

        //기존 소비아이템 채우기 -> 하얀포션 30개, 50개, 98개, 99개, 97개인데 addconsumeritem(?, 1000)을 하면 99개, 99개, 99개, 99개, 99개 ... 이렇게 채우기용
        if (consumerItemSearchItemID.TryGetValue(_itemID, out var Slotlist))
        {
            foreach(var itemSlotIndex in Slotlist)
            {
                ConsumerItemInstance item = consumerItemSlotList[itemSlotIndex].item;

                if(item.count >= new_consumerItemDB.maxNum) // 해당 아이템이 이미 최대 수치라면 패스
                {
                    continue;
                }

                int maxAddNum = new_consumerItemDB.maxNum - item.count; // 최대 증가가능 갯수

                int realAddNum = Mathf.Min(maxAddNum, _amount); //실제 증가시킬 갯수

                item.AddCount(realAddNum);

                OnChangeConsumerInventory?.Invoke(itemSlotIndex, false);

                _amount -= realAddNum;
                addedAmount += realAddNum;

                if (_amount <= 0) // 다 못채웠는데 amount가 딸리면 그대로 종료
                {
                    GameEventChannel.OnNotify?.Invoke(notify_str);

                    UpdateItemCount(_itemID, addedAmount);
                    return true; // -> 몇개 채우긴했으니까 add 성공으로 침
                }
            }
        }
        
        //채울거 다 채웠으면
        while (_amount > 0) // 설정한 값 전부 생성할때까지 무한 생성
        {
            int slotIndex = FindEmptyConsumerSlot();

            if(slotIndex == -1)
            {
                break;
            }

            int count = Mathf.Min(_amount, new_consumerItemDB.maxNum); // 생성시도 갯수 vs 최대 갯수

            ConsumerItemInstance newConsumerItem = ItemDataManager.Instance.CreateConsumerItemInstance(_itemID); // 아이템 생성

            //Debug.Log("아이템생성중입니다 생성 아이템이름: ");
            newConsumerItem.AddCount(count);

            //consumerItemList.Add(newConsumerItem);
            //consumerItemMap.Add(newConsumerItem.setting.item, newConsumerItem);

            consumerItemSlotList[slotIndex].item = newConsumerItem;
            current_consumerNum++;

            RegisterConsumerItem(_itemID, slotIndex);
            OnChangeConsumerInventory?.Invoke(slotIndex, true);
            _amount -= count;
            addedAmount += count;
        }

        UpdateItemCount(_itemID, addedAmount);
        GameEventChannel.OnNotify?.Invoke(notify_str);

        return true;
    }

    //아이템 추가시 등록 함수
    private void RegisterConsumerItem(int _itemID, int _slotIndex)
    {
        //consumerItemSlotList.Add(_item);

        //int itemID = _item.setting.itemID; // 고유아이디 받아오자

        if(!consumerItemSearchItemID.TryGetValue(_itemID, out var list)) // 인벤토리에 포션저장리스트같은게 하나도 없엇다?
        {
            list = new List<int>(); // 그럼 만들어주자

            consumerItemSearchItemID.Add(_itemID, list);
        }

        list.Add(_slotIndex);
    }

    public bool EquipItem(int _slotIndex)
    {
        return RemoveEquipmentItem(_slotIndex);
    }

    //인벤토리에서 장비아이템 제거
    //1. 슬롯자체 번호전달받아서 제거 -> 장비창으로 옮기는 용도
    public bool RemoveEquipmentItem(int _slotIndex)
    {
        if(current_equipNum == 0)
        {
            return false;
        }
        UpdateItemCount(equipmentItemSlotList[_slotIndex].item.setting.itemID, -1);

        equipmentItemSlotList[_slotIndex].item = null;
        current_equipNum--;
        OnChangeEquipInventory?.Invoke(_slotIndex);
        return true;
    }

    private int FindEmptyEquipSlot()
    {
        for(int i = 0; i < equip_ListMaxNum; i++)
        {
            if (equipmentItemSlotList[i].IsEmpty())
            {
                return i; //빈 자리 발견 return
            }
        }

        return -1;
    }

    private int FindEmptyConsumerSlot()
    {
        for (int i = 0; i < consumer_ListMaxNum; i++)
        {
            if (consumerItemSlotList[i].IsEmpty())
            {
                return i; //빈 자리 발견 return
            }
        }

        return -1;
    }

    public bool isFullConsumerItem(int _itemID, int _amount)
    {
        ConsumerItemDB new_consumerItemDB = ItemDataManager.Instance.GetConsumerItemDB(_itemID);

        int needSlot = Mathf.CeilToInt((float)_amount / new_consumerItemDB.maxNum);

        if (current_consumerNum + needSlot > consumer_ListMaxNum)
        {
            return true;
        }

        return current_consumerNum > consumer_ListMaxNum;
    }

    public bool isFullEquipItem(int _amount)
    {
        return current_equipNum + _amount > equip_ListMaxNum;
    }

    public int GetItemCount(int _itemID)
    {
        if (ItemCountMap.TryGetValue(_itemID, out var count))
        {
            return count;
        }
        return 0;
    }

    //장비아이템 정렬
    //public void SortEquipmentItem()
    //{
    //    equipmentItemList.Sort((a, b) => a.setting.itemID.CompareTo(b.setting.itemID)); // 아이템 고유 ID 순서대로 정렬 
    //}

    ////소비아이템 정렬
    //public void SortConsumerItem()
    //{
    //    consumerItemList.Sort((a, b) => a.setting.itemID.CompareTo(b.setting.itemID)); // 아이템 고유 ID 순서대로 정렬 
    //}
}
