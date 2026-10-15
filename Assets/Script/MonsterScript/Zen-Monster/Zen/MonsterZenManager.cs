using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterZenManager : MonoBehaviour
{
    //private void Awake()
    //{
    //    MonsterEvent.OnMonsterDead += ReZenMonster;
    //}
    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //public void SpawnMonster(int _monsterID, Transform _zenTransform)
    //{
    //    Queue<GameObject> pool = MonsterPoolManager.instance.GetMonsterPoolQueue(_monsterID);

    //    Debug.Log("현재 큐안에 들어있는 몬스터의 수는 " + pool.Count);

    //    if (pool.Count <= 0)
    //    {
    //        return; // 이미 모든 몬스터를 내보냈으므로 return
    //    }

    //    while (pool.Count > 0)
    //    {
    //        GameObject monster = pool.Dequeue();

    //        monster.transform.position = _zenTransform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    //        monster.SetActive(true);

    //    }
    //    //while (monsterPool.Count > 0) // 풀에 아직도 몬스터가 남아있다면
    //    //{
    //    //    GameObject monster = monsterPool.Dequeue(); // 몬스터 뺴오고

    //    //    // 랜덤위치에 스폰시킴
    //    //    monster.transform.position = this.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    //    //    monster.SetActive(true);
    //    //}
    //    //풀에 몬스터가 남아있지 않다는건 필드에 이미 정해둔 수만큼의 몬스터가 젠이 됐기때문에 아무것도 안함
    //}

    //private void ReZenMonster(int _monsterID, GameObject _gameObject)
    //{
    //    MonsterSpawnObject mosnterSpawnObj = MonsterPoolManager.instance.GetMonsterSpawnObject(_monsterID);

    //    //StartCoroutine(SpawnCoroutine(mosnterSpawnObj));
    //}


    ////IEnumerator SpawnCoroutine(MonsterSpawnObject _mosnterSpawnObj)
    ////{
    //    //yield return new WaitForSeconds(_mosnterSpawnObj.spawnTime);
    //    //SpawnMonster(_mosnterSpawnObj.monsterID, _mosnterSpawnObj.monsterSpawnTransform);
    ////}
}
