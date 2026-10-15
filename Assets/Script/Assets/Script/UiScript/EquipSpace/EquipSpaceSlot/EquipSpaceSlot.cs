using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSpaceSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action<EquipmentItemInstance> OnInventoryRequest;

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private RectTransform rectTransform;

    private EquipmentItemInstance equip_item;

    [SerializeField]
    private float doubleClickTime = 0.2f;

    private float lastClickTime = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearSlot()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        equip_item = null;
    }

    public void SetItem(EquipmentItemInstance _item)
    {
        equip_item = _item;
        itemIcon.enabled = true;
        itemIcon.sprite = equip_item.setting.itemIcon;
    }

    //마우스 관련 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equip_item == null)
        {
            return;
        }

        UIManager.Instance.ShowEquipToolTip(equip_item, rectTransform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideEquipToolTip();
    }

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

        OnInventoryRequest?.Invoke(equip_item);
    }
}
