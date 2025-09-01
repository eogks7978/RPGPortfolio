using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableTableCell", menuName = "Scriptable Object/ConsumableTableCell", order = int.MaxValue)]
public class ConsumableTableCell : InventoryItemTableCell
{
    public InventoryItem item;
}
