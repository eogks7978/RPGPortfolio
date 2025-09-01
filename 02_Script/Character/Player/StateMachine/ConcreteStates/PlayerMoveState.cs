using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 플레이어 이동시 상태
/// </summary>
public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        if (JoyStick.Instance.IsMoving)
        {
            player.Anim.SetFloat("MoveSpeed", player.MoveSpeed);
            player.Anim.SetBool("Run", true);
        }
        else
            player.StartMove();

        EventHandler.OnMonsterSelected += OnMosnterSelected;
    }

    public override void ExitState()
    {
        base.ExitState();
        player.RB.velocity = Vector2.zero;
        player.Anim.SetBool("Run", false);
        EventHandler.OnMonsterSelected -= OnMosnterSelected;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (JoyStick.Instance.IsMoving)
            player.Move(JoyStick.Instance.InputDirection);
    }

    public void OnMosnterSelected()
    {
        playerStateMachine.ChangeState(player.AttackState);
    }
}
