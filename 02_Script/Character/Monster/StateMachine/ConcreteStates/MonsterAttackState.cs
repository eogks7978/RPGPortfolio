using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터가 타겟을 공격하는 상태
/// </summary>
public class MonsterAttackState : MonsterState
{
    public MonsterAttackState(Monster monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        monster.Attack();
    }

    public override void ExitState()
    {
        base.ExitState();
        monster.StopAttack();
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
