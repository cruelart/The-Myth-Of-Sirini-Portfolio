using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestInProgressUI : MonoBehaviour
{
    [Header("퀘스트 타입 텍스트 수정")]
    [SerializeField]
    private TextMeshProUGUI inProgressText; // 진행중 텍스트 찐하게 만들예정

    [SerializeField]
    private Color onEnableColor = new Color32(255,255,255, 255); // 하얀색

    [SerializeField]
    private Color onDisableColor = new Color32(147,147,147,255); // 회색

    [SerializeField]
    private GameObject inProgress_prefab; // 진행중 프리팹

    //private List<QuestContent> inProgressQuestList = new List<QuestContent>(); // 진행중 퀘스트 리스트 -> 해시가 나을거같아서 지움
    private Dictionary<int, QuestContent> inProgressQuestMap = new Dictionary<int, QuestContent>(); // 해시로 변경

    [SerializeField]
    private Transform questListTransform; // 퀘스트 리스트ui의 위치(실제 보일 장소) -> content 받아오면 될듯

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        inProgressText.color = onEnableColor;
    }

    private void OnDisable()
    {
        inProgressText.color = onDisableColor;
    }

    public void UpdateInProgressQuest(QuestData _questData, QuestProgressData _questProgressData)
    {
        QuestContent questContent = inProgressQuestMap[_questData.questID];
        questContent.UpdateQuestContent(_questProgressData);
    }
    public void AddInProgressQuest(QuestData _questData, QuestProgressData _questProgressData)
    {
        QuestContent inProgressQuestContent = Instantiate(inProgress_prefab, questListTransform).GetComponent<QuestContent>();
        inProgressQuestContent.Init(_questData, _questProgressData);

        inProgressQuestMap.Add(_questData.questID, inProgressQuestContent);
    }

    public void RemoveInProgressQuest(int _questID)
    {
        if (inProgressQuestMap.TryGetValue(_questID, out QuestContent questContent)) // 실제로 진행중인 퀘스트였다면
        {
            inProgressQuestMap.Remove(_questID);
            Destroy(questContent.gameObject);
        }
    }
}
