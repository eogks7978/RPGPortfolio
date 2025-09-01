using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectTableCell", menuName = "Scriptable Object/ObjectTableCell", order = int.MaxValue)]
public class MapObjectTableCell : ScriptableObject
{
    public int Code;
    public GameObject Prefab;
}
