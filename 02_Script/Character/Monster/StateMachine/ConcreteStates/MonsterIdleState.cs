using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터가 플레이어를 발견하기 전 상태
/// </summary>
public class MonsterIdleState : MonsterState
{
    public MonsterIdleState(Monster monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        monster.StartPositionMove(monster.WanderingMove());
        monster.Anim.SetBool("Idle", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        monster.StopPositionMove();
        monster.Anim.SetBool("Idle", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (monster.IsAggroed)
        {
            monsterStateMachine.ChangeState(monster.ChaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
