using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapObjectTable", menuName = "Scriptable Object/MapObjectTable", order = int.MaxValue)]
public class MapObjectTable : ScriptableObject
{
    public List<MapObjectTableCell> List;

    public MapObjectTableCell FindTableCellWithCode(int code)
    {
        foreach (var cell in List)
        {
            if (cell.Code == code)
                return cell; 
        }
        return null;
    }
}
