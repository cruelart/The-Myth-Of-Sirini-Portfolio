using System.Collections.Generic;
using UnityEngine;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField]
    private SkillUI[] skillUIs = new SkillUI[8]; // 8개 스킬 사용제한

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSkillUI(int _index, PlayerSkillRunTime _playerSkillRuntime)
    {
        skillUIs[_index].Init(_playerSkillRuntime);
    }

    public void UseSkillUISetting(int _index)
    {
        skillUIs[_index].UseSkillUISetting();
    }
}
