using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 맵 제작 클래스. 프리펩 안에서 사용.
/// </summary>
[Serializable]
public class MapEditorClass : MonoBehaviour
{
    public MapObjectTable MapObjTb;

    public Tilemap DeepBackTilemap;
    public Tilemap BackTilemap;
    public Tilemap MiddleTilemap;
    public Transform ObjectParentTr;
    public Transform PlayerDefaultTr;

    public int Code;
    public string MapName;
    public float Brightness;
    public int BgmCode;
    public int BackgroundCode;

    public bool UseCameraOffset;
    public Vector2 CameraOffsetMin;
    public Vector2 CameraOffsetMax;

    public List<MonsterSpawnData> MonsterSpawnDatas;
    public List<SerializableKeyValuePair<int, Vector2Wrapper>> NPCSpawnDatas;

    public void GenerateMap()
    {
        // DeepBack 타일맵 데이터화
        Dictionary<string, List<Vector3Int>> deepBackTileData = new();

        for (int i = DeepBackTilemap.cellBounds.xMin; i < DeepBackTilemap.cellBounds.xMax; i++)
        {
            for (int j = DeepBackTilemap.cellBounds.yMin; j < DeepBackTilemap.cellBounds.yMax; j++)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                TileBase tileBase = DeepBackTilemap.GetTile(tilePos);

                if (tileBase != null)
                {
                    if (!deepBackTileData.ContainsKey(tileBase.name))
                        deepBackTileData.Add(tileBase.name, new List<Vector3Int>());

                    List<Vector3Int> value = deepBackTileData.GetValueOrDefault(tileBase.name);
                    value.Add(tilePos);
                    deepBackTileData[tileBase.name] = value;
                }
            }
        }

        // Back 타일맵 데이터화
        Dictionary<string, List<Vector3Int>> backTileData = new();

        for (int i = BackTilemap.cellBounds.xMin; i < BackTilemap.cellBounds.xMax; i++)
        {
            for (int j = BackTilemap.cellBounds.yMin; j < BackTilemap.cellBounds.yMax; j++)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                TileBase tileBase = BackTilemap.GetTile(tilePos);

                if (tileBase != null)
                {
                    if (!backTileData.ContainsKey(tileBase.name))
                        backTileData.Add(tileBase.name, new List<Vector3Int>());

                    List<Vector3Int> value = backTileData.GetValueOrDefault(tileBase.name);
                    value.Add(tilePos);
                    backTileData[tileBase.name] = value;
                }
            }
        }

        // Middle 타일맵 데이터화
        Dictionary<string, List<Vector3Int>> middleTileData = new();

        for (int i = MiddleTilemap.cellBounds.xMin; i < MiddleTilemap.cellBounds.xMax; i++)
        {
            for (int j = MiddleTilemap.cellBounds.yMin; j < MiddleTilemap.cellBounds.yMax; j++)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                TileBase tileBase = MiddleTilemap.GetTile(tilePos);

                if (tileBase != null)
                {
                    if (!middleTileData.ContainsKey(tileBase.name))
                        middleTileData.Add(tileBase.name, new List<Vector3Int>());

                    List<Vector3Int> value = middleTileData.GetValueOrDefault(tileBase.name);
                    value.Add(tilePos);
                    middleTileData[tileBase.name] = value;
                }
            }
        }

        // 유효한 오브젝트 Sort
        List<MapObjectBase> MapObjects = new();

        for (int i = 0; i < ObjectParentTr.childCount; i++)
        {
            MapObjectBase obj = ObjectParentTr.GetChild(i).GetComponent<MapObjectBase>();

            if (obj != null)
            {
                MapObjectTableCell cell = MapObjTb.FindTableCellWithCode(obj.TbCell.Code);

                if (cell != null)
                    MapObjects.Add(obj);
            }
        }

        // Sort된 오브젝트들 데이터화
        List<MapObjectData> objectDatas = new();

        foreach (MapObjectBase mapObject in MapObjects)
        {
            MapObjectData data = mapObject.GetMapObjectData();

            objectDatas.Add(data);
        }

        TilemapData tilemapData = new TilemapData(
            deepBackTileData, 
            backTileData,
            middleTileData);

        MapCameraData mapCameraData = new MapCameraData(
            UseCameraOffset,
            CameraOffsetMin.x,
            CameraOffsetMax.x,
            CameraOffsetMin.y,
            CameraOffsetMax.y);

        List<MonsterSpawnData> monsterSpawnDataList = new List<MonsterSpawnData>();

        for (int i = 0; i < MonsterSpawnDatas.Count; ++i)
        {
            monsterSpawnDataList.Add(MonsterSpawnDatas[i]);
        }

        // 변환된 데이터 저장
        MapData mapData = new MapData(
            Code,
            MapName,
            Brightness,
            BgmCode, BackgroundCode,
            PlayerDefaultTr.localPosition.x,
            PlayerDefaultTr.localPosition.y,
            tilemapData,
            mapCameraData,
            objectDatas,
            monsterSpawnDataList,
            NPCSpawnDatas);

        DataManager.SaveData(mapData, "Map_" + mapData.Code, DataManager.DefaultMapDataPath);
    }
}
