using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데미지를 주고 받을 수 있는 클래스에 상속
/// </summary>
public interface IDamagable
{
    public float MaxHealth { get; }
    public float CurrentHealth { get; }
    public float DefensePoint { get; }
    public float AttackPoint { get; }
    public float AttackSpeed { get; }

    public bool IsDead { get; }

    public void SetStatus(Dictionary<string, float> stats);

    public object GetFieldValueByName(string fieldName);

    public void Die();
}
