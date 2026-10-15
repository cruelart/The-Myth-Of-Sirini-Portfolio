using UnityEngine;

public interface IMonsterDamageable
{
    void OnDamaged(int _damage, Vector3 _attackerPos);
}
