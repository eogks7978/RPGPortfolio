using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItemData
{
    public int Code;
    public string Name;
    public string Description;
    public Sprite Icon;

    public Rarity Rarity;
    public int SellPrice;
    public int MaxStack;

    public InventoryItemData(InventoryItemTableCell tbCell)
    {
        Code = tbCell.Code;
        Name = tbCell.Name;
        Description = tbCell.Description;
        Icon = tbCell.Icon;
        Rarity = tbCell.Rarity;
        SellPrice = tbCell.SellPrice;
        MaxStack = tbCell.MaxStack;
    }
}
