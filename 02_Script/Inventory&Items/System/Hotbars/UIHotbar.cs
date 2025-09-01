using System;
using UnityEngine;

public class UIHotbar : MonoBehaviour
{
    [SerializeField] private HotbarSlot[] hotbarSlots = new HotbarSlot[6];

    public HotbarSlot[] HotbarSlots => hotbarSlots;

    public void Add(Item itemToAdd, int index)
    {
        if (index < 0 || index >= HotbarSlots.Length)
            return;

        ConsumableItem consumableItem = itemToAdd as ConsumableItem;

        if (consumableItem == null)
            return;

        HotbarSlots[index].AddItem(itemToAdd, index);
    }

    public void Remove(Item itemToDecrease)
    {
        ConsumableItem consumableItem = itemToDecrease as ConsumableItem;

        if (consumableItem == null)
            return;

        for (int i = 0; i < hotbarSlots.Length; ++i)
        {
            if (itemToDecrease == hotbarSlots[i].SlotItem)
            {
                hotbarSlots[i].RemoveItem();
            }
        }
    }

    public bool isExistItem(Item item)
    {
        ConsumableItem consumableItem = item as ConsumableItem;

        for(int i = 0; i < hotbarSlots.Length; ++i)
        {
            if (hotbarSlots[i] != null && hotbarSlots[i].SlotItem == consumableItem)
                return true;
        }

        return false;
    }
}
