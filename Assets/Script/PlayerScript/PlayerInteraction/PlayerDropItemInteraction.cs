using System.Collections.Generic;
using UnityEngine;

public class PlayerDropItemInteraction : MonoBehaviour
{
    Re_Inventory inventoryDB;

    //플레이어와 닿은 모든 드랍아이템들 저장공간
    HashSet<InstanceDropItem> dropItemsHash = new HashSet<InstanceDropItem>();

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            //추가할수도 있는 코드
            //1. 인벤토리가 꽉찼는지 확인하는 조건문

            //Debug.Log("줍줍");
            GetDropItem();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DropItem"))
        {
            InstanceDropItem dropItem = other.GetComponentInParent<InstanceDropItem>();

            dropItemsHash.Add(dropItem);
            //Debug.Log("현재 임시저장소에 들어있는 아이템 갯수는 " + dropItemsHash.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("DropItem"))
        {
            InstanceDropItem dropItem = other.GetComponentInParent<InstanceDropItem>();
            //Debug.Log("현재 임시저장소에 들어있는 아이템 갯수는 " + dropItemsHash.Count);

            dropItemsHash.Remove(dropItem);
        }
    }

    public void Init(Re_Inventory _inventoryDB)
    {
        inventoryDB = _inventoryDB;
    }


    public void GetDropItem()
    {
        //드랍할거 없으면 그냥 return
        if(dropItemsHash.Count == 0)
        {
            return;
        }

        InstanceDropItem real_dropitem = null; // 실제로 획득할 아이템
        float min_dist = 1000000.0f;

        //저장할수도 있는 아이템들을 모아둔 리스트에서 하나하나 거리 계산할 예정 -> 가까운거 먼저 먹어야 자연스러울거같음
        foreach(InstanceDropItem dropitem in dropItemsHash)
        {
            float distance = Vector3.Distance(this.transform.position, dropitem.gameObject.transform.position); // 플레이어와 아이템사이의 거리

            if (distance < min_dist)
            {
                real_dropitem = dropitem;
                min_dist = distance;
            }
        }
        //-> 가까운 아이템 찾기 완료

        if (!InventoryManager.Instance.isPossibleGetItem(real_dropitem.itemID,
            real_dropitem.amount))
        {
            GameEventChannel.OnNotify("인벤토리가 가득찼습니다.");
            return;
        }

        Debug.Log("획득할 아이템 갯수 는" + real_dropitem.amount);
        InventoryManager.Instance.AddItemInInventory(real_dropitem.itemID,
            real_dropitem.amount); // 플레이어 인벤토리에 해당 아이템 아이디를 가진 것을 amount만큼 넣어주세요~
        real_dropitem.ImmediatelyreturnDropItem(); // 쓴건 반환하기
        dropItemsHash.Remove(real_dropitem);

    }
}
