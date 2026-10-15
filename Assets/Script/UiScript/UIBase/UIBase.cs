using UnityEngine;

public class UIBase : MonoBehaviour
{
    public virtual void UIOpen()
    {
        this.gameObject.SetActive(true);
        this.gameObject.transform.SetAsLastSibling();
    }

    public virtual void UIHide()
    {
        this.gameObject.SetActive(false);
    }

}
