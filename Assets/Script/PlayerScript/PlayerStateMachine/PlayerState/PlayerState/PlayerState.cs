using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    public event Action<PlayerRequestType> OnEndAnimationEvent; // -> 리펙토링 후 제거 예정

    protected bool IsPossibleChangeState = true; // 상태 교체가 가능한 상태를 나타내는 변수

    //public readonly List<PlayerRequestType> possibleChangeStateList; // 전환할 수 있는 요청 목록

    protected HashSet<PlayerRequestType> possibleRequestTypes = new();

    protected PlayerAnimationEvents animationEvents;

    protected Animator animator;

    protected PlayerStateAction playerStateAction;

    protected bool isSuperMode = false;

    public PlayerState(PlayerAnimationEvents _animationEvents, Animator animator, PlayerStateAction playerStateAction)
    {
        this.animationEvents = _animationEvents;
        this.animator = animator;
        this.playerStateAction = playerStateAction;

        SetPossibleChangeState();
    }

    public abstract void Enter(); // 해당 상태에 들어왔을 때 필요한 것들 들여오기
    public abstract void Tick(); // 실제 실행할 것들
    public abstract void FixedTick(); // 물리연산처리 할 것들
    public abstract void Exit(); // 빠져나갈때 처리할 것들 적기

    protected virtual void PossibleChangeState()
    {
        IsPossibleChangeState = true;
    }

    protected virtual void ImPossibleChangeState()
    {
        IsPossibleChangeState = false;
    }

    public bool GetIsPossibleChangeState()
    {
        return IsPossibleChangeState;
    }

    public bool GetPossibleRequestType(PlayerRequestType _type)
    {
        return possibleRequestTypes.Contains(_type);
    }

    public bool GetIsSuperMode()
    {
        return isSuperMode;
    }
    public abstract void AnimationAction();

    protected abstract void SetPossibleChangeState();

    protected virtual void EndAnimation()
    {
        OnEndAnimationEvent?.Invoke(PlayerRequestType.None);
    }

    public virtual bool TryPrepareRequestAndExeCute(BufferedPlayerRequest _request) // 해당 상태로 전환이 가능한지 여부를 따짐
    {
        return true;
    }

    public virtual bool TryHandleSameStateRequest(PlayerRequestType _type)
    {
        return false;
    }
}
