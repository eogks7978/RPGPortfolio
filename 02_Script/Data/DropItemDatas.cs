using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 드랍된 아이템 데이터
/// </summary>
[Serializable]
public class DropItemDatas
{
    [Header("드랍 아이템 리스트")] public List<DropItemData> data;
}

/// <summary>
/// 드랍된 아이템 정보
/// </summary>
[Serializable]
public class DropItemData
{
    [Header("드랍 아이템의 TableCell")] public InventoryItemTableCell ItemTbCell;
    [Header("드랍 확률")] public float DropRate;
    [Header("최대 드랍 개수")] public int MaxDropCount;
}