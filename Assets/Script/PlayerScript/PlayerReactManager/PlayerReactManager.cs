using System;
using UnityEngine;

public class PlayerReactManager : MonoBehaviour, IF_OnDamaged
{
    bool isPlayerDead = false;

    private bool isSuperMode = false;

    public event Action<PlayerPriorityStateType> OnPriorityStateRequested;
    private PlayerStatManager playerStatManager;
    private Color damageColor = new Color(1,0,0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isSuperMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(PlayerStatManager _playerStatManager)
    {
        playerStatManager = _playerStatManager;
    }

    public void OnDamaged(int _damage, GameObject _attacker, DamageType _damageType, Vector3 _hitPoint, Vector3 _hitDir)
    {
        if(isSuperMode)
        {
            return;
        }

        if (playerStatManager != null)
        {
            playerStatManager.OnDamaged(_damage);

            if (playerStatManager.playerStatDB.currentHp <= 0)
            {
                if (!isPlayerDead)
                {
                    OnPriorityStateRequested?.Invoke(PlayerPriorityStateType.Death);
                    isPlayerDead = true;
                }
                return;
            }
            DamageTextManager.Instance.ShowDamageText(this.transform, _damage, damageColor);


            if (_damageType == DamageType.Normal)
            {
                OnPriorityStateRequested?.Invoke(PlayerPriorityStateType.NormalReact);
            }
            else if (_damageType == DamageType.Strong)
            {
                OnPriorityStateRequested?.Invoke(PlayerPriorityStateType.KnockDown); // 테스트용 녹다운 -> 나중에 일반 피격과 녹다운 조건 구분해서 리펙토링
            }
        }
    }

    public void OnSuperMode()
    {
        isSuperMode = true;
    }

    public void OffSuperMode()
    {
        isSuperMode = false;
    }
}
