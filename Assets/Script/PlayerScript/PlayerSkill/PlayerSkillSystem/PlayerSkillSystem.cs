using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillSystem : MonoBehaviour
{
    //모든 스킬 데이테베이스 참조
    [SerializeField]
    private SkillDatabase skillDatabase;

    //플레이어가 사용할 스킬 선정
    PlayerSkillRunTime[] playerSkillRunTimes = new PlayerSkillRunTime[8]; // 8개 스킬 사용으로 일단 선언

    private readonly Collider[] skillHitBuffer = new Collider[64]; // 생성파괴하기보단 미리 배열 생성

    private readonly HashSet<IF_OnDamaged> skillHitTargets = new HashSet<IF_OnDamaged>();

    private void Awake()
    {
        if(skillDatabase != null)
        {
            skillDatabase.Init();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(EquipSkill(0,1))
        {
            Debug.Log("장착완료");
        }

        if (EquipSkill(1, 2))
        {
            Debug.Log("장착완료");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool EquipSkill(int _index, int _skillID) // 사용할 스킬 세팅
    {
        if (_index < 0 || _index >= playerSkillRunTimes.Length)
        {
            Debug.LogError($"잘못된 인덱스 접근임 말도 안되는 인덱스니까 다시 확인조치");
            return false;
        }

        if(!skillDatabase.GetSkillData(_skillID, out SkillData skillData))
        {
            Debug.LogError($"있지도 않은 스킬데이터 찾으려하니까 다시 확인조치해");
            return false;
        }

        playerSkillRunTimes[_index] = new PlayerSkillRunTime(skillData);
        UIManager.Instance.SkillUIManager.InitSkillUI(_index, playerSkillRunTimes[_index]);

        return true;
    }

    public bool TryUseSkill(int _index, out SkillData skillData) // 스킬사용이 가능한가? + 해당 스킬 데이터 반환
    {
        skillData = null;

        if (_index < 0 || _index >= playerSkillRunTimes.Length)
        {
            Debug.LogError($"잘못된 인덱스 접근임 말도 안되는 인덱스니까 다시 확인조치");
            return false;
        }

        PlayerSkillRunTime wantSkill = playerSkillRunTimes[_index];

        if (wantSkill == null)
        {
            Debug.LogError($"오류(null) : {_index}번에 있는 스킬은 현재 존재하지않음");
            return false; // 스킬이 없어 return
        }

        double currentTime = Time.timeAsDouble;

        if (!wantSkill.CanUseSkill(currentTime))
        {
            //-> 스킬 쿨타임이 돌지않았음
            return false;
        }

        skillData = wantSkill.skillData;

        //실제 스킬을 실행하는 함수를 적을 예정

        wantSkill.StartSkill_setEndCoolTime(currentTime);
        UIManager.Instance.SkillUIManager.UseSkillUISetting(_index);
        return true;
    }
    public void ExecuteSkillImpact(SkillData _skillData)
    {
        if (_skillData == null)
        {
            return;
        }

        if (_skillData.skillExecute == null)
        {
            Debug.LogError($"{_skillData.skillName}에 SkillExecute가 없어.");
            return;
        }

        if (PlayerStatManager.Instance == null)
        {
            Debug.LogError("PlayerStatManager가 비어있네.");
            return;
        }

        skillHitTargets.Clear();

        float attackValue = PlayerStatManager.Instance.GetStatValue(StatType.Attack);

        SkillExecuteData executeData = new SkillExecuteData(this.transform, attackValue, skillHitBuffer, skillHitTargets);

        IEnumerator executeRoutine = _skillData.skillExecute.Execute(executeData, _skillData);

        if (executeRoutine != null)
        {
            StartCoroutine(executeRoutine);
        }
    }
}
