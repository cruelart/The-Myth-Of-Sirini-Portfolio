using UnityEngine;
using UnityEngine.EventSystems;

public class StoreBuyButton : MonoBehaviour, IPointerClickHandler
{
    private BaseitemDB itemDB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(BaseitemDB _itemDB)
    {
        itemDB = _itemDB;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerGoldManager.Instance.MinusGold(itemDB.buy_gold))
        {
            InventoryManager.Instance.AddItemInInventory(itemDB.itemID, 1);
        }
        else
        {
            GameEventChannel.OnNotify.Invoke("보유하신 재화가 부족합니다");
        }
        
    }
}
