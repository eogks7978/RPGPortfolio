using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    #region singleton
    private static MapManager instance;
    public static MapManager Instance => instance;
    #endregion

    [SerializeField, Tooltip("현재 플레이어가 있는 맵")] private Map currentMap;
    [SerializeField, Header("한 번 이상 방문한 맵(캐시)")] private List<Map> visitedMap;
    [SerializeField, Header("맵 생성을 위한 프리펩 뼈대")] private GameObject mapBasePrefab32X32;
    [SerializeField, Header("타일맵 뒤에 랜더링 될 배경 관리자")] private BackgroundManager scrollingBackground;
    [SerializeField] private MiniMapManager miniMap;

    public bool MapLoadDone;

    private void Awake()
    {
        //if (instance == null)
        //{
        instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
    }

    public Map CurrentMap { get => currentMap; }
    public List<Map> VisitedMap { get => visitedMap; }

    public void ChangeMap(int mapCode, Vector3 playerPosition)
    {
        MapLoadDone = false;
        UIManager.Instance.OpenLoadingPannel();

        string mapName = DataManager.Instance.TableDataTable.MapTables
            .FindTableCellWithCode(mapCode).Data.name;

        MapData mapData = DataManager.LoadData<MapData>(mapName);

        // 맵 최초 로딩 시 디폴트 데이터 저장.
        if (mapData == null)
        {
            MapData defaultMapData = DataManager.ParseData<MapData>(
                DataManager.Instance.TableDataTable.MapTables.FindTableCellWithCode(mapCode).Data.text);

            if (defaultMapData == null)
                return;

            mapData = defaultMapData;

            DataManager.SaveData(mapData, mapName);
        }

        // 이전 맵 설정
        if (currentMap)
        {
            currentMap.ResetMonsters();
            currentMap.ResetNPC();
            currentMap.gameObject.SetActive(false);
        }

        // 이미 로딩된 맵이면 SetActive 후 공통세팅만 실행.
        foreach (var map in visitedMap)
        {
            if (mapData.Code == map.MapData.Code)
            {
                map.gameObject.SetActive(true);
                CommonSetting(map);
                return;
            }
        }
        
        /////// 실행 후 첫 로딩 ///////

        // 맵 틀 생성
        Map newMap = Instantiate(mapBasePrefab32X32).GetComponent<Map>();
        newMap.transform.position = Vector3.zero;
        newMap.SetMapData(mapData);

        // 타일맵 생성
        foreach (var tileData in mapData.TMData.DeepBackTileData)
        {
            TileBase tileBase =
                DataManager.Instance.TableDataTable.TilemapTable.FindWithName(tileData.Key);
            foreach (var positionData in tileData.Value)
            {
                newMap.DeepBackTilemap.SetTile(positionData, tileBase);
            }
        }
        foreach (var tileData in mapData.TMData.BackTileData)
        {
            TileBase tileBase =
                DataManager.Instance.TableDataTable.TilemapTable.FindWithName(tileData.Key);
            foreach (var positionData in tileData.Value)
            {
                newMap.BackTilemap.SetTile(positionData, tileBase);
            }
        }
        foreach (var tileData in mapData.TMData.MiddleTileData)
        {
            TileBase tileBase =
                DataManager.Instance.TableDataTable.TilemapTable.FindWithName(tileData.Key);
            foreach (var positionData in tileData.Value)
            {
                newMap.MiddleTilemap.SetTile(positionData, tileBase);
            }
        }

        // 맵 오브젝트 생성
        foreach (var data in mapData.MapObjectDatas)
        {
            if (data != null)
            {
                var objCell = DataManager.Instance.TableDataTable.MapObjectTables.
                    FindTableCellWithCode(data.Code);

                GameObject obj = Instantiate(objCell.Prefab);
                obj.transform.SetParent(newMap.MapObjTr);
                obj.transform.localPosition = new Vector3(data.PosX, data.PosY, 0f);
                obj.GetComponent<MapObjectBase>().SetMapObjectData(data);
            }
        }

        // 플레이어 디폴트 포지션 세팅
        newMap.SetPlayerDefaultPos(
            mapData.PlayerDefaultPositionX,
            mapData.PlayerDefaultPositionY);

        // 카메라 오프셋 세팅
        newMap.SetMapCameraOffset(
            mapData.MCData.CameraOffsetMinX,
            mapData.MCData.CameraOffsetMaxX,
            mapData.MCData.CameraOffsetMinY,
            mapData.MCData.CameraOffsetMaxY);

        visitedMap.Add(newMap);

        CommonSetting(newMap);

        void CommonSetting(Map map)
        {
            currentMap = map;

            // 패스파인더 세팅
            //PathFinder.Instance.UpdatePathFinder(map.BackTilemap.cellBounds.size);

            // 백그라운드 세팅
            scrollingBackground.SetScrollingBackground(mapData.MCData.UseCameraOffset 
                ? DataManager.Instance.TableDataTable.BackgroundTable.FindTableCellWithCode(
                    mapData.BackgroundCode).Sprite
                : null);

            // 미니맵 세팅
            miniMap.SetMiniMap(PlaySceneMain.Instance.Player.transform, mapData.MapName);

            // 빛 조정
            LightController.Instance.ChangeLight(mapData.Brightness);

            // 카메라 세팅
            Vector2 mapSize = map.GetMapSize();
            CameraController.Instance.ChangeTarget(
                PlaySceneMain.Instance.Player.transform,
                map.BackTilemap.transform.position.x + mapData.MCData.CameraOffsetMinX,
                map.BackTilemap.transform.position.x + mapSize.x
                + mapData.MCData.CameraOffsetMaxX,
                map.BackTilemap.transform.position.y + mapData.MCData.CameraOffsetMinY,
                map.BackTilemap.transform.position.y + mapSize.y
                + mapData.MCData.CameraOffsetMaxY);

            // 배경음 설정
            SoundController.Instance.PlayBgm(mapData.BgmCode);

            // 오브젝트풀 생성
            if (mapData.MonsterSpawnData != null)
            {
                List<ObjectPool> monsterPools = new();
                foreach (MonsterSpawnData data in mapData.MonsterSpawnData)
                {
                    MonsterTableCell tbCell =
                    DataManager.Instance.TableDataTable.MonsterTable.FindTableCellWithCode(data.MonsterCode);

                    monsterPools.Add(new ObjectPool(tbCell.Prefab.gameObject, 7, 50, true));
                }
                ObjectPoolManager.Instance.AddMainPool(monsterPools);
            }
            if (mapData.NPCSpawnData != null)
            {
                List<ObjectPool> npcPool = new();
                foreach (var data in mapData.NPCSpawnData)
                {
                    NPCTableCell tbCell =
                        DataManager.Instance.TableDataTable.NPCTable.FindTableCellWithCode(data.key);

                    npcPool.Add(new ObjectPool(tbCell.Prefab.gameObject, 2, 50, true));
                }
                ObjectPoolManager.Instance.AddMainPool(npcPool);
            }

            // NPC, 몬스터 스폰
            map.SpawnAllNPC();
            map.SpawnAllMonsters();

            // 플레이어 위치 세팅
            PlaySceneMain.Instance.Player.transform.position 
                = currentMap.transform.position + playerPosition;

            // 플레이어 카메라 즉시 타겟
            CameraController.Instance.ImmediatelyShowTarget(
                PlaySceneMain.Instance.Player.transform);

            MapLoadDone = true;
            EventHandler.OnMapChanged?.Invoke();
            UIManager.Instance.OpenMapNamePopup(mapData.MapName);
        }
    }
}
