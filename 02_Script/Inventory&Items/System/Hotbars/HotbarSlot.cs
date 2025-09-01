using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : ItemSlotUI
{
    [SerializeField] private InventoryTableCell inventory = null;
    [SerializeField] private TextMeshProUGUI itemQuantityText = null;
    [SerializeField] private Button thisButton;

    private Item slotItem = null;

    public override Item SlotItem
    {
        get { return slotItem; }
        set { slotItem = value; UpdateSlotUI(); }
    }

    private void Awake()
    {
        if (!thisButton)
            thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(UseSlotItem);
    }

    public bool AddItem(Item itemToAdd, int index)
    {
        RemoveItem();

        ConsumableItem item = itemToAdd as ConsumableItem;

        if (item != null)
        {
            item.IsSlotted = true;
            item.SlotIndex = index;
            SlotItem = item;
        }
        else
            return false;

        return true;
    }

    public void RemoveItem()
    {
        if (SlotItem == null) return;

        ConsumableItem item = SlotItem as ConsumableItem;

        if (item != null)
        {
            SlotItem = null;

            if (!UIManager.Instance.ItemHotbar.isExistItem(item))
            {
                item.IsSlotted = false;
                item.SlotIndex = 0;
            }
        }
    }

    public void UseSlotItem()
    {
        InventoryManager.Instance.UseItem(slotItem, 1);
    }

    public override void UpdateSlotUI()
    {
        if (SlotItem != null)
        {
            InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(SlotItem.Code);
            itemQuantityText.text = SlotItem.Quantity > 1 ? SlotItem.Quantity.ToString() : "";
            itemIconImage.sprite = itemTbCell.Icon;

            EnableSlotUI(true);
            SetItemQuantityUI();
        }
        else
        {
            EnableSlotUI(false);
            return;
        }
    }

    private void SetItemQuantityUI()
    {
        if (SlotItem == null)
            return;

        if (SlotItem is Item inventoryItem)
        {
            if (inventoryItem != null && inventory.ItemContainer.HasItem(inventoryItem))
            {
                int quantityCount = inventory.ItemContainer.GetTotalQuantity(inventoryItem);
                itemQuantityText.text = quantityCount > 1 ? quantityCount.ToString() : "";
            }
            else
            {
                SlotItem = null;
            }
        }
        else
        {
            itemQuantityText.enabled = false;
        }
    }

    protected override void EnableSlotUI(bool enable)
    {
        base.EnableSlotUI(enable);
        itemQuantityText.enabled = enable;
    }
}

