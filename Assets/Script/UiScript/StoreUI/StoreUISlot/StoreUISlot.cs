using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StoreUISlot : MonoBehaviour
{
    private BaseitemDB itemDB; // 아이템의 정보를 저장해야 UI에 입력가능하다고 판단

    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TextMeshProUGUI itemNameText;

    [SerializeField]
    private TextMeshProUGUI itemPriceText;

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

        //이미지 적용
        itemImage.sprite = itemDB.itemIcon;

        //아이템 이름 적용
        itemNameText.text = itemDB.name;

        itemPriceText.text = itemDB.buy_gold.ToString() + " 루비";
    }

    public void ClickBuyButton()
    {
        UIManager.Instance.TraderSellButton.UIOpen();
        UIManager.Instance.TraderSellButton.Init(itemDB);
        Debug.Log("뭉뭉탱탱");
    }
}
