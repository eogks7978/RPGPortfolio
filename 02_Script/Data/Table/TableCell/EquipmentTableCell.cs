using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentTableCell", menuName = "Scriptable Object/EquipmentTableCell", order = int.MaxValue)]
public class EquipmentTableCell : InventoryItemTableCell
{
    public enum EquipmentType
    {
        Weapon, Defensive
    }
    public EquipmentType equipmentType;

    public enum EquipmentTag
    {
        Sword, Bow, Wand, Helmet, Armor, Pants, Boots, Ring, Neckless
    }
    public EquipmentTag equipmentTag;

    public Sprite ItemSprite;
    public Vector3 OriginalRotation;

    [Header("Stat Data")]
    public EquipmentStat Stat;

    [Header("Enhance Data")]
    public EquipmentData EnhanceData;
}
