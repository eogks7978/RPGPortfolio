using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬관련 데이터
/// </summary>
[Serializable]
public class SkillData
{
    [Header("스킬, 스킬레벨 목록 리스트")] public List<SkillAndLevelData> ManagedSkillLevelInfos;
    [Header("슬롯에 장착된 스킬 데이터 리스트")] public List<SlotedSkillData> SlotedSkillLevelInfos;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="managedSkillLevelInfos">스킬, 스킬레벨 목록 리스트</param>
    /// <param name="slotedSkillLevelInfos">슬롯에 장착된 스킬 데이터 리스트</param>
    public SkillData(List<SkillAndLevelData> managedSkillLevelInfos, List<SlotedSkillData> slotedSkillLevelInfos)
    {
        ManagedSkillLevelInfos = managedSkillLevelInfos;
        SlotedSkillLevelInfos = slotedSkillLevelInfos;
    }
}
