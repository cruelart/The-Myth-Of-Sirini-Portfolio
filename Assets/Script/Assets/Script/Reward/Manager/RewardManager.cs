using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MonsterEvent.OnMonsterDead += GetExpReward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectMonsterDie()
    {

    }

    public void GetReward(MonsterDeadInfo _monsterDeadInfo)
    {

    }

    //몬스터가 지급하는 경험치
    public void GetExpReward(MonsterDeadInfo _monsterDeadInfo)
    {
        MonsterDB monsterDB = MonsterDBManager.Instance.GetMonsterDB(_monsterDeadInfo.monsterID);

        if (PlayerStatManager.Instance != null)
        {
            PlayerStatManager.Instance.GetExp(monsterDB.exp_value);
        }
    }

    public void GetExpReward(long _expValue)
    {
        PlayerStatManager.Instance.GetExp(_expValue);
    }

    public void GetItemReward(int _itemID, int _itemCount)
    {
        InventoryManager.Instance.AddItemInInventory(_itemID, _itemCount);
    }

    public void GetMoneyReward(long _moneyValue)
    {
        PlayerGoldManager.Instance.PlusGold(_moneyValue);
    }
}
