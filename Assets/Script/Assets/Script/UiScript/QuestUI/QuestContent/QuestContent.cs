using TMPro;
using UnityEngine;

public class QuestContent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questContentText; // 퀘스트 내용 텍스트

    //Quest Description UI에 넘길 데이터들
    private QuestData questData;
    private QuestProgressData questProgressData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(QuestData _questData, QuestProgressData _questProgressData )
    {
        questData = _questData;
        questProgressData = _questProgressData;
        questContentText.text = _questData.questName;
    }

    public void UpdateQuestContent(QuestProgressData _questProgressData)
    {
        questProgressData = _questProgressData;
        UIManager.Instance.QuestDescriptionUI.UpdateExplainInProgressTextUI(questData, questProgressData);
    }

    public void OnClick()
    {
        UIManager.Instance.ShowQuestDescriptionUI();
        UIManager.Instance.QuestDescriptionUI.Init(questData, questProgressData);
    }
}
