using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵의 배치되는 모든 오브젝트들의 부모 클래스
/// </summary>
public class MapObjectBase : MonoBehaviour
{
    [Tooltip("오브젝트 TableCell")] public MapObjectTableCell TbCell;

    public virtual MapObjectData GetMapObjectData()
    {
        return new MapObjectData(
            TbCell.Code,
            transform.position.x,
            transform.position.y,
            null);
    }

    public virtual void SetMapObjectData(MapObjectData data)
    {
        transform.position = new Vector3(data.PosX, data.PosY);
    }
}
