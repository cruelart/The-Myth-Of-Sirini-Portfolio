using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action<int> OnEquipRequest; // 슬롯번호 전달예정

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TMP_Text countText;

    private EquipmentItemInstance equip_item;

    [SerializeField]
    private RectTransform rectTansform;

    public int slotIndex { get; private set; }

    public bool isSettingItem { get; private set; }

    [Header("더블 클릭 설정")]
    [SerializeField]
    private float doubleClickTime = 0.2f;

    private float lastClickTime;

    public void Init(int _slotIndex)
    {
        slotIndex = _slotIndex;
        ClearItem();
    }

    public void SetItem(EquipmentItemInstance _item)
    {

        //전달받은 아이템이 비어있으면 Clear시켜버리기
        if (_item == null)
        {
            ClearItem();
            return;
        }

        isSettingItem = true;

        itemIcon.enabled = true;
        equip_item = _item;
        //Debug.Log("InventorySlotUI" + item.count);

        itemIcon.sprite = equip_item.setting.itemIcon;

        countText.text = "";
    }

    public void ClearItem()
    {
        equip_item = null;
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

    //----------------------------------------------------------------

    //마우스 드래그 아이템 정보관련 함수

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(equip_item == null)
        {
            Debug.Log("EquipSlotUI에 있는 equip_item이 비어있어");
            return;
        }
        UIManager.Instance.ShowEquipToolTip(equip_item, rectTansform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideEquipToolTip();
    }

    //------------------------------------------------------------------

    //아이템 더블 클릭시 장비창으로 끼게 하는 함수모음
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
        if (equip_item == null)
        {
            return; // 아이템 비어있으면 아무것도 안되야 정상
        }

        OnEquipRequest?.Invoke(slotIndex);
    }
}
