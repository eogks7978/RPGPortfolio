using System;
using UnityEngine;

[Serializable]
public class InventoryItemTableCell : ScriptableObject
{
    public enum ItemType
    {
        None, Equipment, Consumable, Etc
    }
    public ItemType _ItemType;

    public int Code;
    [Header("Basic Info")]
    public string Name = "Item Name";
    public string Description;
    public Sprite Icon = null;

    [Header("Item Data")]
    public Rarity Rarity = null;
    public int SellPrice = 1;
    public int BuyPrice = 1;
    public int MaxStack = 1;

    public string ColouredName
    {
        get
        {
            string hexColour = ColorUtility.ToHtmlStringRGB(Rarity.TextColour);
            return $"<color=#{hexColour}>{Name}</color>";
        }
    }
}

