using UnityEngine;

public class SkillState : PlayerState
{
    private readonly PlayerSkillSystem playerSkillSystem;

    private SkillData currentSkillData;

    public SkillState(PlayerAnimationEvents _animationEvents, Animator _animator, PlayerStateAction _playerStateAction, PlayerSkillSystem _playerSkillSystem)
        : base(_animationEvents, _animator, _playerStateAction)
    {
        playerSkillSystem = _playerSkillSystem;
    }

    public override void Enter()
    {
        IsPossibleChangeState = false;

        animationEvents.OnPossibleChangeState += PossibleChangeState;
        animationEvents.OnImPossibleChangeState += ImPossibleChangeState;
        animationEvents.OnEndAnimation += EndAnimation;
        animationEvents.OnEffectViewAnimation += HandleSkillImpact;

        playerStateAction.StopMoveAction();
        playerStateAction.SetRootMotion(currentSkillData.useAnimationRootMotion);

        AnimationAction();
    }

    public override void Tick()
    {
        //Action();
    }

    public override void FixedTick()
    {

    }

    public override void Exit()
    {
        playerStateAction.SetRootMotion(false);

        animationEvents.OnPossibleChangeState -= PossibleChangeState;
        animationEvents.OnImPossibleChangeState -= ImPossibleChangeState;
        animationEvents.OnEndAnimation -= EndAnimation;
        animationEvents.OnEffectViewAnimation -= HandleSkillImpact;

        currentSkillData = null;
        IsPossibleChangeState = false;
    }

    public override void AnimationAction() // 리펙토링으로 제거 예정 실제 행동에서 처리할 것
    {
        if (currentSkillData == null)
        {
            Debug.LogError("실행할 SkillData 없음 확인 요망");
            return;
        }

        animator.CrossFadeInFixedTime(currentSkillData.animationName, 0.2f, 0, 0f);
    }

    protected override void SetPossibleChangeState()
    {
        //-> 변경가능 상태 없게 처리
    }

    public override bool TryPrepareRequestAndExeCute(BufferedPlayerRequest _request)
    {
        if (_request.RequestType != PlayerRequestType.Skill)
        {
            return false;
        }

        if (playerSkillSystem == null)
        {
            Debug.LogError("PlayerSkillSystem이 없음.");
            return false;
        }

        int slotIndex = _request.RequestTypeIndex;

        return playerSkillSystem.TryUseSkill(slotIndex, out currentSkillData);
    }

    private void HandleSkillImpact()
    {
        if (currentSkillData == null)
        {
            return;
        }

        playerSkillSystem.ExecuteSkillImpact(currentSkillData);
    }
}
