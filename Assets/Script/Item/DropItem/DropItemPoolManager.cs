using System.Collections.Generic;
using UnityEngine;

public class DropItemPoolManager : MonoBehaviour
{
    public static DropItemPoolManager Instance;

    [SerializeField]
    private GameObject dropItemObj;

    public Queue<InstanceDropItem> instanceDropItemList = new Queue<InstanceDropItem>();

    private int poolsize = 100; // 기본적인 드랍아이템의 개수는 100개정도로 설정

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        CreatePool();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePool()
    {
        for(int i = 0; i < poolsize; i++)
        {
            GameObject instanceDropItemObj = Instantiate(dropItemObj, this.transform);

            InstanceDropItem instanceDropItem = instanceDropItemObj.GetComponent<InstanceDropItem>();

            instanceDropItemList.Enqueue(instanceDropItem);

            instanceDropItemObj.SetActive(false);

        }
    }

    public void DropItem(int _itemID, int _amount, Vector3 _itemPostion)
    {
        BaseitemDB itemDB = ItemDataManager.Instance.GetBaseitemDB(_itemID); // 드랍시킬 아이템의 정보를 불러옴

        // 활성화 시킬 오브젝트 꺼내기
        InstanceDropItem instanceDropItem = instanceDropItemList.Dequeue();

        GameObject instanceDropItemObj = instanceDropItem.gameObject;

        // 꺼낸 오브젝트를 드랍시킬 아이템으로 둔갑시키기
        instanceDropItem.SettingDropItem(itemDB.dropMesh, itemDB.dropMaterial, itemDB.dropRotation, itemDB.dropScale, _itemPostion, _itemID, _amount);

        //씬에 활성화시키기
        instanceDropItemObj.SetActive(true);

        instanceDropItem.RealDrop();
    }

    public void ReturnPool(InstanceDropItem _dropItem)
    {
        GameObject instanceDropItemObj = _dropItem.gameObject;
        instanceDropItemObj.SetActive(false);

        instanceDropItemList.Enqueue(_dropItem);
    }
}
