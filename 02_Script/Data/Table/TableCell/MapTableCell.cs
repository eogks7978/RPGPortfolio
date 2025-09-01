using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapTableCell", menuName = "Scriptable Object/MapTableCell", order = int.MaxValue)]
public class MapTableCell : ScriptableObject
{
    public int Code;
    public TextAsset Data;
}
