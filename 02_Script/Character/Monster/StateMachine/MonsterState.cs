using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터 상태의 부모격 클래스
/// </summary>
public class MonsterState
{
    protected Monster monster;
    protected MonsterStateMachine monsterStateMachine;

    public MonsterState(Monster monster, MonsterStateMachine monsterStateMachine)
    {
        this.monster = monster;
        this.monsterStateMachine = monsterStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
}
