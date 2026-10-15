using System;
using UnityEngine;

public class PlayerGoldManager : MonoBehaviour
{
    public static PlayerGoldManager Instance;
    public Action<long> OnChangeGold;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlusGold(long _value)
    {
        PlayerDBManager.instance.playerDB.gold += _value;
        OnChangeGold?.Invoke(PlayerDBManager.instance.playerDB.gold);
    }

    public bool MinusGold(long _value)
    {
        long currentGold = PlayerDBManager.instance.playerDB.gold;

        currentGold -= _value;

        if (currentGold >= 0)
        {
            PlayerDBManager.instance.playerDB.gold = currentGold;
            OnChangeGold?.Invoke(PlayerDBManager.instance.playerDB.gold);
            return true;
        }

        return false; // 음수가 돼버리니까 안된다고 알려
    }

    public long GetCurrentGoldValue()
    {
        return PlayerDBManager.instance.playerDB.gold;
    }
}
