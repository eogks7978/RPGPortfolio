using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player 상태 관리 클래스
/// </summary>
public class PlayerStateMachine
{
    public Player _player { get; private set; }
    public PlayerState CurrentPlayerState { get; private set; }

    public void Initialize(Player player, PlayerState startingState)
    {
        CurrentPlayerState = startingState;
        _player = player;
        CurrentPlayerState.EnterState();
    }

    public void ChangeState(PlayerState newState)
    {
        if (newState == CurrentPlayerState ||
            CurrentPlayerState == _player.DeadState)
            return;

        CurrentPlayerState.ExitState();
        CurrentPlayerState = newState;
        CurrentPlayerState.EnterState();
    }

    public void ChangeStateHard(PlayerState newState)
    {
        CurrentPlayerState.ExitState();
        CurrentPlayerState = newState;
        CurrentPlayerState.EnterState();
    }
}
