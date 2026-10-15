using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterHpUIManager : MonoBehaviour
{
    [SerializeField]
    private Image HpBar;

    [SerializeField]
    private Image HpBarBackground;

    [SerializeField]
    private float showDistance = 10;

    private MonsterStatManager monsterStatManager;

    private void Awake()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward; // UI가 카메라 정면 바라보게함

        float dist = Vector3.Distance(Camera.main.transform.position, this.transform.position);

        HpBar.gameObject.SetActive(dist <= showDistance);
        HpBarBackground.gameObject.SetActive(dist <= showDistance);
    }

    public void Init(MonsterStatManager _monsterStatManager)
    {
        monsterStatManager = _monsterStatManager;

        monsterStatManager.OnHPChange += UpdateHP;
    }

    public void UpdateHP(float _currentHP, float _MaxHP)
    {
        HpBar.fillAmount = _currentHP / _MaxHP;
    }
}
