using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터 스폰 데이터
/// </summary>
[Serializable]
public class MonsterSpawnData
{
    [Header("몬스터 테이블 코드")] public int MonsterCode;
    [Header("몬스터가 스폰 될 포지션")] public List<Vector2Wrapper> MonsterSpawnPos;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="monsterCode">몬스터 테이블 코드</param>
    /// <param name="monsterSpawnPos">몬스터가 스폰 될 포지션</param>
    public MonsterSpawnData(int monsterCode, List<Vector2Wrapper> monsterSpawnPos)
    {
        MonsterCode = monsterCode;
        MonsterSpawnPos = monsterSpawnPos;
    }
}
