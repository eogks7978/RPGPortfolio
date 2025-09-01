using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인벤토리 데이터
/// </summary>
[Serializable]
public class InventoryData
{
    [Header("보유 돈")] public int Money;
    [Header("보유 아이템 리스트")] public List<object> ItemList;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="money">돈</param>
    /// <param name="list">아이템 리스트</param>
    public InventoryData(int money, List<object> list)
    {
        Money = money;
        ItemList = list;
    }
}