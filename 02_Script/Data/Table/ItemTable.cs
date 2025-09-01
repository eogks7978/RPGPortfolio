using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemTable", menuName = "Scriptable Object/ItemTable", order = int.MaxValue)]
public class ItemTable : ScriptableObject
{
    public List<InventoryItemTableCell> ItemList;

    public InventoryItemTableCell FindTableCellWithCode(int code)
    {
        if (code == 0)
            return null;

        foreach (var cell in ItemList)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }
}
