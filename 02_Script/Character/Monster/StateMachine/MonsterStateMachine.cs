using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터 상태를 관리하는 클래스
/// </summary>
public class MonsterStateMachine
{
    public Monster _monster { get; private set; }
    public MonsterState CurrentMonsterState { get; private set; }

    public void Initialize(Monster monster, MonsterState startingState)
    {
        _monster = monster;
        CurrentMonsterState = startingState;
        CurrentMonsterState.EnterState();
    }

    public void ChangeState(MonsterState newState)
    {
        if (newState == CurrentMonsterState 
            || CurrentMonsterState == _monster.DeadState)
            return;

        CurrentMonsterState.ExitState();
        CurrentMonsterState = newState;
        CurrentMonsterState.EnterState();
    }

    public void ChangeStateHard(MonsterState newState)
    {
        CurrentMonsterState.ExitState();
        CurrentMonsterState = newState;
        CurrentMonsterState.EnterState();
    }
}
