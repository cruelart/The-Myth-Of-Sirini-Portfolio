using UnityEngine;

public class PlayerAttackAnimationEvents : MonoBehaviour
{
    private PlayerAttackManager playerAttackManager;
    //맨손 격투용-------------------------------------------
    [SerializeField]
    private PlayerNormalHitBox leftHand_hitBox;

    [SerializeField]
    private PlayerNormalHitBox rightHand_hitBox;

    [SerializeField]
    private PlayerNormalHitBox leftLeg_hitBox;

    [SerializeField]
    private PlayerNormalHitBox rightLeg_hitBox;
    //-----------------------------------------------------

    //-------검
    [SerializeField]
    private PlayerNormalHitBox sword_hitbox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(PlayerAttackManager _playerAttackManager)
    {
        playerAttackManager = _playerAttackManager;
    }
    //---------------------------맨손 공격------------------------------------
    //왼손 공격
    public void OnLeftHandAttackStart()
    {
        if(leftHand_hitBox != null)
        {
            playerAttackManager.StartAttack(leftHand_hitBox);
        }
    }

    public void OnLeftHandAttackEnd()
    {
        if(leftHand_hitBox != null)
        {
            playerAttackManager.EndAttack(leftHand_hitBox);
        }
    }

    //오른손 공격
    public void OnRightHandAttackStart()
    {
        if (rightHand_hitBox != null)
        {
            playerAttackManager.StartAttack(rightHand_hitBox);
        }
    }

    public void OnRightHandAttackEnd()
    {
        if (rightHand_hitBox != null)
        {
            playerAttackManager.EndAttack(rightHand_hitBox);
        }
    }

    //왼쪽 발 공격
    public void OnLeftLegAttackStart()
    {
        if(leftLeg_hitBox != null)
        {
            playerAttackManager.StartAttack(leftLeg_hitBox);
        }
    }

    public void OnLeftLegAttackEnd()
    {
        if (leftLeg_hitBox != null)
        {
            playerAttackManager.EndAttack(leftLeg_hitBox);
        }
    }

    //오른 발 공격
    public void OnRightLegAttackStart()
    {
        if (rightLeg_hitBox != null)
        {
            playerAttackManager.StartAttack(rightLeg_hitBox);
        }
    }

    public void OnRightLegAttackEnd()
    {
        if (rightLeg_hitBox != null)
        {
            playerAttackManager.EndAttack(rightLeg_hitBox);
        }
    }
    //------------------------------------------------------------------
    public void OnSwordCombo1Start()
    {
        if(sword_hitbox != null)
        {
            playerAttackManager.StartAttack(sword_hitbox, 1.1f);
        }
    }
    public void OnSwordCombo2Start()
    {
        if (sword_hitbox != null)
        {
            playerAttackManager.StartAttack(sword_hitbox, 1.3f);
        }
    }

    public void OnSwordCombo3Start()
    {
        if (sword_hitbox != null)
        {
            playerAttackManager.StartAttack(sword_hitbox, 1.4f);
        }
    }

    public void OnSwordCombo4Start()
    {
        if (sword_hitbox != null)
        {
            playerAttackManager.StartAttack(sword_hitbox, 1.5f);
        }
    }

    public void OnSwordCombo5Start()
    {
        if (sword_hitbox != null)
        {
            playerAttackManager.StartAttack(sword_hitbox, 1.4f);
        }
    }

    public void OnSwordCombo6Start()
    {
        if (sword_hitbox != null)
        {
            playerAttackManager.StartAttack(sword_hitbox, 2.0f);
        }
    }
    public void OnSwordComboEnd()
    {
        if (sword_hitbox != null)
        {
            playerAttackManager.EndAttack(sword_hitbox);
        }
    }
}
