using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct SkillExecuteData
{
    public Transform casterTransform { get; } // 시전
    public GameObject CasterObj => casterTransform.gameObject; // 시전 대상 오브젝트
    public float casterAttackValue { get; }

    public Collider[] hitBuffer { get; }

    public HashSet<IF_OnDamaged> hitTargets { get; } // 데미지를 입은 대상들

    public SkillExecuteData(Transform _casterTranform, float _casterAttackValue, Collider[] _hitBuffer, HashSet<IF_OnDamaged> _hitTargets)
    {
        casterTransform = _casterTranform;
        casterAttackValue = _casterAttackValue;
        hitBuffer = _hitBuffer;
        hitTargets = _hitTargets;
    }
}

public abstract class SkillExecute : ScriptableObject
{
    public abstract IEnumerator Execute(SkillExecuteData skillExecuteData, SkillData _skillData);
}
