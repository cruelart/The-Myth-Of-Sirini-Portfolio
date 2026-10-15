using System;
using UnityEngine;

//이 스크립트는 입력만을 처리합니다.
public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }

    //플레이어의 이동 방향 변수
    private int lastHorizontalInput; // 수평 입력 저장 (-1 왼쪽이고 1이 오른쪽)
    private int lastVerticalInput; // 수직 입력 저장 (-1이 아래고 1이 위쪽)

    //플레이어 달리기관련 변수

    //더블클릭 판정 설정
    private float doubleClickTimeLimit = 0.25f;

    private KeyCode lastClickMoveKey = KeyCode.None; // 마지막으로 누른 방향 키 저장
    private float lastClickMoveKeyTime = -999f;

    //private KeyCode pressingRunKey = KeyCode.None;

    public bool IsRunning { get; private set; }

    //UI관련 입력
    public event Action OnInteractPressed; // 상호작용 -> TalkUI띄우는 용도
    public event Action OnInventoryPressed; // 인벤토리 -> 인벤토리 UI 띄우는 용도
    public event Action OnEquipSpacePressed; // 장비창 -> 장비창 UI 띄우는 용도
    public event Action OnQuestPressed; // 퀘스트 -> 퀘스트 UI 띄우는 용도
    public event Action OnPlayerStatusPressed; // 플레이어 정보 -> 플레이어 정보 UI 띄우는 용도
    public event Action OnUIExitPressed; // Exit

    //선입력 저장 관련 입력
    public event Action<PlayerRequestType, int , float> OnPlayerRequst; // 선입력 요청 저장

    //이동 관련 입력
    public event Action OnMoveStart;
    public event Action OnMoveEnd;

    public event Action OnRunStart;
    public event Action OnRunEnd;


    //public event Action On
    // Start is called once before the first execution of Updatep after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------
        UpdateUIInput();
        UpdateActionInput();
        UpdateMoveInput();
    }

    private void OnDisable()
    {
        MoveInput = Vector2.zero;
        lastHorizontalInput = 0;
        lastVerticalInput = 0;

        IsRunning = false;
        //pressingRunKey = KeyCode.None;

        lastClickMoveKey = KeyCode.None;
        lastClickMoveKeyTime = -999f;
    }

    private void UpdateUIInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
        }

        //----------------------------------------------------UI On - Off
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnInventoryPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnEquipSpacePressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OnPlayerStatusPressed?.Invoke();
        }

        //target과의 상호작용
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnInteractPressed?.Invoke();
            //target?.Interact(transform.parent.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnQuestPressed?.Invoke();
            //UIManager.Instance.ShowQuestUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnUIExitPressed?.Invoke();
        }
    }
    private void UpdateMoveInput()
    {
        bool isRunStart = false;
        Vector2 previousInput = MoveInput;

        bool isLeftInput = Input.GetKey(KeyCode.A);
        bool isRightInput = Input.GetKey(KeyCode.D);
        bool isDownInput = Input.GetKey(KeyCode.S);
        bool isUpInput = Input.GetKey(KeyCode.W);

        if (Input.GetKeyDown(KeyCode.A))
        {
            lastHorizontalInput = -1;
            isRunStart = TrypStartRun(KeyCode.A) || isRunStart;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            lastHorizontalInput = 1;
            isRunStart = TrypStartRun(KeyCode.D) || isRunStart;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            lastVerticalInput = -1;
            isRunStart = TrypStartRun(KeyCode.S) || isRunStart;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            lastVerticalInput = 1;
            isRunStart = TrypStartRun(KeyCode.W) || isRunStart;
        }

        float horizontal = 0.0f;
        float vertical = 0.0f;

        if (isLeftInput && isRightInput) // 왼쪽 오른쪽 동시입력이라면
        {
            horizontal = lastHorizontalInput; // 나중에 입력받은 것을 우선시해서 처리예정 -> 하나 무시해버리기
        }
        else if (isLeftInput)
        {
            horizontal = -1.0f;
        }
        else if (isRightInput)
        {
            horizontal = 1.0f;
        }

        if (isDownInput && isUpInput)
        {
            vertical = lastVerticalInput;
        }
        else if(isDownInput)
        {
            vertical = -1.0f;
        }
        else if(isUpInput)
        {
            vertical = 1.0f;
        }

        MoveInput = Vector2.ClampMagnitude(new Vector2(horizontal, vertical),1f); // 대각선 보정 

        bool wasMoving = previousInput.sqrMagnitude > 0.01f; // 움직임이 조금이라고 있었다면

        bool isMoving = MoveInput.sqrMagnitude > 0.01f; // 현재 움직임이 조금이라도 있다면

        bool isRunEnd = IsRunning && !isLeftInput && !isRightInput && !isDownInput && !isUpInput; // 뛰는 상태에서 모든 방향키를 때버렸다면

        if (isRunStart)
        {
            OnRunStart?.Invoke();
        }

        else if (!wasMoving && isMoving)
        {
            OnMoveStart?.Invoke();
        }

        if(isRunEnd)
        {
            IsRunning = false;
            //pressingRunKey = KeyCode.None;

            OnRunEnd?.Invoke();
        }
        else if (wasMoving && !isMoving)
        {
            OnMoveEnd?.Invoke();
        }
    }

    private bool TrypStartRun(KeyCode _pressedKey)
    {
        float currentTime = Time.unscaledTime;

        bool isDoubleClick = (lastClickMoveKey == _pressedKey) && (currentTime - lastClickMoveKeyTime <= doubleClickTimeLimit);

        lastClickMoveKey = _pressedKey;
        lastClickMoveKeyTime = currentTime;

        if (!isDoubleClick)
        {
            return false;
        }

        //여기부터는 더블 클릭에 성공한 단계이므로

        IsRunning = true;
        //pressingRunKey = _pressedKey;

        lastClickMoveKey = KeyCode.None; // 초기화시키고
        lastClickMoveKeyTime = -999f;

        return true;
    }

    private void UpdateActionInput()
    {
        //------------선입력 관련 처리---------------------

        if (Input.GetMouseButtonDown(0)) // 공격
        {
            OnPlayerRequst?.Invoke(PlayerRequestType.NormalAttack, 0, Time.unscaledTime + 0.2f);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnPlayerRequst?.Invoke(PlayerRequestType.Rolling, 0, Time.unscaledTime + 0.2f);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnPlayerRequst?.Invoke(PlayerRequestType.Skill, 0, Time.unscaledTime + 0.2f);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnPlayerRequst?.Invoke(PlayerRequestType.Skill, 1, Time.unscaledTime + 0.2f);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnPlayerRequst?.Invoke(PlayerRequestType.WeaponToggle, 0, Time.unscaledTime + 0.2f);
        }
    }
}
