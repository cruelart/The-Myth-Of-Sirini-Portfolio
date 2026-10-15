using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDatabase", menuName = "Scriptable Objects/SkillDatabase")]
public class SkillDatabase : ScriptableObject
{
    [SerializeField]
    private List<SkillData> skillDatas;

    private Dictionary<int, SkillData> skillDataMap;

    public void Init()
    {
        skillDataMap = new Dictionary<int, SkillData>();

        foreach(SkillData skillData in skillDatas)
        {
            if(skillDataMap.ContainsKey(skillData.skillID)) // ToDictionary를 하게되면 중복 아이디 확인이 안됨 따라서 이렇게 처리해보고자 설정 
            {
                Debug.LogError($"중복된 Skill Id가 감지 빠르게 고치도록 중복된 id는 {skillData.skillID}임");
                continue;
            }

            skillDataMap.Add(skillData.skillID, skillData);
        }
    }

    public bool GetSkillData(int _id, out SkillData _skillData)
    {
        return skillDataMap.TryGetValue(_id, out _skillData);
    }
}
