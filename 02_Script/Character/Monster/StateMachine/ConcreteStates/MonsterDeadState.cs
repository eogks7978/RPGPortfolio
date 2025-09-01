using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터가 죽고 ObjectPool로 반환 전의 상태
/// </summary>
public class MonsterDeadState : MonsterState
{
    public MonsterDeadState(Monster monster, MonsterStateMachine monsterStateMachine) : base(monster, monsterStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        monster.SetPlayerDetectCollider(false);
        monster.SetCollider(false);
        monster.StopAllCoroutines();
        monster.Anim.SetTrigger("Die");
    }

    public override void ExitState()
    {
        base.ExitState();
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
