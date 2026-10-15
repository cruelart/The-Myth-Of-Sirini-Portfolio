using UnityEngine;

public class PlayerTotalController : MonoBehaviour
{
    //입력처리담당
    private PlayerInputReader playerInputReader;

    //선입력 정보
    private PlayerInputBuffer playerInputBuffer;

    //상태결정
    private PlayerStateMachine playerStateMachine;

    //애니메이터
    private Animator animator;

    //컨트롤러
    private PlayerWeaponController weaponController;

    //플레이어 실제 상태별 행동실행
    private PlayerStateAction stateAction;

    //스킬
    private PlayerSkillSystem playerSkillSystem;

    //애니메이션 이벤트
    PlayerAnimationEvents playerStateAnimationEvents;

    private static readonly int WeaponStyleHash = Animator.StringToHash("WeaponStyle");
    private void Awake()
    {
        PlayerReactManager playerReactManager = GetComponent<PlayerReactManager>(); 
        playerInputReader = GetComponent<PlayerInputReader>();
        playerSkillSystem = GetComponent<PlayerSkillSystem>();
        playerStateAnimationEvents = GetComponent<PlayerAnimationEvents>();
        weaponController = GetComponent<PlayerWeaponController>();
        animator = GetComponent<Animator>();
        stateAction = GetComponent<PlayerStateAction>();
        stateAction.Init();

        playerInputBuffer = new PlayerInputBuffer();
        playerStateMachine = new PlayerStateMachine(playerInputBuffer, playerStateAnimationEvents, animator, stateAction, playerInputReader, playerSkillSystem, weaponController);

        playerInputBuffer.InitConnectEvent(playerInputReader);

        playerReactManager.OnPriorityStateRequested += playerStateMachine.RequestPriorityState;

        weaponController.OnActiveWeaponStyleChanged += HandleActiveWeaponStyleChanged;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerStateMachine.Tick();
    }

    private void FixedUpdate()
    {
        playerStateMachine.FixedTick();
    }

    private void HandleActiveWeaponStyleChanged(WeaponStyle weaponStyle)
    {
        animator.SetFloat(WeaponStyleHash, (float)weaponStyle);
    }

    private void OnDestroy()
    {
        if (weaponController == null)
        {
            return;
        }

        weaponController.OnActiveWeaponStyleChanged -= HandleActiveWeaponStyleChanged;
    }
}
