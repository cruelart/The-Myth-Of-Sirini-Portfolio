using UnityEngine;

//해당 스크립트는 플레이어의 입력을 일정시간 저장 및 소멸시키고 실행시키기 위함

public readonly struct BufferedPlayerRequest // 굳이 객체생성하지 않을 예정이니 구조체로 선언
{
    public PlayerRequestType RequestType { get; } // 저장될 요청 타입
    public int RequestTypeIndex { get; } // 헤딩 타입의 어떤 것을 실행할 것인지 정하는 변수
    
    public float remainTime { get; } // 버퍼에 유지되는 시간

    public BufferedPlayerRequest(PlayerRequestType _playerRequestType, int _requestTypeIndex, float _reaminTime)
    {
        RequestType = _playerRequestType;
        RequestTypeIndex = _requestTypeIndex;
        remainTime = _reaminTime;
    }
}

public sealed class PlayerInputBuffer
{
    private BufferedPlayerRequest bufferedPlayerRequest;

    public void InitConnectEvent(PlayerInputReader _inputReader) // 해당 함수는 단 한번만 호출하도록 하자.
    {
        _inputReader.OnPlayerRequst += ConnectEvent;
    }

    //요청 저장 -> 기존저장있으면 그냥 덮어씌울예정
    private void StoreRequest(PlayerRequestType _playerRequestType, int _requestTypeIndex, float _currentTimePlusRemainTime)
    {
        bufferedPlayerRequest = new BufferedPlayerRequest(_playerRequestType, _requestTypeIndex, _currentTimePlusRemainTime);
    }
    
    public void ClearRequest()
    {
        bufferedPlayerRequest = default;
    }

    public bool GetRequest(out BufferedPlayerRequest bufferedRequest)
    {
        CheckIsRomoveRequest();
        
        if(bufferedPlayerRequest.RequestType == PlayerRequestType.None)
        {
            bufferedRequest = default;
            return false; // 현재 저장된 버퍼는 없으니 false 반환
        }

        bufferedRequest = bufferedPlayerRequest;
        return true; // 저장된 버퍼가 있으니 반환가능
    }

    //public bool CheckExeRequest()
    //{
    //    CheckIsRomoveRequest();

    //    if(!hasRequest)
    //    {
    //        return false;
    //    }

    //    return true;
    //}

    private void CheckIsRomoveRequest() // 삭제해야할 요청인지 확인
    {
        if(bufferedPlayerRequest.RequestType == PlayerRequestType.None) // 요청이 없으면 굳이 확인할 필요도 없음
        {
            return;
        }

        if(Time.unscaledTime <= bufferedPlayerRequest.remainTime)
        {
            return;
        }

        ClearRequest(); // 
    }

    private void ConnectEvent(PlayerRequestType _requestType, int _requestTypeIndex, float _currentTimePlusRemainTime)
    {
        switch(_requestType)
        {
            case PlayerRequestType.None:
                return;
            case PlayerRequestType.NormalAttack:
                StoreRequest(_requestType, _requestTypeIndex, _currentTimePlusRemainTime);
                break;
            case PlayerRequestType.Rolling:
                StoreRequest(_requestType, _requestTypeIndex, _currentTimePlusRemainTime);
                break;
            case PlayerRequestType.Skill:
                StoreRequest(_requestType, _requestTypeIndex, _currentTimePlusRemainTime);
                break;

            case PlayerRequestType.WeaponToggle:
                StoreRequest(_requestType, _requestTypeIndex, _currentTimePlusRemainTime);
                break;
        }
    }
}
