using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterStatManager : MonoBehaviour
{
    private MonsterDB monsterDB;

    public System.Action OnDeath;
    public Dictionary<StatType, Stat> statDict = new Dictionary<StatType, Stat>();

    public event Action<float, float> OnHPChange;

    //몬스터의 현재 상태
    float current_monsterHp;
    float current_monsterMp;

    public int Monster_exp { get; private set; } = 100;

    public PlayerStatManager playerStatManager { get; private set; } = null;

    //상태이상에 따라 조정할 수 있으므로 MonsterDB 말고 여기다 넣자
    public float viewAngle = 140.0f;

    public float detectionRange = 10;

    public float attackRange = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        current_monsterHp = GetStat(StatType.HP);
        current_monsterMp = GetStat(StatType.MP);

        viewAngle = monsterDB.viewAngle;
        detectionRange = monsterDB.detectionRange;
        attackRange = monsterDB.attackRange;

        OnHPChange?.Invoke(current_monsterHp, GetStat(StatType.HP));
    }

    public void Init(MonsterDB _data)
    {
        monsterDB = _data;

        statDict = monsterDB.stats.ToDictionary(s => s.type);

        //처음 스텟 초기화
        current_monsterHp = GetStat(StatType.HP);
        current_monsterMp = GetStat(StatType.MP);
        viewAngle = monsterDB.viewAngle;
        detectionRange = monsterDB.detectionRange;
        attackRange = monsterDB.attackRange;

        OnHPChange?.Invoke(current_monsterHp, GetStat(StatType.HP));
    }

    public void OnDamaged(int _damage, GameObject _attacker)
    {
        current_monsterHp -= (_damage - (int)GetStat(StatType.Defense));
        playerStatManager = _attacker.GetComponent<PlayerStatManager>();
        
        OnHPChange?.Invoke(current_monsterHp, GetStat(StatType.HP));

        if (current_monsterHp < 0)
        {
            current_monsterHp = 0;
            OnDeath?.Invoke();
        }
    }

    public float GetStat(StatType _type)
    {
        return statDict[_type].value;
    }

    public void SetKiller(PlayerStatManager _killer)
    {
        playerStatManager = _killer;
    }
}
