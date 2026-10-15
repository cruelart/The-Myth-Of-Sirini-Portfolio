using TMPro;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalkUI : UIBase
{
    public event Action OnHide;// 대화창이 닫힐때 이벤트

    private NpcTalkData npcTalkData; // 어떤 타입의 npc 대화창을 띄울건지 데이터 받아오기
    public int questID { get; private set; } = 0; // 

    [SerializeField]
    private TextMeshProUGUI npcName;

    [SerializeField]
    private NpcTalkText npcTalkText;

    [SerializeField]
    private NpcTalkButtonManager npcTalkButtonPanel;

    private List<NpcTextData> npcTexts; // 보여줄 대화 내용

    private int currentIndex; // 현재 실행중인 인덱스번호

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(NpcTalkData _npcTalkData)
    {
        currentIndex = 0;

        npcTalkData = _npcTalkData;
        npcTexts = npcTalkData.npcTexts; // 참조
        questID = _npcTalkData.questID;

        npcName.text = npcTexts[currentIndex].npcName; // 수정예정 -> 따로 이름 변경하는 스크립트 짤까 고민중

        npcTalkText.ShowNpcTalkText(npcTexts, currentIndex);
        npcTalkButtonPanel.ShowButton(npcTexts[currentIndex].buttonTypes);
    }

    public void NextText()
    {
        npcTalkText.ShowNpcTalkText(npcTexts, ++currentIndex);
        npcName.text = npcTexts[currentIndex].npcName;
        npcTalkButtonPanel.ShowButton(npcTexts[currentIndex].buttonTypes);
        Debug.Log("다음");
    }

    public void OnClickButton(TalkButtonType _type)
    {
        switch (_type)
        {
            case TalkButtonType.OpenShop:
                break;
        }
    }

    public override void UIHide()
    {
        base.UIHide();
        OnHide?.Invoke();
    }

}
