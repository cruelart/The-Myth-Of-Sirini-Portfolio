using UnityEngine;

public class OpenWanderingShopCommand : INpcCommand
{
    public void Execute()
    {
        UIManager.Instance.ShowWanderingShop();
    }
}
