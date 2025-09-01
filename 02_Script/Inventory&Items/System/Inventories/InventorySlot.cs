using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : ItemSlotUI
{
    [SerializeField] private InventoryTableCell inventory = null;
    [SerializeField] private TextMeshProUGUI itemQuantityText = null;
    [SerializeField] private TextMeshProUGUI enhanceText;
    [SerializeField] private GameObject isEquipedDisplay;
    [SerializeField] protected Button thisButton;

    private void Awake()
    {
        if (!thisButton)
            thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnClickButton);
    }

    public override Item SlotItem
    {
        get { return item; }
        set { }
    }

    public Item item => inventory.ItemContainer.GetSlotByIndex(SlotIndex);

    public override void UpdateSlotUI()
    {
        if (item != null)
        {
            InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);
            itemQuantityText.text = item.Quantity > 1 ? item.Quantity.ToString() : "";
            itemIconImage.sprite = itemTbCell.Icon;

            if (item.ItemType == InventoryItemTableCell.ItemType.Equipment)
            {
                EquipmentItem equipmentItem = item as EquipmentItem;
                enhanceText.gameObject.SetActive(true);
                enhanceText.text = equipmentItem.Level != 0 ? "+" + equipmentItem.Level : "";
                isEquipedDisplay.SetActive(equipmentItem.IsEquiped);
            }
            else
            {
                enhanceText.gameObject.SetActive(false);
                isEquipedDisplay.SetActive(false);
            }

            EnableSlotUI(true);
        }
        else
        {
            enhanceText.gameObject.SetActive(false);
            isEquipedDisplay.SetActive(false);
            EnableSlotUI(false);
            return;
        }

    }

    protected override void EnableSlotUI(bool enable)
    {
        base.EnableSlotUI(enable);
        itemQuantityText.enabled = enable;
    }

    private void OnClickButton()
    {
        if (SlotItem == null)
            return;
        UIManager.Instance.OpenItemDescriptionPopup(item);
    }
}

