using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCTableCell", menuName = "Scriptable Object/NPCTableCell", order = int.MaxValue)]
public class NPCTableCell : ScriptableObject
{
    public int Code;
    public GameObject Prefab;
    public string NPCName;
}
