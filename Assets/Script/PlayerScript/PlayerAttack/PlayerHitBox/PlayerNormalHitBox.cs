using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalHitBox : MonoBehaviour
{
    int damage = 0;

    [SerializeField]
    private LayerMask targetLayer;

    [SerializeField]
    private float halfCollriderWidth = 0.35f;

    [SerializeField]
    private float halfColliderHeight = 0.15f;

    [SerializeField]
    private float halfColliderDepth = 0.15f;

    [SerializeField]
    private Vector3 centerOffset = Vector3.zero;

    //-------------------칼 전용 콜리더--------------------------
    private BoxCollider collider = new();

    //-------------------단순 생성 콜리터용----------------------
    private Collider[] colliders = new Collider[10]; // 충돌체를 담을 배열

    private HashSet<IF_OnDamaged> hitTargets = new HashSet<IF_OnDamaged>(); // 중복타격 방지용 Set

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttack(int _attackValue) // -> 플레이어 애니메이션 이벤트스크립트에서 호출
    {
        //Debug.Log($"{gameObject.name} ON");
        hitTargets.Clear(); // 기존 처리했던 몬스터 초기화
        StartCoroutine(AttackExecute(_attackValue, 1f));
        //damage = _attackValue;
    }

    public void SwordStartAttack(int _attackValue)
    {
        hitTargets.Clear();
        damage = _attackValue;
        collider.enabled = true;
    }

    public void EndAttack() // -> 플레이어 애니메이션 이벤트스크립트에서 호출
    {
        collider.enabled = false;
        //Debug.Log($"{gameObject.name} OFF");
    }

    public void CancelAttack() // 예상치 못한 피격이나 사망으로 애니메이션 끊길때 호출하는 예외 함수
    {
        EndAttack();
        hitTargets.Clear();
    }

    public IEnumerator AttackExecute(int _attackValue, float _damageMultiplier)
    {
        damage = Mathf.RoundToInt(_attackValue * _damageMultiplier); // 데미지 계산 -> Int로 전환

        Vector3 center = this.transform.position + centerOffset;

        Vector3 halfExtents = new Vector3(halfCollriderWidth, halfColliderHeight, halfColliderDepth);

        int hitCount = Physics.OverlapBoxNonAlloc(center, halfExtents, colliders, this.transform.rotation, targetLayer, QueryTriggerInteraction.Collide);

        for (int i = 0; i < hitCount; i++)
        {
            Collider detectedCollider = colliders[i];
            if (detectedCollider.transform.root == this.transform.root) // 시전자 제외 (콜리더에 재수없게 걸리는거 방지)
            {
                continue;
            }
            IF_OnDamaged target = detectedCollider.GetComponentInParent<IF_OnDamaged>(); // 피격가능 여부 따지고
            if (target == null) // 안되면 바로 return처리
            {
                continue;
            }
            if (!hitTargets.Add(target)) // 이미 한대 때렸으면 pass하자
            {
                continue;
            }
            Vector3 hitPoint = detectedCollider.ClosestPoint(center); // 데미지 텍스트를 위한 타격지점
            Vector3 hitDir = (detectedCollider.bounds.center - center).normalized;
            target.OnDamaged(damage, this.transform.root.gameObject, DamageType.Normal, hitPoint, hitDir); // 맞으면 피격처리
        }

        yield return null; // 물리연산이 끝난 후에 실행되도록 함
    }

    //칼을 이거로 처리해야 될거같아서 이걸로 처리할 예정
    private void OnTriggerEnter(Collider _col)
    {
        if (_col.TryGetComponent<IF_OnDamaged>(out IF_OnDamaged target)) // 공격이 가능한 상대이면
        {
            if (_col.transform.root == this.transform.root) // 자기 자신 때리기 방어
            {
                return;
            }

            if (hitTargets.Contains(target)) // 이미 한대 때렸으면 pass하자
                return;

            hitTargets.Add(target); // 맞은 애는 중복처리방지를 위해 Set에 넣어줌

            Vector3 hitPoint = _col.ClosestPoint(transform.position);

            Vector3 hitDir = (_col.bounds.center - transform.position).normalized;

            target.OnDamaged(damage, transform.root.gameObject, DamageType.Normal, hitPoint, hitDir); // 맞으면 피격처리
        }
    }
}
