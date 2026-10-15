using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum PlayerRequestType // 플레이어에게 요청하는 타입
{
    None,
    NormalAttack,
    Skill,
    Walk,
    Run,
    Death,
    KnockDown,
    NormalReact,
    Rolling,
    WeaponToggle

}

//무조건 최우선 순위인 상태 목록
public enum PlayerPriorityStateType
{
    None,
    Death,
    KnockDown,
    NormalReact
}

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; } // 플레이어의 현재 상태
    private PlayerState pastState;

    private PlayerInputBuffer playerInputBuffer; // 선입력 처리를 위한 버퍼
    private PlayerInputReader playerInputReader; // 지속 입력 처리를 위한 변수
    private PlayerPriorityStateType playerPriorityStateType = PlayerPriorityStateType.None; // 최우선 처리 타입

    private PlayerStateFactory playerStateFactory;


    public PlayerStateMachine(PlayerInputBuffer playerInputBuffer, PlayerAnimationEvents _playerStateAnimationEvents,
        Animator _animator, PlayerStateAction _playerStateAction, PlayerInputReader playerInputReader, PlayerSkillSystem _playerSkillSystem, PlayerWeaponController _playerWeaponController)
    {
        playerStateFactory = new PlayerStateFactory(_playerStateAnimationEvents, _animator, _playerStateAction, playerInputReader, _playerSkillSystem, _playerWeaponController);

        this.playerInputBuffer = playerInputBuffer;
        this.playerInputReader = playerInputReader;

        playerInputReader.OnMoveStart += HandleWalkStart;
        playerInputReader.OnMoveEnd += HandleWalkEnd;

        playerInputReader.OnRunStart += HandleRunStart;
        playerInputReader.OnRunEnd += HandleRunEnd;

        //대기상태로 처음 생성

        if (playerStateFactory.GetPlayerState(PlayerRequestType.None, out PlayerState _idleState))
        {
            ChangeState(_idleState);
        }
    }

    public void Tick()
    {
        if(IsExistPriorityState())
        {
            ExePriorityState();
            currentState.Tick();
            return;
        }

        PreInputProcessing(); // 선입력 관련 처리
        currentState.Tick();
    }

    public void FixedTick()
    {
        currentState.FixedTick();
    }

    public void ChangeState(PlayerState _state)
    {
        //기존 상태 해제처리하기
        if (currentState != null)
        {
            currentState.OnEndAnimationEvent -= ReturnToLocomationState;
            currentState.Exit();
        }
        //새로운 상태 받아오기
        pastState = currentState;
        currentState = _state;

        //새로운 상태 첫 시작끊기
        currentState.Enter();
        currentState.OnEndAnimationEvent += ReturnToLocomationState;
    }

    //public void RemainStateAndActionCombo(PlayerState _state)
    //{
    //    _state.AnimationAction(); // 콤보실행
    //}

    private void ReturnToLocomationState(PlayerRequestType _type)
    {
        if (_type == PlayerRequestType.None && playerInputReader.MoveInput.sqrMagnitude > 0.01f)
        {
            _type = playerInputReader.IsRunning ? PlayerRequestType.Run : PlayerRequestType.Walk;
        }

        if (playerStateFactory.GetPlayerState(_type, out PlayerState state))
        {
            ChangeState(state);
        }
    }

    private void PreInputProcessing()
    {
        if (playerInputBuffer.GetRequest(out BufferedPlayerRequest _request))// 다음 동작이 있고
        {
            //지금 현재 상태에서 다음 동작으로 변경이 가능한지 여부를 판단
            if (currentState.GetIsPossibleChangeState()) // 변경 가능한 상태라면
            {
                if (currentState.GetPossibleRequestType(_request.RequestType)) // 전환이 가능하다면
                {
                    if (playerStateFactory.GetPlayerState(_request.RequestType, out PlayerState playerState))
                    {
                        if (currentState == playerState) // 동일 상태라면 캔슬 혹은 콤보를 실행하라는 뜻
                        {
                            if (currentState.TryHandleSameStateRequest(_request.RequestType))
                            {
                                playerInputBuffer.ClearRequest();
                            }
                            return;
                        }

                        if(!playerState.TryPrepareRequestAndExeCute(_request)) // 요청을 준비할 수 없다면
                        {
                            playerInputBuffer.ClearRequest();
                            return;
                        }

                        ChangeState(playerState);
                        playerInputBuffer.ClearRequest();
                    }
                }
                //foreach(PlayerRequestType type in currentState.possibleChangeStateList) // 전환 가능한 상태인지 확인
                //{
                //    if(type == _request.RequestType)
                //    {
                //        if (playerStateFactory.GetPlayerState(_request.RequestType, out PlayerState playerState))
                //        {
                //            if(currentState == playerState) // 동일 상태라면 콤보를 실행하라는 뜻
                //            {
                //                RemainState(playerState);
                //            }
                //            ChangeState(playerState);
                //        }
                //        break;
                //    }
                //}
            }
        }
    }


    private int GetPriorityValue(PlayerPriorityStateType _type)
    {
        switch (_type)
        {
            case PlayerPriorityStateType.Death:
                return 100;

            case PlayerPriorityStateType.KnockDown:
                return 99;

            case PlayerPriorityStateType.NormalReact:
                return 98;
        }
        return 0;
    }

    public void RequestPriorityState(PlayerPriorityStateType _type)
    {
        if (GetPriorityValue(playerPriorityStateType) <= GetPriorityValue(_type))
        {
            playerPriorityStateType = _type;
        }
    }

    private bool IsExistPriorityState()
    {
        if(playerPriorityStateType == PlayerPriorityStateType.None)
        {
            return false;
        }
        return true;
    }

    private void ExePriorityState()
    {
        PlayerPriorityStateType requestedType = playerPriorityStateType;
        playerPriorityStateType = PlayerPriorityStateType.None;

        PlayerRequestType requestType = PlayerRequestType.None;

        switch(requestedType)
        {
            case PlayerPriorityStateType.Death:
                requestType = PlayerRequestType.Death;
                break;

            case PlayerPriorityStateType.KnockDown:
                requestType = PlayerRequestType.KnockDown;
                break;

            case PlayerPriorityStateType.NormalReact:
                requestType = PlayerRequestType.NormalReact;
                break;
        }

        if(!playerStateFactory.GetPlayerState(requestType, out PlayerState priorityState))
        {
            return;
        }

        playerInputBuffer.ClearRequest();
        ChangeState(priorityState);
    }


    //------- 조건 처리------
    private void HandleWalkStart()
    {
        if (!currentState.GetIsPossibleChangeState())
        {
            return;
        }

        if (!currentState.GetPossibleRequestType(PlayerRequestType.Walk))
        {
            return;
        }

        if (playerStateFactory.GetPlayerState(PlayerRequestType.Walk, out PlayerState moveState))
        {
            ChangeState(moveState);
        }
    }

    private void HandleWalkEnd()
    {
        if (!playerStateFactory.GetPlayerState(PlayerRequestType.Walk, out PlayerState moveState))
        {
            return;
        }

        // 타 상태임에서 키를 뗐다고 다른 상태 무시하고 Idle처리 방지
        if (currentState != moveState)
        {
            return;
        }

        if (playerStateFactory.GetPlayerState(PlayerRequestType.None, out PlayerState idleState))
        {
            ChangeState(idleState);
        }
    }

    private void HandleRunStart()
    {
        if (!currentState.GetIsPossibleChangeState())
        {
            return;
        }

        if (!currentState.GetPossibleRequestType(PlayerRequestType.Run))
        {
            return;
        }

        if (playerStateFactory.GetPlayerState(PlayerRequestType.Run, out PlayerState runState))
        {
            ChangeState(runState);
        }
    }

    private void HandleRunEnd()
    {
        if (!playerStateFactory.GetPlayerState(PlayerRequestType.Run, out PlayerState runState))
        {
            return;
        }

        if (currentState != runState)
        {
            return;
        }

        PlayerRequestType nextStateType = playerInputReader.MoveInput.sqrMagnitude > 0.01f ? PlayerRequestType.Walk: PlayerRequestType.None;

        if (playerStateFactory.GetPlayerState(nextStateType,out PlayerState nextState))
        {
            ChangeState(nextState);
        }
    }
}
