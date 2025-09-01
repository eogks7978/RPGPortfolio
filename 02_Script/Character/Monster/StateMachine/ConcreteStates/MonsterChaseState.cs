using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타겟을 발견하고 추격하는 상태
/// </summary>
public class MonsterChaseState : MonsterState
{
    public MonsterChaseState(Monster monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        monster.StartTransformMove();
    }

    public override void ExitState()
    {
        base.ExitState();
        monster.Anim.SetBool("Run", false);
        monster.StopTransformMove();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (!monster.IsAggroed)
        {
            monster.StateMachine.ChangeState(monster.BackToSpawnState);
        }

        if (monster.IsWithinStrikingDistance)
        {
            monster.StateMachine.ChangeState(monster.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
