using UnityEngine;

public class MonsterNormalHitBox : MonoBehaviour
{
    private MonsterStatManager monsterStatManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(MonsterStatManager _monsterStatManager)
    {
        monsterStatManager = _monsterStatManager;
    }

    private void OnTriggerEnter(Collider _col)
    {
        //Debug.Log("뭔가 충돌하긴함");
        if (_col.gameObject.CompareTag("Player"))
        {
            IF_OnDamaged it_playerDamaged = _col.gameObject.GetComponent<IF_OnDamaged>();

            if(it_playerDamaged != null)
            {
                //Debug.Log("몬스터가 플레이어를 공격성공");
                it_playerDamaged.OnDamaged((int)monsterStatManager.statDict[StatType.Attack].value, transform.root.gameObject, DamageType.Normal, new Vector3(0,0,0), new Vector3(0,0,0)); // 일반 데미지를 입힌다.
            }
        }
    }
}
