using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 이펙트 집합 핸들러
/// </summary>
public class EventHandler : Singleton<EventHandler>
{
    public static Action OnPlayerStatChanged;
    public static Action OnCameraSizeChanged;
    public static Action OnMonsterSelected;
    public static Action<int> OnMoneyEarned;
    public static Action OnMapChanged;
    public static Action OnPlayerLevelUp;
    public static Action OnSkillLevelUp;
    public static Action OnJoyStickDragEnd;
    public static Action OnMainUIButtonUpdated;
    public static Action OnItemsUpdated = delegate { };
    public static Action<Type> OnPopupOpened;
    public static Action OnfirstStart;
    public delegate void MonsterDeadEvent(Monster monster);
    public static event MonsterDeadEvent OnMonsterDead;

    public static void OnMonsterDie(Monster monster) => OnMonsterDead?.Invoke(monster);
}
