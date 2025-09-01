using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬을 사용할 수 있는 객체에 상속
/// </summary>
public interface ISkillable
{
    public float MaxMana { get; }
    public float CurrentMana { get; }

    public void SetStatus(Dictionary<string, float> stats);

    IEnumerator UseSkill(int skillCode, string animationName, SerializableKeyValuePair<int, StatBonusData> statBonusData);
}
