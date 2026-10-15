using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance;

    [SerializeField]
    private DamageText damageTextPrefab;

    [SerializeField]
    private Canvas damageCanvas;

    [SerializeField]
    private float poolSize = 30;

    private Queue<DamageText> damageTextPool = new Queue<DamageText>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        CreatePool();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void CreatePool()
    {
        for(int i = 0; i < poolSize; i++)
        {
            DamageText damageText = Instantiate(damageTextPrefab, damageCanvas.transform);
            damageText.gameObject.SetActive(false);

            damageTextPool.Enqueue(damageText);
        }
    }

    public void ShowDamageText(Transform _targetTransform, int _damage, Color _color)
    {
        DamageText damageText = damageTextPool.Dequeue();

        Vector3 showPos = Camera.main.WorldToScreenPoint(_targetTransform.position + new Vector3(Random.Range(-0.4f,0.4f), 1.3f, Random.Range(-0.4f,0.4f)));
        damageText.GetRectTransform().position = showPos;
        damageText.Setup(_damage, _color);

        //설정 완료 후 활성화
        damageText.gameObject.SetActive(true);
    }

    public void ReturnDamageText(DamageText _damageText)
    {
        _damageText.gameObject.SetActive(false);
        damageTextPool.Enqueue(_damageText);
    }
}
