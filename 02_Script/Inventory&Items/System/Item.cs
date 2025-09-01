using System;

[Serializable]
public class Item
{
    public int Code;
    public int Quantity;
    public InventoryItemTableCell.ItemType ItemType =>
        DataManager.Instance.TableDataTable.ItemTable.
        FindTableCellWithCode(Code)._ItemType;

    public Item(int code, int quantity)
    {
        Code = code;
        Quantity = quantity;
    }

    public static Item GetDeepCopiedItemAsChildType(Item item)
    {
        InventoryItemTableCell tbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);

        Item resultItem = null;

        switch (tbCell._ItemType)
        {
            case InventoryItemTableCell.ItemType.Equipment:
                EquipmentItem equipment = item as EquipmentItem;
                if (equipment != null)
                    resultItem = new EquipmentItem(
                    equipment.Code,
                    equipment.Quantity,
                    equipment.Level,
                    equipment.BonusStat,
                    equipment.IsEquiped);
                break;
            case InventoryItemTableCell.ItemType.Consumable:
                ConsumableItem consumable = item as ConsumableItem;
                if (consumable != null)
                    resultItem = new ConsumableItem(
                    consumable.Code,
                    consumable.Quantity,
                    consumable.IsSlotted,
                    consumable.SlotIndex);
                break;
            case InventoryItemTableCell.ItemType.Etc:
                EtcItem etc = item as EtcItem;
                if (etc != null)
                    resultItem = new ConsumableItem(
                    etc.Code,
                    etc.Quantity);
                break;
        }

        return resultItem;
    }
}

