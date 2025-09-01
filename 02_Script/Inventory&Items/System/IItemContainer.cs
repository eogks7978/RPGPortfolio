using System;

public interface IItemContainer
{
    Item AddItem(Item itemSlot, Action<bool> successAdd = null);
    void RemoveItem(Item itemSlot, int count);
    void RemoveAt(int slotIndex);
    bool HasItem(Item item);
    int GetTotalQuantity(Item item);
}