using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 스탯 데이터
/// </summary>
[Serializable]
public class PlayerStatData
{
    public float MaxHealth;
    public float CurrentHealth;
    public float MaxMana;
    public float CurrentMana;
    public float AttackPoint;
    public float DefensePoint;
    public float AttackSpeed;
    public float MoveSpeed;

    public int Strength;
    public int Dexterity;
    public int Constitution;

    public PlayerStatData(
        float maxHp,
        float currentHp,
        float maxMp,
        float currentMp,
        float attackPoint,
        float defensePoint,
        float attackSpeed,
        float moveSpeed,
        int strength,
        int dexterity,
        int constitution)
    {
        MaxHealth = maxHp;
        CurrentHealth = currentHp;
        MaxMana = maxMp;
        CurrentMana = currentMp;
        AttackPoint = attackPoint;
        DefensePoint = defensePoint;
        AttackSpeed = attackSpeed;
        MoveSpeed = moveSpeed;
        Strength = strength;
        Dexterity = dexterity;
        Constitution = constitution;
    }
}
