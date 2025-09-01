using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵에 배치되는 오브젝트 데이터
/// </summary>
[Serializable]
public class MapObjectData
{
    [Header("맵 오브젝트 테이블 코드")] public int Code;
    [Header("배치 될 포지션 X")] public float PosX;
    [Header("배치 될 포지션 Y")] public float PosY;
    [Header("데이터 저장, 불러오기 목적의 데이터 형식")] public Dictionary<string, object> Properties;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="code">맵 오브젝트 테이블 코드</param>
    /// <param name="posX">배치 될 포지션 X</param>
    /// <param name="posY">배치 될 포지션 Y</param>
    /// <param name="properties">데이터 저장, 불러오기 목적의 데이터 형식</param>
    public MapObjectData(
        int code, 
        float posX, 
        float posY,
        Dictionary<string, object> properties)
    {
        Code = code;
        PosX = posX;
        PosY = posY;
        Properties = properties;
    }
}
