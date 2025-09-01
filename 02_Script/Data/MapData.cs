using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵 생성 데이터
/// </summary>
[Serializable]
public class MapData
{
    [Header("맵 테이블 코드")] public int Code;
    [Header("맵 이름")] public string MapName;
    [Header("맵 디폴드 밝기")] public float Brightness;
    [Header("Bgm 테이블 코드")] public int BgmCode;
    [Header("Background 테이블 코드")] public int BackgroundCode;

    [Header("플레이어 디폴트 포지션X")] public float PlayerDefaultPositionX;
    [Header("플레이어 디폴트 포지션Y")] public float PlayerDefaultPositionY;

    [Header("타일맵 데이터")] public TilemapData TMData;
    [Header("카메라 설정 데이터")] public MapCameraData MCData;
    [Header("맵에 배치될 오브젝트들 데이터")] public List<MapObjectData> MapObjectDatas;
    [Header("몬스터 스폰 데이터")] public List<MonsterSpawnData> MonsterSpawnData;
    [Header("NPC 스폰 데이터")] public List<SerializableKeyValuePair<int, Vector2Wrapper>> NPCSpawnData;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="code">맵 테이블 코드</param>
    /// <param name="mapName">맵 이름</param>
    /// <param name="brightness">맵 디폴드 밝기</param>
    /// <param name="bgmCode">Bgm 테이블 코드</param>
    /// <param name="backgroundCode">Background 테이블 코드</param>
    /// <param name="playerDefaultPositionX">플레이어 디폴트 포지션X</param>
    /// <param name="playerDefaultPositionY">플레이어 디폴트 포지션Y</param>
    /// <param name="tMData">타일맵 데이터</param>
    /// <param name="mCData">카메라 설정 데이터</param>
    /// <param name="mapObjectDatas">맵에 배치될 오브젝트들 데이터</param>
    /// <param name="monsterSpawnData">몬스터 스폰 데이터</param>
    /// <param name="nPCSpawnData">NPC 스폰 데이터</param>
    public MapData(
        int code,
        string mapName,
        float brightness,
        int bgmCode,
        int backgroundCode,
        float playerDefaultPositionX,
        float playerDefaultPositionY,
        TilemapData tMData,
        MapCameraData mCData,
        List<MapObjectData> mapObjectDatas, 
        List<MonsterSpawnData> monsterSpawnData,
        List<SerializableKeyValuePair<int, Vector2Wrapper>> nPCSpawnData)
    {
        Code = code;
        MapName = mapName;
        Brightness = brightness;
        TMData = tMData;
        BgmCode = bgmCode;
        BackgroundCode = backgroundCode;
        PlayerDefaultPositionX = playerDefaultPositionX;
        PlayerDefaultPositionY = playerDefaultPositionY;
        MCData = mCData;
        MapObjectDatas = mapObjectDatas;
        MonsterSpawnData = monsterSpawnData;
        NPCSpawnData = nPCSpawnData;
    }
}
