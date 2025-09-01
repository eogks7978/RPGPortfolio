using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장착된 스킬들의 데이터
/// </summary>
[System.Serializable]
public class SlotedSkillData
{
    [Header("슬롯 인덱스와 스킬 데이터")] public SerializableKeyValuePair<int, SkillAndLevelData> SlotNumberAndSkill;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="slotNumberAndSkill">슬롯 인덱스와 스킬 데이터</param>
    public SlotedSkillData(SerializableKeyValuePair<int, SkillAndLevelData> slotNumberAndSkill)
    {
        SlotNumberAndSkill = slotNumberAndSkill;
    }
}
