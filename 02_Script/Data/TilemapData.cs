using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 타일맵 Grid에 배치된 타일들 데이터
/// </summary>
[Serializable]
public class TilemapData
{
    [Header("DeepBackTile들의 이름과 위치")] public Dictionary<string, List<Vector3Int>> DeepBackTileData;
    [Header("BackTile들의 이름과 위치")] public Dictionary<string, List<Vector3Int>> BackTileData;
    [Header("MiddleTile들의 이름과 위치")] public Dictionary<string, List<Vector3Int>> MiddleTileData;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="deepBackTileData">DeepBackTile들의 이름과 위치</param>
    /// <param name="backTileData">BackTile들의 이름과 위치</param>
    /// <param name="middleTileData">MiddleTile들의 이름과 위치</param>
    public TilemapData(
        Dictionary<string, List<Vector3Int>> deepBackTileData,
        Dictionary<string, List<Vector3Int>> backTileData,
        Dictionary<string, List<Vector3Int>> middleTileData)
    {
        DeepBackTileData = deepBackTileData;
        BackTileData = backTileData;
        MiddleTileData = middleTileData;
    }
}