using UnityEngine;

public interface IT_ShowItemName
{
    public void ShowItemName();
    public void HideItemName();
}
public class DropItemCollider : MonoBehaviour, IT_ShowItemName
{
    [SerializeField]
    GameObject TextObj;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TextObj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TextObj.SetActive(false);
        }
    }

    private void OnDisable()
    {
        TextObj.SetActive(false);
    }

    public void ShowItemName()
    {
        TextObj.SetActive(true);
    }

    public void HideItemName()
    {
        TextObj.SetActive(false);
    }
}
