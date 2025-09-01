using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingTableCell", menuName = "Scriptable Object/CraftingTableCell")]
public class CraftingTableCell : ScriptableObject
{
    public int Code;
    public int ResultItemCode;
    public List<SerializableKeyValuePair<int, int>> MaterialItemCodeAndRequiredNumber;
}
