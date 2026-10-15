using UnityEngine;
using BehaviorTree; // 만든 커스텀 테스트용 행동트리 사용예정

public class SlimeAIScript : MonsterAIController, IMonsterDamageable
{
    //먼저 슬라임의 행동트리 구성
    //대기, 공격, 사망, 이동, 피격

    //몬스터 AI
    private Node root; // 루트노드

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        SelectorNode rootSelector = new SelectorNode();
        //root = new SelectorNode(); // 처음엔 설렉터로 가자 -> 이렇게하면 root의 자료형이 Node이기 때문에 ListAdd호출이 불가능

        //공격노드 생성
        ConditionNode attackNode = new ConditionNode(
            () => IsTargetInRange(monsterStatManager.attackRange) || IsAnimationPlaying("Attack", 0.95f), new ActionNode(Attack) // 너무 가까워지면 바로 공격 but 애니메이션 재생중이라면 또 실행
            );

        //추적노드 생성
        ConditionNode chaseNode = new ConditionNode(
            () => ViewAngle.isFindPlayer(monsterStatManager.detectionRange, monsterStatManager.viewAngle, target.transform, transform) , new ActionNode(Chase)
            );

        //피격노드 생성
        ConditionNode reactNode = new ConditionNode(
            () => blackboard.GetData<bool>("IsHitted"), new ActionNode(React)
            );

        //대기노드 생성 -> 얘는 그냥 조건 필요없으니 바로 생성
        ActionNode idleNode = new ActionNode(Idle);

        rootSelector.ListAdd(reactNode);
        rootSelector.ListAdd(attackNode);
        rootSelector.ListAdd(chaseNode);
        rootSelector.ListAdd(idleNode);

        root = rootSelector; // 주소넘겨주기
    }

    // Update is called once per frame
    void Update()
    {
        //최적화
        if (Time.frameCount % 4 == 0)
        {
            root.Evaluate();
        }

    }

    void LateUpdate()
    {
        //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); // 몬스터 뒤집히는거 방지용
        rigid.angularVelocity = Vector3.zero; // 몬스터와 충돌시 몬스터가 빙글빙글 도는 문제 방지용으로 회전에너지 삭제
    }

    private void OnEnable()
    {
        blackboard.SetData("IsHitted", false);
        currentAnime_name = string.Empty;

        if(rigid != null)
        {
            rigid.linearVelocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    //이 함수는 플레이어가 호출할 예정인 함수로 ㄱㄱ
    public void OnDamaged(int _damage, Vector3 _attackerPos)
    {
        blackboard.SetData("IsHitted", true); // 피격상태로 전환

        currentAnime_name = string.Empty;
        return;
    }

    //공격, 추격 범위확인 함수
    bool IsTargetInRange(float _range)
    {
        if (target == null)
            return false;

        return Vector3.Distance(this.transform.position, target.transform.position) < _range;
    }

    void ViewToPlayer()
    {
        Vector3 dir = (target.transform.position - this.transform.position).normalized;

        dir.y = 0f; // 바닥, 하늘 보는거 방지용

        if(dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up); // dir방향을 쿼터니언으로 교체

            this.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, monsterStatManager.statDict[StatType.RotationSpeed].value * Time.deltaTime); // 천천히 회전하게 설정
        }
    }

    void MoveToPlayer()
    {
        ViewToPlayer();

        Vector3 forwardDir = transform.forward;

        Vector3 nextPos = transform.position + (forwardDir * monsterStatManager.statDict[StatType.MoveSpeed].value * Time.deltaTime);
        rigid.MovePosition(nextPos);

    }

    void StopMove()
    {
        rigid.linearVelocity = new Vector3(0, rigid.linearVelocity.y, 0); // y는 중력이므로 생략
    }

    public void AnimationEvent_EndReact()
    {
        blackboard.SetData("IsHitted", false);
        currentAnime_name = string.Empty;
    }


    //데코레이터 조건 함수들
    bool Deco_attack()
    {

        return true;
    }

    tasks Attack()
    {
        StopMove();

        if(!IsAnimationPlaying("Attack", 0.95f) && !ViewAngle.isFindPlayer(monsterStatManager.detectionRange, monsterStatManager.viewAngle, target.transform, transform)) // 공격범위에 있는데 시야각에는 플레이어가 안잡히면 빠르게 회전
        {
            ViewToPlayer();
            return tasks.Running;
        }

        PlayAnime("Attack", 0.1f);
        Debug.Log("슬라임이 공격 상태 진입");

        return tasks.Running; // 공격 진행중
    }

    tasks Chase()
    {
        //먼저 플레이어를 바라보는 방향으로 전환
        //전환 후 실제 플레이어의 위치로 이동하는 로직
        Debug.Log("슬라임이 이동 상태 진입");
        MoveToPlayer();
        PlayAnime("Move", 0.1f);
        return tasks.Running;
    }

    tasks Idle()
    {
        //Debug.Log("슬라임이 대기 상태 진입");
        StopMove();
        PlayAnime("Idle", 0.1f);
        return tasks.Success;
    }

    tasks React()
    {
        Debug.Log("슬라임이 피격 상태 진입");
        StopMove();
        PlayAnime("React", 0.1f);
        return tasks.Success;
    }

    tasks Dead()
    {
        Debug.Log("슬라임 사망");
        StopMove();
        return tasks.Success;
    }
}
