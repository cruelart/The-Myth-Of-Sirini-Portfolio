using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawnZone : MonoBehaviour
{
    MonsterPoolManager monsterPoolManager;
    private float currentSpawnTime = 0.0f;
    public bool isMonsterDeadHandle = false;

    List<GameObject> activeMonsterList = new List<GameObject>();

    [SerializeField]
    private float spawnSpace = 10.0f;

    private void Awake()
    {
        monsterPoolManager = GetComponent<MonsterPoolManager>(); // 받아오는건 되잖아? 풀은 비어있을수도 있지만 아마 -> 그러니 나머지는 start에서 처리하자
        //SpawnMonster();
    }

    private void Start()
    {
        SpawnMonster(0.0f); // 리스폰 대기시간 0초 -> 처음에는 바로 실행
        Queue<GameObject> monsterList = monsterPoolManager.monsterPool;

        //Debug.Log("SpawnMonster에 있는 monsterList의 수는" + monsterList.Count);
    }

    private void FixedUpdate()
    {
        //Debug.Log("Physics Tick");
    }

    private void OnEnable()
    {
        float monsterSpawnTime = monsterPoolManager.GetMonsterSpawnTime();
        //기존 몬스터들 스폰
        StartCoroutine(ReSpawnCoroutine()); // 리스폰 대기시간비교

        MonsterEvent.OnMonsterDead -= ReSpawnMonster;
        MonsterEvent.OnMonsterDead += ReSpawnMonster; // 다시 구독
    }

    private void OnDisable()
    {
        MonsterEvent.OnMonsterDead -= ReSpawnMonster; // 비활성화면 구독해제
        StopCoroutine(ReSpawnCoroutine());
        ActiveMonsterReturnPool();
        isMonsterDeadHandle = false; // 비활성화되면 진항하던 몬스터죽음으로 인한 스폰 중지하고 false 처리할게
    }

    private void DeadMonsterReturnPool(MonsterDeadInfo _monsterDeadInfo)
    {
        GameObject monster = _monsterDeadInfo.monsterObj;
        activeMonsterList.Remove(monster);
        monsterPoolManager.ReturnMonster(monster);
    }

    private void ActiveMonsterReturnPool()
    {
        foreach (GameObject monster in activeMonsterList)
        {
            monsterPoolManager.ReturnMonster(monster);
        }
        activeMonsterList.Clear();
    }

    private void SpawnMonster(float _monsterSpawnTime)
    {
        //Queue<GameObject> monsterList = monsterPoolManager.monsterPool;
        //Debug.Log("SpawnMonster에 있는 monsterList의 수는" + monsterList.Count);

        if (Time.time - currentSpawnTime >= _monsterSpawnTime) // 스폰시간이 됐다면
        {
            Queue<GameObject> monsterList = monsterPoolManager.monsterPool;

            while (monsterList.Count != 0)
            {
                GameObject monster = monsterList.Dequeue();
                activeMonsterList.Add(monster);

                monster.transform.position = this.transform.position + new Vector3(Random.Range(-spawnSpace, spawnSpace), 0.0f, Random.Range(-spawnSpace, spawnSpace));
                monster.SetActive(true);
            }
            currentSpawnTime = Time.time;
        }
    }

    private void ReSpawnMonster(MonsterDeadInfo _monsterDeadInfo)
    { 
        DeadMonsterReturnPool(_monsterDeadInfo); // 죽은 몬스터 풀에 되돌리기

        if (!isMonsterDeadHandle)
        {
            isMonsterDeadHandle = true;

            //죽은 몬스터를 다시 넣어주고

            //리스폰 대기시작
            StartCoroutine(ReSpawnCoroutine());
        }
    }

    IEnumerator ReSpawnCoroutine()
    {
        float monsterSpawnTime = monsterPoolManager.GetMonsterSpawnTime();

        yield return new WaitForSeconds(monsterSpawnTime);

        SpawnMonster(monsterSpawnTime);
        isMonsterDeadHandle = false; // 너가 보낸 몬스터사망 이벤트에 관련된거 처리 끝났으니 false 처리할게라고 표현

    }
}
