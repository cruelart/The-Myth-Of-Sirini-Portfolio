using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MonsterDeathHandler : MonoBehaviour
{
    private MonsterStatManager monsterStatManager; // 구독할 곳
    private MonsterBase monsterBase;
    private MonsterDeadInfo monsterDeadInfo; // 몬스터가 죽으면 내보내야될 정보들


    private int monsterID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int _monsterID, MonsterStatManager _monsterStatManager, MonsterBase _monsterBase)
    {
        Debug.Log("Init 호출");

        monsterID = _monsterID;
        monsterStatManager = _monsterStatManager;
        monsterBase = _monsterBase;

        monsterStatManager.OnDeath -= HandleDeath;
        monsterStatManager.OnDeath += HandleDeath;

       monsterDeadInfo = new MonsterDeadInfo(monsterID, this.gameObject, monsterBase.spawnZone, null);
    }
    private void HandleDeath()
    {
        monsterDeadInfo.killerPlayer = monsterStatManager.playerStatManager; // 누가 죽였는지 정보 넣어주기
        MonsterEvent.RaiseMonsterDead(monsterDeadInfo);
        //MonsterZen.instance.ReturnMonster(monsterType, gameObject);
    }

    private void OnEnable()
    {
        if (monsterStatManager != null)
        {
            monsterStatManager.OnDeath -= HandleDeath;
            monsterStatManager.OnDeath += HandleDeath;
        }
    }

    private void OnDisable()
    {
        if (monsterStatManager != null)
        {
            monsterStatManager.OnDeath -= HandleDeath;
        }
    }
}
