using System;
using UnityEngine;

public static class InventoryEvent
{
    public static event Action<int, int> OnOwnedItemCountChanged;

    public static void RaiseOwnedItemCountChanged(int _itemID, int _amount)
    {
        OnOwnedItemCountChanged?.Invoke(_itemID, _amount);
    }
}
