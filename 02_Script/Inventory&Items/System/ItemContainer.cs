using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ItemContainer : IItemContainer
{
    private List<Item> items = new();

    public ItemContainer(int size) => items = new(new Item[size]);

    public Item GetSlotByIndex(int index) => items[index];

    public List<Item> Items => items;

    public void Initialize(InventoryData data)
    {
        for (int i = 0; i < data.ItemList.Count; ++i)
        {
            // JSON으로 불러왔을 때 대응
            JObject jObj = data.ItemList[i] as JObject;

            Item item = jObj != null
                ? jObj.ToObject<Item>()
                : data.ItemList[i] as Item;

            if (item == null)
                return;

            switch (item.ItemType)
            {
                case InventoryItemTableCell.ItemType.Equipment:
                    EquipmentItem equipmentItem = jObj != null
                        ? jObj.ToObject<EquipmentItem>()
                        : data.ItemList[i] as EquipmentItem;
                    items[i] = equipmentItem;
                    if (equipmentItem.IsEquiped)
                        InventoryManager.Instance.SetEquipment(equipmentItem);
                    break;
                case InventoryItemTableCell.ItemType.Consumable:
                    ConsumableItem consumableItem = jObj != null
                        ? jObj.ToObject<ConsumableItem>()
                        : data.ItemList[i] as ConsumableItem;
                    items[i] = consumableItem;
                    if (consumableItem.IsSlotted)
                        UIManager.Instance.ItemHotbar.Add(consumableItem, consumableItem.SlotIndex);
                    break;
                case InventoryItemTableCell.ItemType.Etc:
                    EtcItem etcItem = jObj != null
                        ? jObj.ToObject<EtcItem>()
                        : data.ItemList[i] as EtcItem;
                    items[i] = etcItem;
                    break;
            }
        }
    }

    public Item AddItem(Item newItem, Action<bool> successAdd = null)
    {
        InventoryItemTableCell newItemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(newItem.Code);

        if (newItemTbCell == null)
            return null;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
                continue;

            InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(items[i].Code);

            if (itemTbCell != null)
            {
                if (itemTbCell == newItemTbCell)
                {
                    int slotRemainingSpace = itemTbCell.MaxStack - items[i].Quantity;

                    if (newItem.Quantity <= slotRemainingSpace)
                    {
                        items[i].Quantity += newItem.Quantity;
                        successAdd?.Invoke(true);
                        EventHandler.OnItemsUpdated.Invoke();
                        return newItem;
                    }
                    else if (slotRemainingSpace > 0)
                    {
                        items[i].Quantity += slotRemainingSpace;
                        newItem.Quantity -= slotRemainingSpace;
                    }
                }
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                if (newItem.Quantity <= newItemTbCell.MaxStack)
                {
                    items[i] = newItem;

                    successAdd?.Invoke(true);
                    EventHandler.OnItemsUpdated.Invoke();
                    return newItem;
                }
                else
                {
                    items[i] = new Item(newItem.Code, newItemTbCell.MaxStack);
                    newItem.Quantity -= newItemTbCell.MaxStack;
                }
            }
        }

        successAdd?.Invoke(false);
        EventHandler.OnItemsUpdated.Invoke();
        return newItem;
    }

    public void RemoveItem(Item item, int count)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                if (items[i] == item)
                {
                    int resultValue = items[i].Quantity - count;
                    if (resultValue <= 0)
                    {
                        items[i] = null;
                        EventHandler.OnItemsUpdated.Invoke();
                        return;
                    }

                    items[i].Quantity = resultValue;

                    EventHandler.OnItemsUpdated.Invoke();
                }
            }
        }
    }

    public void RemoveAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex > items.Count - 1)
            return;

        items[slotIndex] = null;

        EventHandler.OnItemsUpdated.Invoke();
    }

    public bool HasItem(Item newItem)
    {
        if (newItem == null)
            return false;

        InventoryItemTableCell newItemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(newItem.Code);

        foreach (Item item in items)
        {
            if (item == null)
                continue;

            InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);

            if (itemTbCell == newItemTbCell)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 아이템에 접근해서 받아오기. Get 메서드와 비슷.
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public List<Item> GetItem(int itemCode)
    {
        List<Item> resultItems = new();
        foreach (Item item in items)
        {
            if (item == null)
                continue;

            InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);

            if (itemTbCell.Code == itemCode)
                resultItems.Add(item);
        }

        return resultItems;
    }

    public int GetTotalQuantity(Item item)
    {
        InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);

        int totalCount = 0;

        foreach (Item currentItem in items)
        {
            if (currentItem == null)
                continue;

            InventoryItemTableCell currentItemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(currentItem.Code);

            if (currentItemTbCell == itemTbCell)
            {
                totalCount += currentItem.Quantity;
            }
        }

        return totalCount;
    }

    public void SortItems()
    {
        items = items
        .OrderBy(n => n == null)         // null 값은 마지막으로 보내기
        .ThenBy(n => n?.Code)          // null이 아닌 값들은 길이에 따라 오름차순 정렬
        .ToList();

        EventHandler.OnItemsUpdated.Invoke();
    }

    public bool isFull()
    {
        foreach (Item item in items)
        {
            if (item == null)
                return false;
        }

        return true;
    }
}

