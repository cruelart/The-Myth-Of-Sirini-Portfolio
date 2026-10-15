using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IsTrueTraderSellButton : UIBase
{
    private BaseitemDB itemDB; // 사게된다면 제공할 아이템의 데이터

    [SerializeField]
    private TextMeshProUGUI notifyBuy; // 살건지 안살건지 안내문

    [SerializeField]
    private StoreBuyButton storeBuyButton; // 구매 버튼
    
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

        storeBuyButton.Init(itemDB);
        notifyBuy.text = itemDB.itemName.ToString() + "을 구매하시겠습니까?";
    }

    public void OnDisable()
    {
        notifyBuy.text = "Error";
    }
}
