using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 타겟 접근, 공격 트리거
/// </summary>
public interface ITriggerCheckable
{
    [Tooltip("Trigger 충돌시 true")] bool IsAggroed { get; }
    [Tooltip("공격범위 안에 있는지 체크")] bool IsWithinStrikingDistance { get; }
    void SetAggroStatus(bool isAggroed); 
    void SetStrikingDistanceBool(bool isWithinStrikingDistance);
}
