using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private PlayerStatusUIManager playerStatusUI;
    public PlayerStatusUIManager StatusUI => playerStatusUI; // 받아오고 읽기전용으로 변경(식 본문 프로펄티)

    [SerializeField]
    private PlayerStateUI playerStateUI;
    public PlayerStateUI StateUI => playerStateUI; // 받아오고 읽기전용으로 변경(식 본문 프로펄티)

    [SerializeField]
    private InventoryUIManager playerInventoryUI;
    public InventoryUIManager InventoryUI => playerInventoryUI;

    [SerializeField]
    private EquipSpaceUIManager playerEquipSpaceUI;
    public EquipSpaceUIManager EquipSpaceUI => playerEquipSpaceUI;

    [SerializeField]
    private EquipToolTip equipToolTip;

    [SerializeField]
    private ConsumerToolTip consumerToolTip;

    [SerializeField]
    private TraderStoreUI traderStoreUI;

    [SerializeField]
    private TalkUI talkUI;
    public TalkUI TalkUI => talkUI;
    public TalkUI NpcTalkUI => talkUI;

    //퀘스트 관련 UI
    [SerializeField]
    private QuestUI questUI;

    [SerializeField]
    private QuestExplainUI questDescriptionUI;
    public QuestExplainUI QuestDescriptionUI => questDescriptionUI;



    [SerializeField]
    private IsTrueTraderSellButton isTrueTraderSellButton;
    public IsTrueTraderSellButton TraderSellButton => isTrueTraderSellButton;


    [SerializeField]
    private SkillUIManager sKillUIManager;

    public SkillUIManager SkillUIManager => sKillUIManager;

    private List<UIBase> OpenUIList = new List<UIBase>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


        //--테스트용 퀘스트 수락
        //QuestManager.Instance.playerQuestData.AcceptQuest(QuestManager.Instance.questDB.GetQuestData(1));
        //QuestManager.Instance.playerQuestData.AcceptQuest(QuestManager.Instance.questDB.GetQuestData(3));
    }

    public void ShowInventoryUI()
    {
        playerInventoryUI.UIOpen();
        OpenUIList.Add(playerInventoryUI);
    }

    public void HideInventoryUI()
    {
        playerInventoryUI.UIHide();
        OpenUIList.Remove(playerInventoryUI);
    }

    public void ShowEquipSpaceUI()
    {
        playerEquipSpaceUI.UIOpen();
        OpenUIList.Add(playerEquipSpaceUI);
    }

    public void HideEquipSpaceUI()
    {
        playerEquipSpaceUI.UIHide();

        OpenUIList.Remove(playerEquipSpaceUI);
    }

    public void ShowEquipToolTip(EquipmentItemInstance _item, RectTransform _slotRect)
    {
        equipToolTip.transform.SetAsLastSibling();
        equipToolTip.Show(_item, _slotRect);
    }

    public void HideEquipToolTip()
    {
        equipToolTip.Hide();
    }

    public void ShowConsumerToolTip(ConsumerItemInstance _item, RectTransform _slotRect)
    {
        consumerToolTip.transform.SetAsLastSibling();
        consumerToolTip.Show(_item, _slotRect);
    }

    public void HideConsumerToolTip()
    {
        consumerToolTip.Hide();
    }

    public void ShowPlayerStatusUI()
    {
        playerStatusUI.UIOpen();
        OpenUIList.Add(playerStatusUI);
    }

    public void HidePlayerStatusUI()
    {
        playerStatusUI.UIHide();
        OpenUIList.Remove(playerStatusUI);
    }


    public void ShowBuyButton()
    {
        isTrueTraderSellButton.UIOpen();
        OpenUIList.Add(isTrueTraderSellButton);
    }

    public void HideBuyButton()
    {
        isTrueTraderSellButton.UIHide();
        OpenUIList.Remove(isTrueTraderSellButton);
    }
    
    public void ShowWanderingShop()
    {
        traderStoreUI.UIOpen();
        OpenUIList.Add(traderStoreUI);
    }

    public void HideWanderingShop()
    {
        traderStoreUI.UIHide();
        OpenUIList.Remove(traderStoreUI);
    }

    public void ShowNpcTalkUI()
    {
        NpcTalkUI.UIOpen();
    }

    public void HideNpcTalkUI()
    {
        NpcTalkUI.UIHide();
    }

    public void ShowQuestUI()
    {
        questUI.UIOpen();
        OpenUIList.Add(questUI);
    }

    public void HideQuestUI()
    {
        questUI.UIHide();
        OpenUIList.Remove(questUI);
    }

    public void ShowQuestDescriptionUI()
    {  
        questDescriptionUI.UIOpen();
        OpenUIList.Add(questDescriptionUI);
    }

    public void HideQuestDescriptionUI()
    {         
        questDescriptionUI.UIHide();
        OpenUIList.Remove(questDescriptionUI);
    }
    public void InOrderUIHide()
    {
        if (OpenUIList.Count == 0)
        {
            return;
        }

        OpenUIList[^1].UIHide();
        OpenUIList.RemoveAt(OpenUIList.Count - 1);
    }

    public bool IsOpenUI<T>() where T : UIBase
    {
        foreach(UIBase uibase in OpenUIList)
        {
            if(uibase is T)
            {
                return true;
            }
        }
        return false;
    }

}
