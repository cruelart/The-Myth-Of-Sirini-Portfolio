using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsumerSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action<int> OnConsumerRequest; // 슬롯번호 전달예정

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TMP_Text countText;

    private ConsumerItemInstance consumer_item;
    //private EquipmentItemInstance equip_item;

    [SerializeField]
    private RectTransform rectTansform;

    public int slotIndex {get; private set;}

    public bool isSettingItem { get; private set; }

    [Header("더블 클릭 설정")]
    [SerializeField]
    private float doubleClickTime = 0.2f;

    private float lastClickTime;

    public void Init(int _slotIndex)
    {
        slotIndex = _slotIndex;
    }

    public void SetItem(ConsumerItemInstance _item)
    {

        isSettingItem = true;
        itemIcon.enabled = true;

        consumer_item = _item;
        //Debug.Log("InventorySlotUI" + item.count);

        itemIcon.sprite = consumer_item.setting.itemIcon;

        if (consumer_item.count > 1)
        {
            Debug.Log("item.count" + consumer_item.count);
            countText.text = consumer_item.count.ToString();
        }
        else
        {
            countText.text = "";
        }
    }

    public void ClearItem()
    {
        isSettingItem = false;

        itemIcon.sprite = null;
        itemIcon.enabled = false;
        countText.text = "";
    }

    //아이템 정보를 보여주는 UI 호출시키기
    public void ShowItemInformation()
    {

    }

    //public void SetItem(EquipmentItemInstance _item)
    //{
    //    item = _item;

    //    itemIcon.sprite = item.setting.itemIcon;

    //    if (item.count > 1)
    //    {
    //        countText.text = item.count.ToString();
    //    }
    //    else
    //    {
    //        countText.text = "";
    //    }
    //}


    //마우스 드래그 아이템 정보관련 함수

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (consumer_item == null)
        {
            Debug.Log("EquipSlotUI에 있는 consumer_item이 비어있어");
            return;
        }
        UIManager.Instance.ShowConsumerToolTip(consumer_item, rectTansform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideConsumerToolTip();
    }

    //아이템 더블 클릭시 소비아이템 사용 함수모음
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.unscaledTime - lastClickTime <= doubleClickTime)
        {
            OnDoubleClick();
        }

        lastClickTime = Time.unscaledTime;
    }

    public void OnDoubleClick()
    {
        if (consumer_item == null)
        {
            return; // 아이템 비어있으면 아무것도 안되야 정상
        }

        OnConsumerRequest?.Invoke(slotIndex);
    }
}
