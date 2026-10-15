using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    private PlayerSkillRunTime runTimeSkillData;

    [SerializeField]
    private Image skillImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(PlayerSkillRunTime _skillData)
    {
        runTimeSkillData = _skillData;

        skillImage.sprite = runTimeSkillData.skillData.skillIcon;
    }

    public void UseSkillUISetting()
    {
        StartCoroutine(UpdateSkillCoolTimeUI());
    }

    IEnumerator UpdateSkillCoolTimeUI()
    {
        double totalCoolTime = runTimeSkillData.skillData.coolTime;

        while (true)
        {
            double currentTime = Time.timeAsDouble;

            if (runTimeSkillData.CanUseSkill(currentTime))
            {
                skillImage.fillAmount = 1f;
                break;
            }

            double remainingCoolTime = runTimeSkillData.coolEndTime - currentTime;

            double elapsedRatio = 1d - remainingCoolTime / totalCoolTime;

            skillImage.fillAmount = Mathf.Clamp01((float)elapsedRatio);

            yield return null;
        }
    }
}
