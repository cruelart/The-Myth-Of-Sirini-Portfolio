using UnityEngine;

public class QuestComplete : INpcCommand
{
    private int questID;

    public void Init(int _questID)
    {
        this.questID = _questID;
    }
    public void Execute()
    {
        TalkUI talkUI = UIManager.Instance.TalkUI;
        talkUI.UIHide();
        QuestManager.Instance.CompleteQuest(talkUI.questID);
    }
}
