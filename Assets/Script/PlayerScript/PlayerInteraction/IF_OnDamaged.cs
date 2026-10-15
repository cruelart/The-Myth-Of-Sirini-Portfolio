using UnityEngine;

public enum DamageType
{
    Normal,
    Strong
}
public interface IF_OnDamaged
{
    void OnDamaged(int _damage, GameObject _attacker, DamageType _damageType, Vector3 _hitPoint, Vector3 _hitDir); // 데미지 함수
}
