using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackManager : MonoBehaviour
{

    private PlayerStatManager playerStatManager;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //1. PlayerAttackManager 에서 공격을 하라고 명령이 떨어지면 
    public void StartAttack(PlayerNormalHitBox _playerHitBox)
    {
        int damage = (int)playerStatManager.GetStatValue(StatType.Attack);

        _playerHitBox.StartAttack(damage);
    }
    public void StartAttack(PlayerNormalHitBox _playerHitBox, float _damageMultiplier)
    {
        int damage = Mathf.RoundToInt(playerStatManager.GetStatValue(StatType.Attack) * _damageMultiplier);

        _playerHitBox.SwordStartAttack(damage);
    }

    public void EndAttack(PlayerNormalHitBox _playerHitBox)
    {
        _playerHitBox.EndAttack();
    }

    public void Init(PlayerStatManager _playerstatManager)
    {
        playerStatManager = _playerstatManager;
    }
}
