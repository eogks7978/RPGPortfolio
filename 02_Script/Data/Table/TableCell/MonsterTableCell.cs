using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTableCell", menuName = "Scriptable Object/MonsterTableCell", order = int.MaxValue)]
public class MonsterTableCell : ScriptableObject
{
    public int Code;
    public string MonsterName;
    public GameObject Prefab;
    public DropItemDatas DropItemData;
}
