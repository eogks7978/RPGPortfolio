using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장비 강화 데이터 리스트
/// </summary>
[System.Serializable]
public class EquipmentData
{
    public List<EquipmentStatData> EnhanceBonus;
}

/// <summary>
/// 장비 강화 데이터
/// </summary>
[System.Serializable]
public class EquipmentStatData
{
    [Header("해당 레벨에서의 강화 시도")]
    public int Level;
    [Header("성공 확률")]
    public float SuccessChance;
    [Header("성공 시 데이터")]
    public List<EquipmentEnhanceData> EnhanceDatas;
    [Header("강화 재료 아이템 코드, 개수")]
    public List<SerializableKeyValuePair<int, int>> MaterialItemCodeAndRequiredNumber;
}

/// <summary>
/// 강화 성공시 데이터
/// </summary>
[System.Serializable]
public class EquipmentEnhanceData
{
    [Header("성공 시 보너스 스탯")]
    public string StatToEnhance;
    [Header("성공 시 최소 상승치")]
    public float MinValue;
    [Header("성공 시 최대 상승치")]
    public float MaxValue;
}