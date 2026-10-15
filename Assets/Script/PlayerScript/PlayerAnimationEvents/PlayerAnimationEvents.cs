using System;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public event Action OnPossibleChangeState;
    public event Action OnImPossibleChangeState;
    public event Action OnEndAnimation;
    public event Action OnEffectViewAnimation;
    public event Action OnEffectHideAnimation;

    public event Action OnUnArmed;
    public event Action OnEquipSword;

    public void AnimationEvent_possibleChangeState()
    {
        OnPossibleChangeState?.Invoke();
    }

    public void AnimationEvent_imPossibleChangeState()
    {
        OnImPossibleChangeState?.Invoke();
    }

    public void AnimationEvent_endAnimation()
    {
        OnEndAnimation?.Invoke();
    }

    public void AnimationEvent_effectView()
    {
        OnEffectViewAnimation?.Invoke();
    }

    public void AnimationEvent_effectHide()
    {
        OnEffectHideAnimation?.Invoke();
    }

    //------------------------------무기 장착 관련 이벤트------------------------
    public void AnimationEvent_onUnArmed()
    {
        OnUnArmed?.Invoke();
    }

    public void AnimationEvent_onEquipSword()
    {
        OnEquipSword?.Invoke();
    }

    //-------------------------------------------------------------------------
}
