using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player가 타겟을 공격하고 있는 상태
/// </summary>
public class PlayerAttackState : PlayerState
{
    private Player _player;

    public PlayerAttackState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        _player = player;
    }

    public override void EnterState()
    {
        base.EnterState();
        _player.StartAttack();
    }

    public override void ExitState()
    {
        base.ExitState();
        _player.StopAttack();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
