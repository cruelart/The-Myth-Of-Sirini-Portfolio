using UnityEngine;

public class NpcTalkButtonObj : MonoBehaviour
{
    private INpcCommand npcCommand;
  
    public void Init(INpcCommand _npcCommand)
    {
        npcCommand = _npcCommand;
    }

    public void OnClick()
    {
        npcCommand.Execute();
    }
}
