using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory
{
    private Dictionary<PlayerRequestType, PlayerState> allPlayerStateMap = new();

    public PlayerStateFactory(PlayerAnimationEvents _playerStateAnimationEvents, Animator _animator, PlayerStateAction _playerStateAction,
        PlayerInputReader _inputReader, PlayerSkillSystem _playerSkillSystem, PlayerWeaponController _playerWeaponController)
    {
        AddAllStateInMap(_playerStateAnimationEvents, _animator, _playerStateAction, _inputReader, _playerSkillSystem, _playerWeaponController);
    }

    public bool GetPlayerState(PlayerRequestType _playerRequestType, out PlayerState playerState)
    {
        return allPlayerStateMap.TryGetValue(_playerRequestType, out playerState);
    }

    private void AddAllStateInMap(PlayerAnimationEvents _playerStateAnimationEvents, Animator _animator, PlayerStateAction _playerStateAction,
        PlayerInputReader _inputReader, PlayerSkillSystem _playerSkillSystem, PlayerWeaponController _playerWeaponController)
    {
        if (!allPlayerStateMap.TryGetValue(PlayerRequestType.None, out PlayerState IdleState))
        {
            allPlayerStateMap[PlayerRequestType.None] = new IdleState(_playerStateAnimationEvents, _animator, _playerStateAction);
        }

        allPlayerStateMap[PlayerRequestType.Walk] = new MoveState(_playerStateAnimationEvents, _animator, _playerStateAction, _inputReader);

        if (!allPlayerStateMap.TryGetValue(PlayerRequestType.NormalAttack, out PlayerState NormalAttackState))
        {
            allPlayerStateMap[PlayerRequestType.NormalAttack] = new NormalAttackState(_playerStateAnimationEvents, _animator, _playerStateAction);
        }

        if (!allPlayerStateMap.TryGetValue(PlayerRequestType.Run, out PlayerState RunState))
        {
            allPlayerStateMap[PlayerRequestType.Run] = new RunState(_playerStateAnimationEvents, _animator, _playerStateAction, _inputReader);
        }

        if (!allPlayerStateMap.TryGetValue(PlayerRequestType.KnockDown, out PlayerState KnockDownState))
        {
            allPlayerStateMap[PlayerRequestType.KnockDown] = new KnockDownState(_playerStateAnimationEvents, _animator, _playerStateAction);
        }

        if (!allPlayerStateMap.TryGetValue(PlayerRequestType.NormalReact, out PlayerState NormalReactState))
        {
            allPlayerStateMap[PlayerRequestType.NormalReact] = new NormalReactState(_playerStateAnimationEvents, _animator, _playerStateAction);
        }

        if (!allPlayerStateMap.TryGetValue(PlayerRequestType.Rolling, out PlayerState RollingState))
        {
            allPlayerStateMap[PlayerRequestType.Rolling] = new RollingState(_playerStateAnimationEvents, _animator, _playerStateAction);
        }

        if (!allPlayerStateMap.TryGetValue(PlayerRequestType.Death, out PlayerState DeathState))
        {
            allPlayerStateMap[PlayerRequestType.Death] = new DeathState(_playerStateAnimationEvents, _animator, _playerStateAction);
        }

        if(!allPlayerStateMap.TryGetValue(PlayerRequestType.Skill, out PlayerState SkillState))
        {
            allPlayerStateMap[PlayerRequestType.Skill] = new SkillState(_playerStateAnimationEvents, _animator, _playerStateAction, _playerSkillSystem);
        }

        if (!allPlayerStateMap.TryGetValue(PlayerRequestType.WeaponToggle, out PlayerState WeaponToggleState))
        {
            allPlayerStateMap[PlayerRequestType.WeaponToggle] = new WeaponChangeState(_playerStateAnimationEvents, _animator, _playerStateAction, _playerWeaponController);
        }
    }
}
