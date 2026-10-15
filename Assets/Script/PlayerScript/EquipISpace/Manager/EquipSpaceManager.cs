using UnityEngine;

public class EquipSpaceManager : MonoBehaviour
{
    public static EquipSpaceManager Instance;

    private EquipSpace equipSpace;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void Init(EquipSpace _equipSpace)
    {
        equipSpace = _equipSpace;
    }

    public void EquipItem(EquipmentItemInstance _item)
    {
       equipSpace.EquipItem(_item);
    }

    public void TakeOffItem(EquipmentItemInstance _item)
    {
        equipSpace.TakeOffItem(_item);
    }
}
