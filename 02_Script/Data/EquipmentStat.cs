using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¿Â∫Ò Ω∫≈»
/// </summary>
[Serializable]
public class EquipmentStat
{
    public int MaxHealth;
    public int MaxMana;
    public int AttackPoint;
    public int DefensePoint;
    public float AttackSpeed;
    public float MoveSpeed;

    public int Strength;
    public int Dexterity;
    public int Constitution;

    public EquipmentStat(
        int maxHealth = 0,
        int maxMana = 0, 
        int attackPoint = 0, 
        int defensePoint = 0,
        float attackSpeed = 0f,
        float moveSpeed = 0f, 
        int strength = 0, 
        int dexterity = 0, 
        int constitution = 0)
    {
        MaxHealth = maxHealth;
        MaxMana = maxMana;
        AttackPoint = attackPoint;
        DefensePoint = defensePoint;
        AttackSpeed = attackSpeed;
        MoveSpeed = moveSpeed;
        Strength = strength;
        Dexterity = dexterity;
        Constitution = constitution;
    }
}
