using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundTable", menuName = "Scriptable Object/BackgroundTable", order = int.MaxValue)]
public class BackgroundTable : ScriptableObject
{
    public List<BackgroundTableCell> List;

    public BackgroundTableCell FindTableCellWithCode(int code)
    {
        foreach (var cell in List)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }
}
