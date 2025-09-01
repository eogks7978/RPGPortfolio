using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 데이터
/// </summary>
[Serializable]
public class PlayerData
{
    [Header("캐릭터 생성 후 첫 실행인지")] public bool IsFirstStart;
    [Header("플레이어 닉네임")] public string PlayerName;

    [Header("플레이어가 있는 맵의 테이블 코드")] public int CurrentMapCode;
    [Header("플레이어 포지션 X")] public float PositionX;
    [Header("플레이어 포지션 Y")] public float PositionY;

    [Header("현재 레벨")] public int Level;
    [Header("보유 스킬 포인트")] public int SkillPoint;
    [Header("보유 스탯 포인트")] public int StatPoint;
    [Header("현재 Exp")] public float CurrentExp;

    [Header("플레이어 스탯 데이터")] public PlayerStatData PlayerStatData;
    [Header("플레이어 외형 데이터")] public PlayerAppearanceData PlayerAppearanceData;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isFirstStart"></param>
    /// <param name="playerName"></param>
    /// <param name="currentMapCode"></param>
    /// <param name="positionX"></param>
    /// <param name="positionY"></param>
    /// <param name="level"></param>
    /// <param name="skillPoint"></param>
    /// <param name="statPoint"></param>
    /// <param name="currentExp"></param>
    /// <param name="playerStatData"></param>
    /// <param name="playerAppearanceData"></param>
    public PlayerData(
        bool isFirstStart,
        string playerName,
        int currentMapCode,
        float positionX,
        float positionY,
        int level,
        int skillPoint,
        int statPoint,
        float currentExp,
        PlayerStatData playerStatData, 
        PlayerAppearanceData playerAppearanceData)
    {
        IsFirstStart = isFirstStart;
        PlayerName = playerName;
        CurrentMapCode = currentMapCode;
        PositionX = positionX;
        PositionY = positionY;
        Level = level;
        SkillPoint = skillPoint;
        StatPoint = statPoint;
        CurrentExp = currentExp;
        PlayerStatData = playerStatData;
        PlayerAppearanceData = playerAppearanceData;
    }
}
