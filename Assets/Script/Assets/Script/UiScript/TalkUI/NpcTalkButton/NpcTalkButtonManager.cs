using System.Collections.Generic;
using UnityEngine;

public enum TalkButtonType
{
    OpenShop,
    Cancel,
    Next,
    AcceptQuest,
    CompleteQuest
}

[System.Serializable]
public class TalkButton
{
    public TalkButtonType buttonType;
    public GameObject button_object;
}
public class NpcTalkButtonManager : MonoBehaviour
{

    [SerializeField]
    private List<TalkButton> buttonPrefabs; // npc대화에 필요한 모든 버튼들

    private Dictionary<TalkButtonType, GameObject> realButtonMap = new(); // 실제 생성된 버튼들을 저장하는 해시테이블 

    private void Awake()
    {
        CreateButtons();
        //ShowButton(TalkButtonType.OpenShop);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateButtons()
    {
        foreach (var button in buttonPrefabs)
        {
            GameObject buttonObj = Instantiate(button.button_object, this.transform);

            INpcCommand npcCommand = DecideCommandType(button.buttonType);
            NpcTalkButtonObj talkbutton = buttonObj.GetComponent<NpcTalkButtonObj>();
            talkbutton.Init(npcCommand);

            buttonObj.SetActive(false);
            realButtonMap.Add(button.buttonType, buttonObj);
        }
    }

    public void ShowButton(List<TalkButtonType> _buttonTypes)
    {
        HideAllButton();

        foreach (var buttonType in _buttonTypes)
        {
            realButtonMap[buttonType].SetActive(true);
        }
    }

    public void HideAllButton() // 어처피 싹다 안보이게 한 다음 필요한거만 ShowButton할거니까
    {
        foreach(var buttonObj in realButtonMap.Values)
        {
            buttonObj.SetActive(false); 
        }
    }

    private INpcCommand DecideCommandType(TalkButtonType _buttonType)
    {
        switch(_buttonType)
        {
            case TalkButtonType.Next:
                return new NextTalkCommand();

            case TalkButtonType.OpenShop:
                return new OpenWanderingShopCommand();

            case TalkButtonType.Cancel:
                return new TalkCancelCommand();
            case TalkButtonType.AcceptQuest:
                return new QuestAcceptCommand();
            case TalkButtonType.CompleteQuest:
                return new QuestComplete();
        }

        return null;
    }

}
