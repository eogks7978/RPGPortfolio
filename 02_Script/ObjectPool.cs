using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트풀에 들어갈 오브젝트의 데이터 형식
/// </summary>
[System.Serializable]
public class ObjectPool
{
    [Tooltip("오브젝트 프리펩")]
    public GameObject prefab;
    [Tooltip("초기 풀 개수")]
    public int initialSize;
    [Tooltip("최대 풀 개수")]
    public int maxSize;
    [Tooltip("풀 확장 가능 유무")]
    public bool allowExpansion = true;

    public ObjectPool(GameObject prefab,
        int initialSize, 
        int maxSize,
        bool allowExpansion)
    {
        this.prefab = prefab;
        this.initialSize = initialSize;
        this.maxSize = maxSize;
        this.allowExpansion = allowExpansion;
    }
}
