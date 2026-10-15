using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public int skillID;
    public string skillName;
    public string animationName; // 실행시킬 애니메이션 이름

    public float coolTime; // 쿨타임

    public EffectType effectType;

    public bool useAnimationRootMotion = true; // 애니메이션 루트모션 사용 여부

    [Min(0f)]
    public float damageMultiplier = 1.0f; // 데미지 배율

    public SkillExecute skillExecute; // 스킬 실행 스크립트

    public Sprite skillIcon; // 스킬 아이콘
}
