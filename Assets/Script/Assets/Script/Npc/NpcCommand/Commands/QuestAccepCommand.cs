using UnityEngine;

public class QuestAcceptCommand : INpcCommand
{
    private int questID;

    public void Init(int _questID)
    {
        questID = _questID;
    }
    public void Execute()
    {
       TalkUI talkUI = UIManager.Instance.TalkUI;
        talkUI.UIHide();
       QuestManager.Instance.AcceptQuest(talkUI.questID);
    }
}
