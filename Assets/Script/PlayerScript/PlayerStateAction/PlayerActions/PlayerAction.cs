using UnityEngine;

public class PlayerAction
{
    protected Animator animator;
    protected Rigidbody rigid;
    public virtual void PlayAnimation(string _animationName) // 애니메이션은 뭐 있을수도 있고 없을수도 있으니까 가상으로
    {
    }
}
