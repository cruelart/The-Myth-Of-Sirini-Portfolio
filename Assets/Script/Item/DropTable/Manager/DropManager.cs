using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public static DropManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        MonsterEvent.OnMonsterDead += DropItem;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DropItem(MonsterDeadInfo _monsterDeadInfo)
    {
        Debug.Log("아이템 드랍코드 실행");
        List<DropTableEntry> monsterDropList =
            DropSystem.GetMonsterDropList(_monsterDeadInfo.monsterID);

        foreach(DropTableEntry dropTableEntry in monsterDropList)
        {
            int itemID = dropTableEntry.itemID;
            int amount = Random.Range(dropTableEntry.minCount,
                dropTableEntry.maxCount);

            float randomNum = Random.Range(0f, 100f);
            //확률 싸움하기
            if(randomNum <= dropTableEntry.dropPercent)
            {
                //아이템떨구기 코드작성하기
                //1. 드랍아이템 풀매니저에다가 아이템 아이디를 넘겨서 풀에서 바닥에 떨굴 오브젝트를 내보낼거임
                DropItemPoolManager.Instance.DropItem(itemID, 
                    amount, 
                    _monsterDeadInfo.monsterObj.transform.position);
                //Debug.Log("떨굴 갯수는 총" + amount);
                Debug.Log("몬스터가 사망하여 아이템을 지급합니다");
                continue;
            }
        }
    }
}
