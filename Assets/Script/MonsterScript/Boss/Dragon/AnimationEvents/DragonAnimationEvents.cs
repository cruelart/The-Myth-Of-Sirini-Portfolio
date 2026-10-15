using System;
using UnityEngine;

public class DragonAnimationEvents : MonoBehaviour
{
    public event Action RoarFinished;
    public event Action BiteFinished;
    public event Action BiteStart;

    public void AnimationEvent_RoarEnd()
    {
        RoarFinished?.Invoke();
    }

    public void AnimationEvent_BiteEnd()
    {
        BiteFinished?.Invoke();
    }

    public void AnimationEvent_BiteStart()
    {
        BiteStart?.Invoke();
    }
}
