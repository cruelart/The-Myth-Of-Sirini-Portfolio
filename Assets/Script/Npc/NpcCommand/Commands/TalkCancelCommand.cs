using UnityEngine;

public class TalkCancelCommand : INpcCommand
{
    public void Execute()
    {
        UIManager.Instance.TalkUI.UIHide();
    }
}
