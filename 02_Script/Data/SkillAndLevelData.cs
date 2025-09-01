using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 저장될 스킬의 현재 레벨
/// </summary>
[Serializable]
public class SkillAndLevelData
{
    [Header("스킬의 테이블 코드")] public int skillCode;
    [Header("스킬 레벨")] public int level;
}
