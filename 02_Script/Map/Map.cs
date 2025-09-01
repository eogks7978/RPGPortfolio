using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 맵 클래스
/// </summary>
public class Map : MonoBehaviour
{
    [SerializeField] private Tilemap deepBackTilemap;
    [SerializeField] private Tilemap backTilemap;
    [SerializeField] private Tilemap middleTilemap;
    [SerializeField, Header("맵 오브젝트들의 부모가 될 Transform")] private Transform mapObjTr;
    [SerializeField, Header("플레이어 디폴트 포지션 Transform")] private Transform playerDefaultPosTr;
    [SerializeField, Header("맵 데이터")] private MapData mapData;

    [SerializeField] private float cameraOffsetMinX;
    [SerializeField] private float cameraOffsetMaxX;
    [SerializeField] private float cameraOffsetMinY;
    [SerializeField] private float cameraOffsetMaxY;

    [SerializeField, Header("스폰 된 NPC")] private List<NPC> managedNPC;
    [SerializeField, Header("스폰 된 Monster")] private List<Monster> managedMonsters;
    [SerializeField, Header("몬스터 디폴트 리스폰 타임")] private float normalMonsterRespawnTime;

    public Tilemap DeepBackTilemap { get => deepBackTilemap; }
    public Tilemap BackTilemap { get => backTilemap; }
    public Tilemap MiddleTilemap { get => middleTilemap; }
    public Transform MapObjTr { get => mapObjTr; }
    public MapData MapData { get => mapData; }
    public Transform PlayerDefaultPosTr { get => playerDefaultPosTr; }
    public float CameraOffsetMinX { get => cameraOffsetMinX; }
    public float CameraOffsetMaxX { get => cameraOffsetMaxX; }
    public float CameraOffsetMinY { get => cameraOffsetMinY; }
    public float CameraOffsetMaxY { get => cameraOffsetMaxY; }
    public List<Monster> ManagedMonsters { get => managedMonsters; }

    private void OnEnable()
    {
        EventHandler.OnMonsterDead += OnMonsterDie;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        EventHandler.OnMonsterDead -= OnMonsterDie;
    }

    public void SetMapData(MapData mapData)
    {
        this.mapData = mapData;
    }

    public void SetPlayerDefaultPos(float posX, float posY)
    {
        playerDefaultPosTr.localPosition = new Vector3(posX, posY, 0);
    }

    public Vector2 GetMapSize()
    {
        Vector2 result = new Vector2(BackTilemap.size.x, BackTilemap.size.y);

        if (DeepBackTilemap.size.x > result.x)
            result.x = DeepBackTilemap.size.x;
        if (DeepBackTilemap.size.y > result.y)
            result.y = DeepBackTilemap.size.y; 
        if (middleTilemap.size.x > result.x)
            result.x = middleTilemap.size.x; 
        if (middleTilemap.size.y > result.y)
            result.y = middleTilemap.size.y;

        return result;
    }

    public void SetMapCameraOffset(
        float cameraOffsetMinX,
        float cameraOffsetMaxX,
        float cameraOffsetMinY,
        float cameraOffsetMaxY)
    {
        this.cameraOffsetMinX = cameraOffsetMinX;
        this.cameraOffsetMaxX = cameraOffsetMaxX;
        this.cameraOffsetMinY = cameraOffsetMinY;
        this.cameraOffsetMaxY = cameraOffsetMaxY;
    }

    public void SpawnAllNPC()
    {
        if (mapData.NPCSpawnData == null)
            return;

        for (int i = 0; i < mapData.NPCSpawnData.Count; ++i)
        {
            var NPCData = mapData.NPCSpawnData[i];
            NPCTableCell tbCell = DataManager.Instance.TableDataTable.NPCTable.FindTableCellWithCode(
                NPCData.key);
            string npcName = tbCell.Prefab.name;

            if (NPCData != null)
            {
                SpawnNPC(npcName, new Vector2(NPCData.value.X, NPCData.value.Y), tbCell.NPCName);
            }
        }
    }

    public void SpawnAllMonsters()
    {
        if (mapData.MonsterSpawnData == null)
            return;

        for (int i = 0; i < mapData.MonsterSpawnData.Count; ++i)
        {
            MonsterSpawnData monsterSpawnData = mapData.MonsterSpawnData[i];
            string monsterName = DataManager.Instance.TableDataTable.MonsterTable.FindTableCellWithCode(
                monsterSpawnData.MonsterCode).Prefab.name;

            if (monsterSpawnData != null)
            {
                foreach (Vector2Wrapper pos in mapData.MonsterSpawnData[i].MonsterSpawnPos)
                {
                    SpawnMonster(monsterName, new Vector2(pos.X, pos.Y));
                }
            }
        }
    }

    public void SpawnNPC(string prefabName, Vector2 spawnPos, string npcName)
    {
        GameObject npcObj = ObjectPoolManager.Instance.GetObjectInMainPool(prefabName);

        NPC npc = npcObj.GetComponent<NPC>();
        managedNPC.Add(npc);
        npc.Spawn(spawnPos, npcName);
    }

    public void SpawnMonster(string monsterName, Vector2 spawnPos)
    {
        GameObject mosnterObj = ObjectPoolManager.Instance.GetObjectInMainPool(monsterName);

        LongRangeMonster longRangeMonster = mosnterObj.GetComponent<LongRangeMonster>();
        Monster monster = longRangeMonster != null ? longRangeMonster : mosnterObj.GetComponent<Monster>();

        managedMonsters.Add(longRangeMonster ? longRangeMonster : monster);
        monster.Spawn(spawnPos);
    }

    public void ResetMonsters()
    {
        for (int i = 0; i < managedMonsters.Count; i++)
        {
            ObjectPoolManager.Instance.ReturnObjectToMainPool(managedMonsters[i].gameObject);
        }
        managedMonsters.Clear();
    }

    public void ResetNPC()
    {
        for (int i = 0; i < managedNPC.Count; i++)
        {
            ObjectPoolManager.Instance.ReturnObjectToMainPool(managedNPC[i].gameObject);
        }
        managedNPC.Clear();
    }

    private void OnMonsterDie(Monster monster)
    {
        StartCoroutine(DelaySpawn(
            monster, monster.OriginSpawnPos));
    }

    private void DeleteMonster(Monster monster)
    {
        ObjectPoolManager.Instance.ReturnObjectToMainPool(monster.gameObject);
        managedMonsters.Remove(monster);
    }

    private IEnumerator DelaySpawn(Monster monster, Vector2 spawnPos)
    {
        yield return new WaitForSeconds(3f);

        DeleteMonster(monster);

        yield return new WaitForSeconds(normalMonsterRespawnTime);

        SpawnMonster(monster.name.Replace("(Clone)", ""), spawnPos);
    }
}
