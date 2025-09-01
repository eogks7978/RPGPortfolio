using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인게임 아이템들의 최상단(부모) 클래스
/// </summary>
public abstract class InventoryItem : MonoBehaviour
{
    /// <summary>
    /// 사용시 할 일
    /// </summary>
    /// <param name="cbDone">사용 후 콜백</param>
    public virtual void UseItem(Action<bool> cbDone = null)
    {

    }
}
