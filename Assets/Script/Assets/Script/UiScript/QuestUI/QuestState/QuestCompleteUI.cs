using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestCompleteUI : MonoBehaviour
{
    [Header("퀘스트 타입 텍스트 수정")]
    [SerializeField]
    private TextMeshProUGUI completeText; // 진행중 텍스트 찐하게 만들예정

    [SerializeField]
    private Color onEnableColor = new Color32(255, 255, 255, 255); // 하얀색

    [SerializeField]
    private Color onDisableColor = new Color32(147, 147, 147, 255); // 회색

    [SerializeField]
    private GameObject complete_prefab; // 진행중 프리팹

    //private List<QuestContent> completeQuestList = new List<QuestContent>(); // 진행중 퀘스트 리스트 -> 마찬가지로 해시로 변경
    private Dictionary<int, QuestContent> completeQuestMap = new Dictionary<int, QuestContent>(); // 해시로 변경


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
        completeText.color = onEnableColor;
    }

    private void OnDisable()
    {
        completeText.color = onDisableColor;
    }

    public void AddCompleteQuest(QuestData _questData, QuestProgressData _questProgressData)
    {
        QuestContent inCompleteQuestContent = Instantiate(complete_prefab, questListTransform).GetComponent<QuestContent>();
        inCompleteQuestContent.Init(_questData, _questProgressData);

        completeQuestMap.Add(_questData.questID, inCompleteQuestContent);
    }

    //사실상 사용하지 않는 함수일거 같긴함
    public void RemoveCompleteQuest(int _questID)
    {
        if (completeQuestMap.TryGetValue(_questID, out QuestContent questContent)) // 실제로  완료한 퀘스트였다면
        {
            completeQuestMap.Remove(_questID);
            Destroy(questContent.gameObject); // 해당 완료 퀘스트 ui 삭제
        }
    }
}
