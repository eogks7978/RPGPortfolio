using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConsumableItem : Item
{
    public bool IsSlotted;
    public int SlotIndex;

    public ConsumableItem(
        int code,
        int quantity,
        bool isSlotted = false,
        int slotIndex = 0
        ) : base(code, quantity)
    {
        Code = code;
        Quantity = quantity;
        IsSlotted = isSlotted;
        SlotIndex = slotIndex;
    }
}
