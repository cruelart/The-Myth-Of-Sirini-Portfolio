using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ConsumerToolTip : MonoBehaviour
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
    private TextMeshProUGUI itemDescription;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(ConsumerItemInstance _item, RectTransform slotRect)
    {
        this.gameObject.SetActive(true);

        itemIcon.sprite = _item.setting.itemIcon;
        itemName.text = _item.setting.itemName;
        itemDescription.text = _item.setting.description;

        DecidePosition(slotRect);
    }

    public void Hide()
    {
        itemIcon.sprite = null;
        itemName.text = "item Name";


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
