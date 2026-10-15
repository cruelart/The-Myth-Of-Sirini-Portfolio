using System.Collections.Generic;
using UnityEngine;

public class MonsterDBManager : MonoBehaviour
{
    public static MonsterDBManager Instance;
    //모든 몬스터의 데이터를 담을 해시테이블 선언
    private Dictionary<int, MonsterDB> monsterDBMap = new Dictionary<int, MonsterDB>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(transform.root.gameObject); // 파괴 방지

        LoadData();
    }

    public MonsterDB GetMonsterDB(int _monsterID)
    {
        return monsterDBMap[_monsterID];
    }

    private void LoadData()
    {
        MonsterDB[] monster = Resources.LoadAll<MonsterDB>("monsters");

        foreach(MonsterDB monsterDB in monster)
        {
            if(monsterDBMap.ContainsKey(monsterDB.monsterID))
            {
                continue;
            }

            monsterDBMap.Add(monsterDB.monsterID, monsterDB);
        }
    }
}
