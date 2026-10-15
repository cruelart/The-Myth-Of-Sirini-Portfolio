using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipToolTip : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Image itemIcon;

    [SerializeField]
    private TextMeshProUGUI itemName;

    [SerializeField]
    private TextMeshProUGUI stat_Attack;

    [SerializeField]
    private TextMeshProUGUI stat_Defense;

    [SerializeField]
    private TextMeshProUGUI stat_Mental;

    [SerializeField]
    private TextMeshProUGUI stat_MoveSpeed;

    [SerializeField]
    private TextMeshProUGUI stat_Str;

    [SerializeField]
    private TextMeshProUGUI stat_Dex;

    [SerializeField]
    private TextMeshProUGUI stat_Int;

    [SerializeField]
    private TextMeshProUGUI stat_Luk;

    [SerializeField]
    private TextMeshProUGUI itemDescription;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(EquipmentItemInstance _item, RectTransform slotRect)
    {
        this.gameObject.SetActive(true);

        itemIcon.sprite = _item.setting.itemIcon;
        itemName.text = _item.setting.itemName + "(+" + _item.upgradeLv.ToString() + ")";
        itemDescription.text = _item.setting.description;

        stat_Attack.text = _item.GetStat(StatType.Attack).ToString();
        stat_Defense.text = _item.GetStat(StatType.Defense).ToString();
        stat_Mental.text = _item.GetStat(StatType.Mental).ToString();
        stat_MoveSpeed.text = _item.GetStat(StatType.MoveSpeed).ToString();
        stat_Str.text = _item.GetStat(StatType.STR).ToString();
        stat_Dex.text = _item.GetStat(StatType.DEX).ToString();
        stat_Int.text = _item.GetStat(StatType.INT).ToString();
        stat_Luk.text = _item.GetStat(StatType.LUK).ToString();

        DecidePosition(slotRect);
    }

    public void Hide()
    {
        itemIcon.sprite = null;
        itemName.text = "item Name";

        stat_Attack.text = "0";
        stat_Defense.text = "0";
        stat_Mental.text = "0";
        stat_MoveSpeed.text = "0";
        stat_Str.text = "0";
        stat_Dex.text = "0";
        stat_Int.text = "0";
        stat_Luk.text = "0";

        this.gameObject.SetActive(false);
    }

    private void DecidePosition(RectTransform _rect)
    {
        Canvas.ForceUpdateCanvases();

        Vector3 pos = _rect.position;
        rect.position = pos;

        ControllUIPosition();
    }

    private void ControllUIPosition()
    {
        Vector3[] corners = new Vector3[4]; // 화면 밖 삐져나가는거 확인용
        rect.GetWorldCorners(corners);

        Vector3 move_pos = Vector3.zero; // 필요 움직임 벡터

        //오른쪽 위
        if (corners[2].x > Screen.width)
        {
            move_pos.x -= corners[2].x - Screen.width;
        }

        if (corners[2].y > Screen.height)
        {
            move_pos.y -= corners[2].y - Screen.height;
        }

        //왼쪽 아래 (삐져나간 만큼 채워넣기)
        if (corners[0].x < 0)
        {
            move_pos.x -= corners[0].x;
        }

        if (corners[0].y < 0)
        {
            move_pos.y -= corners[0].y;
        }

        rect.position += move_pos;
    }
}
