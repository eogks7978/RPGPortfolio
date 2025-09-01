using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 오브젝트 풀 관리자
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    #region singleton
    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance => instance;
    #endregion

    [SerializeField, Tooltip("메인 풀")] private List<ObjectPool> mainPools;
    [SerializeField, Tooltip("인스턴스화 된 메인 풀")] private Dictionary<string, Queue<GameObject>> mainPoolDictionary;

    [SerializeField, Tooltip("서브 풀")] private List<ObjectPool> subPools;
    [SerializeField, Tooltip("인스턴스화 된 서브 풀")] private Dictionary<string, Queue<GameObject>> subPoolDictionary;

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
        Initialize();
    }

    private void Initialize()
    {
        mainPoolDictionary = new();
        mainPools = new();
        subPoolDictionary = new();
        subPools = new();
    }

    public void AddMainPool(List<ObjectPool> pool)
    {
        if (mainPools.Count == 0)
            mainPools = new List<ObjectPool>(pool);

        foreach (ObjectPool p in mainPools)
        {
            if (mainPools.Find(n => n.prefab.name == p.prefab.name) == null)
                mainPools.Add(p);
        }

        foreach (var p in pool)
        {
            if (!mainPoolDictionary.ContainsKey(p.prefab.name))
            {
                Queue<GameObject> objectQueue = new Queue<GameObject>();

                for (int i = 0; i < p.initialSize; i++)
                {
                    GameObject obj = Instantiate(p.prefab);
                    obj.SetActive(false);
                    obj.transform.SetParent(transform);
                    obj.transform.localPosition = Vector3.zero;
                    objectQueue.Enqueue(obj);
                }

                mainPoolDictionary.Add(p.prefab.name, objectQueue);
            }
        }
    }

    public void AddSubPool(List<ObjectPool> pool)
    {
        if (subPools.Count == 0)
            subPools = new List<ObjectPool>(pool);

        foreach (ObjectPool p in subPools)
        {
            if (subPools.Find(n => n.prefab.name == p.prefab.name) == null)
                subPools.Add(p);
        }

        foreach (var p in pool)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < p.initialSize; i++)
            {
                GameObject obj = Instantiate(p.prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
                objectQueue.Enqueue(obj);
            }

            if (subPoolDictionary.ContainsKey(p.prefab.name))
            {
                objectQueue = new Queue<GameObject>(subPoolDictionary[p.prefab.name].Concat(objectQueue));
                subPoolDictionary[p.prefab.name] = objectQueue;
            }
            else
                subPoolDictionary.Add(p.prefab.name, objectQueue);
        }
    }

    public bool ContainsMainPool(string name)
    {
        return mainPools.Find(n => n.prefab.name == name) != null;
    }

    public bool ContainsSubPool(string name)
    {
        return subPools.Find(n => n.prefab.name == name) != null;
    }

    public GameObject GetObjectInMainPool(string objectName)
    {
        if (mainPoolDictionary.ContainsKey(objectName))
        {
            Queue<GameObject> objectQueue = mainPoolDictionary[objectName];

            if (objectQueue.Count > 0)
            {
                GameObject obj = objectQueue.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                ObjectPool pool = mainPools.Find(p => p.prefab.name == objectName);
                if (pool != null && pool.allowExpansion && mainPoolDictionary[objectName].Count < pool.maxSize)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(true);
                    return obj;
                }
            }
        }

        return null;
    }

    public GameObject GetObjectInSubPool(string objectName)
    {
        if (subPoolDictionary.ContainsKey(objectName))
        {
            Queue<GameObject> objectQueue = subPoolDictionary[objectName];

            if (objectQueue.Count > 0)
            {
                GameObject obj = objectQueue.Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                ObjectPool pool = subPools.Find(p => p.prefab.name == objectName);
                if (pool != null && pool.allowExpansion && subPoolDictionary[objectName].Count < pool.maxSize)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(true);
                    return obj;
                }
            }
        }

        return null;
    }

    public void ReturnObjectToMainPool(GameObject obj)
    {
        obj.SetActive(false);

        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        string objectName = obj.name.Replace("(Clone)", "").Trim();

        if (mainPoolDictionary.ContainsKey(objectName))
            mainPoolDictionary[objectName].Enqueue(obj);
        else
            Destroy(obj);
    }

    public void ReturnObjectToSubPool(GameObject obj)
    {
        obj.SetActive(false);

        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        string objectName = obj.name.Replace("(Clone)", "").Trim();

        if (subPoolDictionary.ContainsKey(objectName))
            subPoolDictionary[objectName].Enqueue(obj);
        else
            Destroy(obj);
    }
}
