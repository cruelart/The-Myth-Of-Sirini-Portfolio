using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestAvailableUI : MonoBehaviour
{
    [Header("퀘스트 타입 텍스트 수정")]
    [SerializeField]
    private TextMeshProUGUI availableText; // 진행중 텍스트 찐하게 만들예정

    [SerializeField]
    private Color onEnableColor;

    [SerializeField]
    private Color onDisableColor;

    [SerializeField]
    private GameObject available_prefab; // 진행중 프리팹

    //private List<QuestContent> availableQuestList = new List<QuestContent>(); // 진행중 퀘스트 리스트
    private Dictionary<int,QuestContent> availableQuestMap = new Dictionary<int,QuestContent>(); // 해시로 변경

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
        availableText.color = onEnableColor;
    }

    private void OnDisable()
    {
        availableText.color = onDisableColor;
    }

    public void AddAvailableQuest(QuestData _questData, QuestProgressData _questProgressData)
    {
        QuestContent availableQuestContent = Instantiate(available_prefab, questListTransform).GetComponent<QuestContent>();
        availableQuestContent.Init(_questData, _questProgressData);

        availableQuestMap.Add(_questData.questID, availableQuestContent);
    }

    public void RemoveAvailableQuest(int _questID)
    {
        if (availableQuestMap.TryGetValue(_questID, out QuestContent questContent)) // 실제로 시작가능인 퀘스트였다면
        {
            availableQuestMap.Remove(_questID);
            Destroy(questContent.gameObject);
        }
    }
}
