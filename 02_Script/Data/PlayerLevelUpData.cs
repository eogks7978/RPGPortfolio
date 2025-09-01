using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 레벨업 시 적용될 데이터
/// </summary>
[System.Serializable]
public class PlayerLevelUpData
{
    public int Level;

    [Header("레벨 달성시 추가될 능력치들")]
    [Header("대입값")] 
    public float MaxExperience;
    [Header("추가값")]
    public int SkillPoint;
    public float MaxHealth;
    public float MaxMana;
    public float AttackPoint;
    public float DefensePoint;
    public float AttackSpeed;
    public float MoveSpeed;
    public int StatPoint;
}
