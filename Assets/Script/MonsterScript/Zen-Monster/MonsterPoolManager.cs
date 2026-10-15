using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//모든 몬스터의 풀을 들고있음
public class MonsterPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public int monsterID = -1;
        public GameObject monsterPrefab;

        public int poolSize = 10;
        public float respawnTime = 5.0f;
    }

    [SerializeField]
    private SpawnEntry monsterSpawnEntry;

    public Queue<GameObject> monsterPool = new Queue<GameObject>();

    private void Awake()
    {
        CreateMonsterInPool();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(monsterPool.Count);
    }

    private void CreateMonsterInPool() // 풀에 들어갈 몬스터 생성
    {
        //GameObject Monster = Instantiate(monsterPrefab, this.transform); // 해당 스크립트를 지닌 오브젝트의 하위에 몬스터 생성
        //Monster.SetActive(false); // 처음에는 전부 안보이게 그냥 초기 생성용
        //monsterPool.Enqueue(Monster);

        for (int i = 0; i < monsterSpawnEntry.poolSize; i++)
        {
            GameObject monster = Instantiate(monsterSpawnEntry.monsterPrefab, this.transform);

            monster.SetActive(false);

            //MonsterBase monsterBaseComp = monster.GetComponent<MonsterBase>();

            //Debug.Log("뭉탱이:" + data.monsterType);
            //monsterBaseComp.SetMonsterType(data.monsterType);

            monsterPool.Enqueue(monster);
        }
    }

    public void ReturnMonster(GameObject _monster)
    {
        _monster.SetActive(false); // 비활성화 하고
        monsterPool.Enqueue(_monster); // 다시 넣어줌
    }


    public float GetMonsterSpawnTime()
    {
        return monsterSpawnEntry.respawnTime;    
    }

}
