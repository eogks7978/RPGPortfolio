using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 관리자
/// </summary>
public class ObjectManager : MonoBehaviour
{
    #region singleton
    private static ObjectManager instance;
    public static ObjectManager Instance => instance;
    #endregion

    [SerializeField] private List<SerializableKeyValuePair<GameObject, int>> objectsToPool;
    public List<SerializableKeyValuePair<GameObject, int>> ObjectsToPool => objectsToPool;

    [SerializeField] private List<SerializableKeyValuePair<GameObject, int>> objectsToSubPool;
    public List<SerializableKeyValuePair<GameObject, int>> ObjectsToSubPool => objectsToSubPool;

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

    public void Initialize()
    {
        CreateOtherObjectPool();
    }

    /// <summary>
    /// 게임 진행에 필요한 기타 오브젝트들을 서브풀에 추가 저장
    /// </summary>
    private void CreateOtherObjectPool()
    {
        List<ObjectPool> mainPool = new();

        foreach(var obj in objectsToPool)
        {
            mainPool.Add(new ObjectPool(obj.key, obj.value, 50, true));
        }

        ObjectPoolManager.Instance.AddMainPool(mainPool);

        List<ObjectPool> subPool = new();

        foreach (var obj in objectsToSubPool)
        {
            subPool.Add(new ObjectPool(obj.key, obj.value, 50, true));
        }

        ObjectPoolManager.Instance.AddSubPool(subPool);
    }

    public SerializableKeyValuePair<GameObject, int> FindPoolInEtcMainObjectToPool(Type type)
    {
        return ObjectsToPool.Find(n => n.key.GetComponent(type) != null);
    }
}
