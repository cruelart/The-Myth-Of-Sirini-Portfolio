using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "FrozenSkill2", menuName = "Scriptable Objects/FrozenSkill2")]
public class FrozenSkill2Execute : SkillExecute
{
    [SerializeField]
    private LayerMask targetLayer;

    [SerializeField]
    private float halfCollriderWidth = 3f;

    [SerializeField]
    private float halfColliderHeight = 3f;

    [SerializeField]
    private float halfColliderDepth = 3f;

    [Header("이펙트")]
    [SerializeField]
    private Vector3 effectLocalOffset = new Vector3(0f, 0f, 1.5f);

    public override IEnumerator Execute(SkillExecuteData _executeData, SkillData _skillData)
    {
        Transform caster = _executeData.casterTransform;

        int damage = Mathf.RoundToInt(_executeData.casterAttackValue * _skillData.damageMultiplier); // 데미지 계산 -> Int로 전환

        Vector3 effectPosition = caster.TransformPoint(effectLocalOffset);

        //이펙트 보여주기
        if (EffectPoolManager.Instance != null)
        {
            EffectPoolManager.Instance.PlayEffect(_skillData.effectType, effectPosition, caster.forward);
        }

        Vector3 center = caster.position;

        Vector3 halfExtents = new Vector3(halfCollriderWidth, halfColliderHeight, halfColliderDepth);

        int hitCount = Physics.OverlapBoxNonAlloc(center, halfExtents, _executeData.hitBuffer, caster.rotation, targetLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hitCount; i++)
        {
            Collider detectedCollider = _executeData.hitBuffer[i];

            if (detectedCollider.transform.root == caster.root) // 시전자 제외 (콜리더에 재수없게 걸리는거 방지)
            {
                continue;
            }

            IF_OnDamaged target = detectedCollider.GetComponentInParent<IF_OnDamaged>(); // 피격가능 여부 따지고

            if (target == null) // 안되면 바로 return처리
            {
                continue;
            }

            if (!_executeData.hitTargets.Add(target))
            {
                continue;
            }

            Vector3 hitPoint = detectedCollider.ClosestPoint(center); // 데미지 텍스트를 위한 타격지점

            Vector3 hitDirection = (detectedCollider.bounds.center - caster.position).normalized;

            target.OnDamaged(damage, _executeData.CasterObj, DamageType.Strong, hitPoint, hitDirection); // 그리고 실제 데미지 처리하고
        }

        yield return null;
    }
}
