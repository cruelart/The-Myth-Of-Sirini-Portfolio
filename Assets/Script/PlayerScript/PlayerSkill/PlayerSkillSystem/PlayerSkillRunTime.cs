using UnityEngine;

public class PlayerSkillRunTime
{
    public SkillData skillData { get; private set; } // 해당 스킬의 데이터

    public double coolEndTime; // 스킬을 재사용할 수 있는 시점

    public PlayerSkillRunTime(SkillData _data)
    {
        skillData = _data;
    }

    public void StartSkill_setEndCoolTime(double _currentTime) // 스킬시작할때 호출 ㄱ
    {
        coolEndTime = _currentTime + skillData.coolTime;
    }

    public bool CanUseSkill(double _currentTime)
    {
        return _currentTime >= coolEndTime;
    }

}
