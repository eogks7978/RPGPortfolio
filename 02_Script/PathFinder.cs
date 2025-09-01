using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

/// <summary>
/// https://assetstore.unity.com/packages/tools/behavior-ai/ultimate-a-pathfinding-solution-224082
/// </summary>
public class PathFinder : MonoBehaviour
{
    #region singleton
    private static PathFinder instance;
    public static PathFinder Instance => instance;
    #endregion

    [SerializeField] float gridSize = 0.5f; //increase patience or gridSize for larger maps
    [SerializeField] LayerMask obstacles;
    [SerializeField] bool searchShortcut = false;
    [SerializeField] bool snapToGrid = false;

    [Tooltip("최대 루프")]
    private int _maxCalculator;
    [Tooltip("인접 지점 좌표 구하는데 필요한 정보들. 넣은 메서드에 따라 4방향 이상의 길찾기 가능할 것으로 보임.")]
    private Func<Vector2, Vector2, float> _HeuristicDistance;
    private Func<Vector2, Dictionary<Vector2, float>> _ConnectedNodesAndStepCosts;

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
        _HeuristicDistance = GetDistance;
        _ConnectedNodesAndStepCosts = GetNeighbourNodes;
        _maxCalculator = 1000;
    }

    public bool GenerateAstarPath(Vector2 startNode, Vector2 targetNode, out List<Vector2> path)
    {
        float num = _HeuristicDistance(startNode, targetNode);
        int num2 = _maxCalculator;
        HashSet<Vector2> hashSet = new HashSet<Vector2>();
        Dictionary<Vector2, pathData> dictionary = new Dictionary<Vector2, pathData> {
        {
            startNode,
            new pathData
            {
                g = 0f,
                h = num,
                f = num
            }
        } };
        Dictionary<Vector2, Vector2> dictionary2 = new Dictionary<Vector2, Vector2>();
        while (num2 > 0)
        {
            num2--;
            if (dictionary.Count == 0)
            {
                break;
            }

            Vector2 key = dictionary.Aggregate((KeyValuePair<Vector2, pathData> l, KeyValuePair<Vector2, pathData> r) => (l.Value.f < r.Value.f) ? l : r).Key;
            pathData pathData = dictionary[key];
            dictionary.Remove(key);
            hashSet.Add(key);
            if (key.Equals(targetNode))
            {
                List<Vector2> list = new List<Vector2>();
                Vector2 val = key;
                while (!val.Equals(startNode))
                {
                    list.Add(val);
                    val = dictionary2[val];
                }

                list.Reverse();

                if (searchShortcut && list.Count > 0)
                    list = ShortenPath(list);
                else
                {
                    if (!snapToGrid) list.Add(targetNode);
                }

                path = list;
                return true;
            }

            foreach (KeyValuePair<Vector2, float> item in _ConnectedNodesAndStepCosts(key))
            {
                if (Enumerable.Contains(hashSet, item.Key))
                {
                    continue;
                }

                float num3 = item.Value + pathData.g;
                if (!dictionary.ContainsKey(item.Key) || dictionary[item.Key].g > num3)
                {
                    float num4 = _HeuristicDistance(item.Key, targetNode);
                    float f = num4 + num3;
                    dictionary2[item.Key] = key;
                    if (!dictionary.ContainsKey(item.Key))
                    {
                        dictionary.Add(item.Key, new pathData
                        {
                            g = num3,
                            h = num4,
                            f = f
                        });
                    }
                    else
                    {
                        dictionary[item.Key] = new pathData
                        {
                            g = num3,
                            h = num4,
                            f = f
                        };
                    }
                }
            }
        }

        path = new List<Vector2>();
        return false;
    }

    float GetDistance(Vector2 A, Vector2 B)
    {
        return (A - B).sqrMagnitude;
    }

    Dictionary<Vector2, float> GetNeighbourNodes(Vector2 pos)
    {
        Dictionary<Vector2, float> neighbours = new Dictionary<Vector2, float>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0) continue;

                Vector2 dir = new Vector2(i, j) * gridSize;
                if (!Physics2D.Linecast(pos, pos + dir, obstacles))
                {
                    neighbours.Add(GetClosestNode(pos + dir), dir.magnitude);
                }
            }

        }
        return neighbours;
    }

    public Vector2 GetClosestNode(Vector2 target)
    {
        return new Vector2(Mathf.Round(target.x / gridSize) * gridSize, Mathf.Round(target.y / gridSize) * gridSize);
    }

    public List<Vector2> ShortenPath(List<Vector2> path)
    {
        List<Vector2> newPath = new List<Vector2>();

        for (int i = 0; i < path.Count; i++)
        {
            newPath.Add(path[i]);
            for (int j = path.Count - 1; j > i; j--)
            {
                if (!Physics2D.Linecast(path[i], path[j], obstacles))
                {

                    i = j;
                    break;
                }
            }
            newPath.Add(path[i]);
        }
        newPath.Add(path[path.Count - 1]);
        return newPath;
    }
}

struct pathData
{
    public float g;

    public float h;

    public float f;
}