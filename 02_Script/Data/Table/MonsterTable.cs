using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTable", menuName = "Scriptable Object/MonsterTable", order = int.MaxValue)]
public class MonsterTable : ScriptableObject
{
    public List<MonsterTableCell> List;

    public MonsterTableCell FindTableCellWithCode(int code)
    {
        foreach (var cell in List)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }

    public MonsterTableCell FindTableCellWithName(string name)
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
