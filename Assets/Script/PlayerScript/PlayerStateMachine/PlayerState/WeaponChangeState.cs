using UnityEngine;

public class WeaponChangeState : PlayerState
{
    private enum ChangeAction
    {
        None,
        Draw,
        Sheat,
    }

    private readonly PlayerWeaponController weaponController;

    private ActiveWeaponType targetWeaponType = ActiveWeaponType.None;

    private ChangeAction currentAction = ChangeAction.None;
    private string currentAnimationStateName;
    private bool changeCompleted;
    public WeaponChangeState(PlayerAnimationEvents _animationEvents, Animator _animator, PlayerStateAction _playerStateAction, PlayerWeaponController _weaponController)
       : base(_animationEvents, _animator, _playerStateAction)
    {
        this.weaponController = _weaponController;
    }

    public override void Enter()
    {
        IsPossibleChangeState = false;
        changeCompleted = false;

        animationEvents.OnEndAnimation += HandleAnimationEnd;

        playerStateAction.StopMoveAction();

        AnimationAction();
    }

    public override void Tick()
    {
    }
    public override void FixedTick()
    {
    }

    public override void Exit()
    {
        animationEvents.OnEndAnimation -= HandleAnimationEnd;

        targetWeaponType = ActiveWeaponType.None;
        currentAction = ChangeAction.None;
        currentAnimationStateName = null;
        changeCompleted = false;

        IsPossibleChangeState = false;
    }

    public override void AnimationAction()
    {
        bool isDrawAnimation = currentAction == ChangeAction.Draw;

        currentAnimationStateName = GetAnimationStateName(targetWeaponType, isDrawAnimation);

        if (string.IsNullOrEmpty(currentAnimationStateName))
        {
            Debug.LogError("해당 무기의 전환 애니메이션이 없습니다.");

            return;
        }

        animator.CrossFadeInFixedTime(currentAnimationStateName, 0.1f, 0, 0f);
    }

    protected override void SetPossibleChangeState()
    {
        // 무기 전환 애니메이션 도중에는 다른 상태로 전환하지 않음
    }

    public override bool TryPrepareRequestAndExeCute(BufferedPlayerRequest request)
    {
        if (request.RequestType != PlayerRequestType.WeaponToggle)
        {
            return false;
        }

        if (!TryGetTargetWeaponType(request.RequestTypeIndex, out ActiveWeaponType requestedWeaponType)) // 말도 안되는 인덱스를 받아오면 false
        {
            return false;
        }

        if (!weaponController.IsWeaponEquipped(requestedWeaponType))
        {
            // 해당 슬롯에 무기가 없으면 입력 무시
            return false;
        }

        ActiveWeaponType activeWeaponType = weaponController.CurrentActiveSlot;

        // 다른 종류의 무기를 이미 꺼낸 상태면 입력 무시
        if (activeWeaponType != ActiveWeaponType.None && activeWeaponType != requestedWeaponType)
        {
            return false;
        }

        targetWeaponType = requestedWeaponType;

        // 같은 버튼을 다시 누르면 집어넣기
        currentAction = (activeWeaponType == requestedWeaponType)? ChangeAction.Sheat : ChangeAction.Draw;

        return HasChangeAnimation(targetWeaponType);
    }
    private bool TryGetTargetWeaponType(int requestIndex, out ActiveWeaponType weaponType)
    {
        switch (requestIndex)
        {
            case 0:
                weaponType = ActiveWeaponType.MainWeapon;
                return true;

            case 1:
                weaponType = ActiveWeaponType.SecondaryWeapon;
                return true;

            default:
                weaponType = ActiveWeaponType.None;
                return false;
        }
    }
    private bool HasChangeAnimation(ActiveWeaponType _weaponType)
    {
        WeaponStyle style = weaponController.GetEquippedWeaponStyle(_weaponType);

        return style == WeaponStyle.Sword || style == WeaponStyle.Gun; // 검이나 총은 애니메이션 보유중 나머지는 false
    }
    private string GetAnimationStateName(ActiveWeaponType _weaponType, bool _isDrawAnimation)
    {
        WeaponStyle style = weaponController.GetEquippedWeaponStyle(_weaponType);

        switch (style)
        {
            case WeaponStyle.Sword:
                return _isDrawAnimation ? "DrawSword" : "SheatingSword";

            case WeaponStyle.Gun:
                return _isDrawAnimation ? "DrawGun" : "SheathingGun";

            default:
                return null;
        }
    }

    private void HandleAnimationEnd()
    {
        CompleteWeaponChange();
    }

    private void CompleteWeaponChange()
    {
        if (changeCompleted)
        {
            return;
        }

        changeCompleted = true;

        switch (currentAction)
        {
            case ChangeAction.Draw:
                weaponController.CompleteDrawWeapon(ActiveWeaponType.MainWeapon);
                break;

            case ChangeAction.Sheat:
                weaponController.CompleteSheatheWeapon();
                break;
        }

        EndAnimation();
    }
}
