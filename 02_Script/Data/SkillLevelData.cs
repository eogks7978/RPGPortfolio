using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬 하나에 대한 최대 레벨과 해당 레벨에 대한 데미지의 스탯 배율
/// </summary>
[Serializable]
public class SkillLevelData
{
    public List<SerializableKeyValuePair<int, StatBonusData>> SkillLevelAndStatus;
    public int maxSkillLevel;
}

/// <summary>
/// 스킬 하나에 대한 능력치
/// </summary>
[Serializable]
public class StatBonusData
{
    public List<StatMagnificationData> StatMagnification;
    [Header("사정거리")] public float ActiveRange;
    [Header("필요마나")] public float requiredMana;
    [Header("쿨타임")] public float Cooldown;
    [Header("스킬 범위")] public float SkillRange;
}

/// <summary>
/// 스킬의 스탯에 대한 배율 데이터
/// </summary>
[Serializable]
public class StatMagnificationData
{
    public string statName;
    [Header("배율")] public float value;
}