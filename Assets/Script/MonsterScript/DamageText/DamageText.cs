using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI damageText;

    private float textSpeed; // 데미지 텍스트가 올라가는 속도
    private float lifeTime; // 유지시간
    private float currentTime;
    private Vector3 baseScale;

    public Color textColor; // 일반 데미지 -> 흰색 , 크리티컬 -> 노란색

    private RectTransform rectTransform;

    private void Awake()
    {
        baseScale = this.transform.localScale;
        rectTransform = GetComponent<RectTransform>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * textSpeed * Time.deltaTime;

        currentTime += Time.deltaTime;

        textColor.a = 1.0f - currentTime / lifeTime; // 점점 희미해져가게
        damageText.color = textColor; // 실시간으로 색변경
        transform.localScale -= Vector3.one * Time.deltaTime * 0.5f;

        if(currentTime > lifeTime)
        {
            DamageTextManager.Instance.ReturnDamageText(this);
        }
    }

    public RectTransform GetRectTransform()
    {
        return rectTransform;
    }
    
    public void Setup(int _damage, Color _color)
    {
        damageText.text = "-" + _damage.ToString(); // 데미지를 문자열로 변환

        textSpeed = 100.0f;
        lifeTime = 1.0f;
        currentTime = 0.0f;

        textColor = _color;
        textColor.a = 1.0f;
        damageText.color = textColor; // 원래 색으로 초기화
        transform.localScale = baseScale;

        gameObject.SetActive(true);
    }
}
