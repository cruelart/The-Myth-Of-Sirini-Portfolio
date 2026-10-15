using System.Collections;
using UnityEngine;

public class InstanceDropItem : MonoBehaviour
{
    [Header("하위 Visual Root에 대한 필요정보")]
    [SerializeField]
    private Transform visualRootTransform;

    [SerializeField]
    private MeshFilter meshFilter;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [Header("UI관련 정보")]
    [SerializeField]
    private DropItemUI dropItemUI;

    [Header("이펙트 정보")]
    [SerializeField]
    private Transform effectTransform;

    [Header("드롭아이템 유지 시간")]
    [SerializeField]
    private float returnTime = 60.0f;

    private Rigidbody rigid;

    //자체적으로 이 오브젝트가 들고있는 정보들 나열
    public int itemID { get; private set; }
    public int amount;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ReturnDropItem());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SettingDropItem(Mesh _itemMesh, Material _itemMaterial, Vector3 _itemRotation, Vector3 _itemScale, Vector3 _itemPosition, int _itemID, int _amount)
    {
        //이미지 변경
        meshFilter.sharedMesh = _itemMesh;
        meshRenderer.sharedMaterial = _itemMaterial;

        //이미지 위치 조절
        visualRootTransform.localEulerAngles = _itemRotation + new Vector3(-90, 0,0);
        visualRootTransform.localScale = _itemScale;
        this.transform.position = _itemPosition + new Vector3(0, 1.5f, 0);
        visualRootTransform.position = _itemPosition + new Vector3(0,1.5f,0);
        effectTransform.position = visualRootTransform.position;

        //UI정보 초기화
        dropItemUI.Init(_itemPosition, _itemID, _amount);

        //아이템 아이디 저장
        itemID = _itemID;

        amount = _amount;
    }

    public void RealDrop()
    {
        Vector3 random_force = new Vector3(Random.Range(-3f, 3f), Random.Range(4f, 6f), Random.Range(-3f, 3f));

        rigid.AddForce(random_force, ForceMode.Impulse);
    }
    
    public void ImmediatelyreturnDropItem()
    {
        DropItemPoolManager.Instance.ReturnPool(this);
    }

    //돌아가야지, 일정 시간지나면 다시 풀로
    IEnumerator ReturnDropItem()
    {
        yield return new WaitForSeconds(returnTime);

        DropItemPoolManager.Instance.ReturnPool(this);
    }
}
