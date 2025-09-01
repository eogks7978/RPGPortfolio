using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapTable", menuName = "Scriptable Object/MapTable", order = int.MaxValue)]
public class MapTable : ScriptableObject
{
    public List<MapTableCell> List;

    public MapTableCell FindTableCellWithCode(int code)
    {
        foreach (var cell in List)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }
}
