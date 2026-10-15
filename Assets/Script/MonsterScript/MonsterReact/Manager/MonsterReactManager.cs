using UnityEngine;

public class MonsterReactManager : MonoBehaviour, IF_OnDamaged
{
    private MonsterStatManager monsterStatManager;
    private IMonsterDamageable monsterDamageable;

    private Color damageColor = new Color(1,1,0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(MonsterStatManager _monsterStatmanager)
    {
        monsterStatManager = _monsterStatmanager;
        monsterDamageable = GetComponent<IMonsterDamageable>();
    }

    public void OnDamaged(int _damage, GameObject _attacker, DamageType _damageType, Vector3 _hitPoint, Vector3 _hitDir)
    {
        Vector3 attackerPosition = _attacker != null ? _attacker.transform.position : transform.position;

        monsterDamageable?.OnDamaged(_damage, attackerPosition);

        if (monsterStatManager != null)
        {
            monsterStatManager.OnDamaged(_damage, _attacker);
            
            DamageTextManager.Instance.ShowDamageText(this.transform, _damage, damageColor);

            EffectPoolManager.Instance.PlayEffect(EffectType.React1, _hitPoint, _hitDir);
        }
    }
}
