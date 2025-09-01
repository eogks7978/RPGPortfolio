using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Items/Inventory")]
public class InventoryTableCell : ScriptableObject
{
    [SerializeField] private VoidEvent onInventoryItemsUpdated = null;
    [SerializeField] private EtcItem testEtcItem;
    [SerializeField] private ConsumableItem testItemSlotConsumable;
    [SerializeField] private EquipmentItem testItemSlotEquipment;

    public ItemContainer ItemContainer { get; } = new ItemContainer(100);

    public void OnEnable() => EventHandler.OnItemsUpdated += onInventoryItemsUpdated.Raise;

    public void OnDisable() => EventHandler.OnItemsUpdated -= onInventoryItemsUpdated.Raise;

    public void Initialize(InventoryData data)
    {
        ItemContainer.Initialize(data);
    }

#if UNITY_EDITOR
    [ContextMenu("Test Etc Add")]
    public void TestEtcAdd()
    {
        ItemContainer.AddItem(new EtcItem(testEtcItem.Code, testEtcItem.Quantity));
    }
    [ContextMenu("Test Consumable Add")]
    public void TestConsumableAdd()
    {
        ItemContainer.AddItem(new ConsumableItem(testItemSlotConsumable.Code, testItemSlotConsumable.Quantity, false, 0));
    }
    [ContextMenu("Test Equipment Add")]
    public void TestEquipmentAdd()
    {
        ItemContainer.AddItem(new EquipmentItem(testItemSlotEquipment.Code, testItemSlotEquipment.Quantity, testItemSlotEquipment.Level, null, false));
    }
#endif
}

