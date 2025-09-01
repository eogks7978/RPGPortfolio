using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 타겟 발견 이후 타겟이 탐지범위 밖으로 나갔을 때의 상태
/// </summary>
public class MonsterBackToSpawnState : MonsterState
{
    public MonsterBackToSpawnState(Monster monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        monster.SetAggroStatus(false);
        monster.StartPositionMove(monster.MoveToSpawnPos());
    }

    public override void ExitState()
    {
        base.ExitState();
        monster.StopPositionMove();
        monster.Anim.SetBool("Run", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (monster.IsAggroed)
        {
            monster.StateMachine.ChangeState(monster.ChaseState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
