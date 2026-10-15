using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "FrozenSkillExecute", menuName = "Scriptable Objects/FrozenSkillExecute")]
public sealed class FrozenSkillExecute : SkillExecute
{
    [Header("범위 판정")]
    [SerializeField]
    private LayerMask targetLayer;

    [SerializeField]
    private int colliderboxCount = 10; // 생성할 충돌 박스의 갯수

    [SerializeField]
    private float firstDistance = 1.2f;

    [SerializeField]
    private float distanceStep = 1.2f;

    [SerializeField]
    private float firstHalfWidth = 0.7f;

    [SerializeField]
    private float widthStep = 0.35f;

    [SerializeField]
    private float halfHeight = 0.8f;

    [SerializeField]
    private float halfDepth = 0.65f;

    [SerializeField]
    private float colliderBoxZenTime = 0.08f;

    [Header("이펙트")]
    [SerializeField]
    private Vector3 effectLocalOffset = new Vector3(0f, 0f, 1.5f);

    [Header("디버그")]
    [SerializeField]
    private bool drawDebugBox = true;

    [SerializeField]
    private float debugDrawDuration = 1f;

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

        WaitForSeconds stageWait = new WaitForSeconds(colliderBoxZenTime);

        for (int stage = 0; stage < colliderboxCount; stage++)
        {
            float distance = firstDistance + distanceStep * stage; // 원뿔 모양으로 만들거니까 각 거리조절

            float halfColliderWidth = firstHalfWidth + widthStep * stage; // 충돌박스의 반쪽 너비 계산

            Vector3 center = caster.position + caster.forward * distance + Vector3.up * halfHeight;

            Vector3 halfExtents = new Vector3(halfColliderWidth, halfHeight, halfDepth); // OverlabBoxNonAlloc이 축 별로 절반 크기 받으니까 절반 크기 박스 생성하고

            Quaternion boxRotation = caster.rotation;
            if (drawDebugBox)
            {
                float stageRatio =
                    colliderboxCount <= 1
                    ? 0f
                    : (float)stage / (colliderboxCount - 1);

                Color boxColor =
                    Color.Lerp(Color.cyan, Color.red, stageRatio);

                DrawWireBox(
                    center,
                    halfExtents,
                    boxRotation,
                    boxColor,
                    debugDrawDuration
                );
            }

            int hitCount = Physics.OverlapBoxNonAlloc(center, halfExtents, _executeData.hitBuffer, caster.rotation, targetLayer, QueryTriggerInteraction.Collide); // 충돌한 애들의 갯수

            for (int i = 0; i < hitCount; i++)
            {
                Collider detectedCollider =  _executeData.hitBuffer[i];

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

            if (stage < colliderboxCount - 1 && colliderBoxZenTime > 0f)
            {
                yield return stageWait;
            }
        }
    }

    //-> 씬 참고용 복붙코드
    private void DrawWireBox(
    Vector3 center,
    Vector3 halfExtents,
    Quaternion rotation,
    Color color,
    float duration)
    {
        Vector3[] corners = new Vector3[8];

        corners[0] = center + rotation * new Vector3(
            -halfExtents.x,
            -halfExtents.y,
            -halfExtents.z
        );

        corners[1] = center + rotation * new Vector3(
            halfExtents.x,
            -halfExtents.y,
            -halfExtents.z
        );

        corners[2] = center + rotation * new Vector3(
            halfExtents.x,
            -halfExtents.y,
            halfExtents.z
        );

        corners[3] = center + rotation * new Vector3(
            -halfExtents.x,
            -halfExtents.y,
            halfExtents.z
        );

        corners[4] = center + rotation * new Vector3(
            -halfExtents.x,
            halfExtents.y,
            -halfExtents.z
        );

        corners[5] = center + rotation * new Vector3(
            halfExtents.x,
            halfExtents.y,
            -halfExtents.z
        );

        corners[6] = center + rotation * new Vector3(
            halfExtents.x,
            halfExtents.y,
            halfExtents.z
        );

        corners[7] = center + rotation * new Vector3(
            -halfExtents.x,
            halfExtents.y,
            halfExtents.z
        );

        // 아래쪽 사각형
        Debug.DrawLine(corners[0], corners[1], color, duration);
        Debug.DrawLine(corners[1], corners[2], color, duration);
        Debug.DrawLine(corners[2], corners[3], color, duration);
        Debug.DrawLine(corners[3], corners[0], color, duration);

        // 위쪽 사각형
        Debug.DrawLine(corners[4], corners[5], color, duration);
        Debug.DrawLine(corners[5], corners[6], color, duration);
        Debug.DrawLine(corners[6], corners[7], color, duration);
        Debug.DrawLine(corners[7], corners[4], color, duration);

        // 위아래 연결선
        Debug.DrawLine(corners[0], corners[4], color, duration);
        Debug.DrawLine(corners[1], corners[5], color, duration);
        Debug.DrawLine(corners[2], corners[6], color, duration);
        Debug.DrawLine(corners[3], corners[7], color, duration);
    }
}
