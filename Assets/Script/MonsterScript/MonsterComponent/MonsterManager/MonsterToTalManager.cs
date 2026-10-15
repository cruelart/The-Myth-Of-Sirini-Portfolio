using UnityEngine;

public class MonsterToTalManager : MonoBehaviour
{
    [Header("몬스터 고유 아이디 입력란")]
    [SerializeField]
    private int monsterID = -1;

    private MonsterDB monsterDB;

    private MonsterBase monsterBase;
    private MonsterStatManager monsterStatManager;
    private MonsterNormalHitBox monsterHitbox;
    private MonsterAIController monsterAIController;
    private MonsterHpUIManager monsterHpUIManager;
    private MonsterReactManager monsterReactManager;
    private MonsterDeathHandler monsterDeathHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Debug.Log($"{gameObject.name} TotalManager Awake");
        //이 스크립트의 주인 오브젝트의 하위 컴포넌트들 받아오기

        monsterBase = GetComponent<MonsterBase>();
        monsterStatManager = GetComponent<MonsterStatManager>();
        monsterHitbox = GetComponentInChildren<MonsterNormalHitBox>(true);
        monsterAIController = GetComponent<MonsterAIController>();
        monsterHpUIManager = GetComponentInChildren<MonsterHpUIManager>();
        monsterReactManager = GetComponent<MonsterReactManager>();
        monsterDeathHandler = GetComponent<MonsterDeathHandler>();

        LoadData(); // MonsterDBManager의 instance 내용물은 Awake에서 처리되기때문에 Start에 넣어줌 -> 수정본(부트스트랩씬에서 미리 초기화했기떄문에 Awake로 옮김)
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LoadData()
    {
        monsterDB = MonsterDBManager.Instance.GetMonsterDB(monsterID); // 해당 몬스터의 데이터를 받아옴

        //데이터전달
        monsterStatManager.Init(monsterDB);
        monsterHitbox.Init(monsterStatManager);
        monsterAIController.Init(monsterStatManager);
        monsterHpUIManager.Init(monsterStatManager);
        monsterReactManager.Init(monsterStatManager);
        monsterDeathHandler.Init(monsterID, monsterStatManager, monsterBase);
    }
}
