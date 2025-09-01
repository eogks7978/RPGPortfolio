using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCTable", menuName = "Scriptable Object/NPCTable", order = int.MaxValue)]
public class NPCTable : ScriptableObject
{
    public List<NPCTableCell> List;

    public NPCTableCell FindTableCellWithCode(int code)
    {
        foreach (var cell in List)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }

    public NPCTableCell FindTableCellWithName(string name)
    {
        string processedName = string.Copy(name).Replace("(Clone)", "");

        foreach (var cell in List)
        {
            if (cell.Prefab.name == processedName)
                return cell;
        }
        return null;
    }
}
