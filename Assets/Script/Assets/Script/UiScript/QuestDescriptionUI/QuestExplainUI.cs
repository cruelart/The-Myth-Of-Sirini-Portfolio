using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestExplainUI : UIBase
{
    private int questID = -1;

    [Header("퀘스트 이름")]
    [SerializeField]
    private TextMeshProUGUI questNameText;

    [Header("퀘스트 제공 Npc")]
    [SerializeField]
    private Image StartNpcImage;

    [SerializeField]
    private TextMeshProUGUI StartNpcNameText;

    [SerializeField]
    private TextMeshProUGUI StartNpcTransformText;

    [Header("퀘스트 완료 Npc")]
    [SerializeField]
    private Image EndNpcImage;

    [SerializeField]
    private TextMeshProUGUI EndNpcNameText;

    [SerializeField]
    private TextMeshProUGUI EndNpcTransformText;

    [Header("퀘스트 설명란")]
    [SerializeField]
    private TextMeshProUGUI questDescriptionText;

    [Header("퀘스트 진행 현황")]

    [SerializeField]
    private List<TextMeshProUGUI> InProgressTextObj;

    [Header("보상")]
    [SerializeField]
    private TextMeshProUGUI rewardMoneyText;

    [SerializeField]
    private TextMeshProUGUI rewardExpText;

    [SerializeField]
    private TextMeshProUGUI rewardItemNameText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(QuestData _questData, QuestProgressData _questProgressData)
    {
        questID = _questData.questID;
        questNameText.text = _questData.questName;

        StartNpcImage.sprite = _questData.StartNpcImage;
        StartNpcNameText.text = $"{_questData.StartNpcName}";
        StartNpcTransformText.text = $"{_questData.StartNpcTransform}";

        EndNpcImage.sprite = _questData.EndNpcImage;
        EndNpcNameText.text = $"{_questData.EndNpcName}";
        EndNpcTransformText.text = $"{_questData.EndNpcTransform}";

        questDescriptionText.text = _questData.questDescription;

        DecideInProgressText(_questData, _questProgressData);

        rewardMoneyText.text = $"루비: {_questData.ClearGold}";
        rewardExpText.text = $"경험치: {_questData.ClearExp}";
        rewardItemNameText.text = _questData.ClearItem != null ? $"아이템: {_questData.ClearItem.itemName}" : "없음";
    }

    public void UpdateExplainInProgressTextUI(QuestData _questData, QuestProgressData _questProgressData)
    {
        if(questID != _questData.questID)
        {
            return; // textUI 수정안함 -> 다른 애를 왜 수정해야되는거지 바로 return 처리
        }
        DecideInProgressText(_questData, _questProgressData);
    }

    private void DecideInProgressText(QuestData _questData, QuestProgressData _questProgressData)
    {
        OnDisableInProgressTextListUI(); // 전부다 비활성화 먼저 시키고

        switch (_questProgressData.questState)
        {
            case QuestState.Available:
                for (int i = 0; i < _questProgressData.requirementProgresses.Count; i++)
                {
                    if (i >= InProgressTextObj.Count)
                    {
                        return;
                    }
                    InProgressTextObj[i].text = $"{_questData.requirements[i].questText} : {_questProgressData.requirementProgresses[i].currentCount}/{_questData.requirements[i].requiredCount}";
                    InProgressTextObj[i].gameObject.SetActive(true);
                }
                break;
            case QuestState.InProgress:
                for(int i = 0; i < _questProgressData.requirementProgresses.Count; i++)
                {
                    if(i > InProgressTextObj.Count)
                    {
                        return;
                    }
                    InProgressTextObj[i].text = $"{_questData.requirements[i].questText} : {_questProgressData.requirementProgresses[i].currentCount}/{_questData.requirements[i].requiredCount}";
                    InProgressTextObj[i].gameObject.SetActive(true);
                }
                //questInProgressText.text = $"진행 현황: {_questProgressData.currentCount}/{_questData.goalCount}";
                break;
            case QuestState.Completed:
                break;
        }
    }

    private void OnDisableInProgressTextListUI()
    {
        for (int i = 0; i < InProgressTextObj.Count; i++)
        {
            InProgressTextObj[i].gameObject.SetActive(false);
        }
    }
}
