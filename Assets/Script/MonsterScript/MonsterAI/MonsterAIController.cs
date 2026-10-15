using UnityEngine;
using BehaviorTree;

public abstract class MonsterAIController : MonoBehaviour
{
    protected MonsterStatManager monsterStatManager;
    //------------------------------
    protected GameObject target;

    //몬스터 애니메이션
    protected Animator animator;
    protected string currentAnime_name = ""; // 실행 중인 애니메이션 저장

    //몬스터 RigidBody
    protected Rigidbody rigid;

    //블랙보드
    protected Blackboard blackboard = new Blackboard(); // 슬라임에 필요한 데이터를 다루는 블랙보드생성


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // X축과 Z축 회전을 막아서 이상한 회전 방지하기
    }

    protected virtual void Start()
    {
        target = GameObject.FindWithTag("Player");
    }
    //--------------------------------------초기화-----------------------------------
    public void Init(MonsterStatManager _monsterStatManager)
    {
        monsterStatManager = _monsterStatManager;
    }

    //--------------------------------------애니메이션--------------------------------

    //애니메이션 재생 함수
    protected void PlayAnime(string _anime_name, float crossFade)
    {
        if (currentAnime_name == _anime_name) return; // 애니메이션이 같으면 바꿀필요 없음 -> 이렇게 되면 공격을 계속해야되는데 문제생김(한번 공격하고 맘)

        animator.CrossFade(_anime_name, crossFade); // 애니메이션간 부드럽게 교체하고
        currentAnime_name = _anime_name; // 실행 중인 애니메이션 교체
    }

    //애니메이션이 실행중인지 체크
    protected bool IsAnimationPlaying(string _anime_name, float stopTime)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0); // 현재 재생중인 애니메이션
        return info.IsName(_anime_name) && info.normalizedTime < stopTime; // 의 이름이 _anime_name이면서 실행시간이 멈추는 시간보다 짧다면
    }

    //-------------------------------------------------------------------------------

    //몬스터 사망시
    public void MonsterDead(MonsterDeadEventData _data)
    {

    }
}
