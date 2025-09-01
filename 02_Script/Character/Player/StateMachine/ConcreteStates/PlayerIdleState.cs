using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Player가 아무 행동도 하지 않는 상태
/// </summary>
public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        player.Anim.SetBool("Idle", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        player.Anim.SetBool("Idle", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (JoyStick.Instance.IsMoving)
            playerStateMachine.ChangeState(player.MoveState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
