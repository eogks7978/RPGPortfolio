using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵에 갈 수 없는 곳 설정하는 오브젝트
/// </summary>
public class InvisibleWall : MapObjectBase
{
    [SerializeField] private BoxCollider2D thisCor;
    private float thisCorOffsetX;
    private float thisCorOffsetY;
    private float thisCorSizeX;
    private float thisCorSizeY;

    private void Awake()
    {
        if (thisCor == null)
            thisCor = GetComponent<BoxCollider2D>();
    }

    public override MapObjectData GetMapObjectData()
    {
        thisCorOffsetX = thisCor.offset.x;
        thisCorOffsetY = thisCor.offset.y;
        thisCorSizeX = thisCor.size.x;
        thisCorSizeY = thisCor.size.y;

        Dictionary<string, object> properties = new Dictionary<string, object>() {
            {nameof(thisCorOffsetX), thisCorOffsetX},
            {nameof(thisCorOffsetY), thisCorOffsetY},
            {nameof(thisCorSizeX), thisCorSizeX},
            {nameof(thisCorSizeY), thisCorSizeY}
        };

        MapObjectData data = new(
            TbCell.Code,
            transform.position.x,
            transform.position.y,
            properties);

        return data;
    }

    public override void SetMapObjectData(MapObjectData data)
    {
        base.SetMapObjectData(data);

        thisCor.offset = new Vector2(
            Convert.ToSingle(data.Properties[nameof(thisCorOffsetX)]),
            Convert.ToSingle(data.Properties[nameof(thisCorOffsetY)]));

        thisCor.size = new Vector2(
            Convert.ToSingle(data.Properties[nameof(thisCorSizeX)]),
            Convert.ToSingle(data.Properties[nameof(thisCorSizeY)]));
    }
}
