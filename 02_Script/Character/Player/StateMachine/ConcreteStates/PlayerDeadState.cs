using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player »ç¸Á »óÅÂ
/// </summary>
public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        player.Anim.SetTrigger("Die");
        player.Col.enabled = false;
    }

    public override void ExitState()
    {
        base.ExitState();
        player.Col.enabled = true; 
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
