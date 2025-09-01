using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EquipmentItem : Item
{
    public int Level;
    public EquipmentStat BonusStat;
    public bool IsEquiped;

    public EquipmentItem(
        int code,
        int quantity,
        int level = 0,
        EquipmentStat equipmentEnhanceStat = null,
        bool isEquiped = false) : base(code, quantity)
    {
        Code = code;
        Quantity = quantity;
        Level = level;
        BonusStat = equipmentEnhanceStat;
        IsEquiped = isEquiped;
    }

    public EquipmentStat GetEquipmentStat()
    {
        InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(Code);
        EquipmentTableCell equipmentTbCell = itemTbCell as EquipmentTableCell;

        if (!equipmentTbCell)
            return null;

        EquipmentStat resultStat = new(
            equipmentTbCell.Stat.MaxHealth + (BonusStat != null ? BonusStat.MaxHealth : 0),
            equipmentTbCell.Stat.MaxMana + (BonusStat != null ? BonusStat.MaxMana : 0),
            equipmentTbCell.Stat.AttackPoint + (BonusStat != null ? BonusStat.AttackPoint : 0),
            equipmentTbCell.Stat.DefensePoint + (BonusStat != null ? BonusStat.DefensePoint : 0),
            equipmentTbCell.Stat.AttackSpeed + (BonusStat != null ? BonusStat.AttackSpeed : 0),
            equipmentTbCell.Stat.MoveSpeed + (BonusStat != null ? BonusStat.MoveSpeed : 0),
            equipmentTbCell.Stat.Strength + (BonusStat != null ? BonusStat.Strength : 0),
            equipmentTbCell.Stat.Dexterity + (BonusStat != null ? BonusStat.Dexterity : 0),
            equipmentTbCell.Stat.Constitution + (BonusStat != null ? BonusStat.Constitution : 0)
            );

        return resultStat;
    }
}
