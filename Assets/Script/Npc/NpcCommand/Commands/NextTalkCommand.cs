using UnityEngine;

public class NextTalkCommand : INpcCommand
{
    public void Execute()
    {
        UIManager.Instance.TalkUI.NextText();
    }
}
