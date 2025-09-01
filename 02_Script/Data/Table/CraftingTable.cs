using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingTable", menuName = "Scriptable Object/CraftingTable", order = int.MaxValue)]
public class CraftingTable : ScriptableObject
{
    public List<CraftingTableCell> List;
}
