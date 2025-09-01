using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Àåºñ Ä­ UI
/// </summary>
public class UIEquipmentCell : MonoBehaviour
{
    [SerializeField] private Button thisButton;
    [SerializeField] private Image equipmentImage;
    [SerializeField] private Sprite defaultEquipmentImage;
    [SerializeField] private TextMeshProUGUI enhanceLevel;
    private Item containedItem;

    public Item ContainedItem => containedItem;

    private void Awake()
    {
        if (!thisButton)
            thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnClickButton);
    }

    public void SetEquipment(Item item)
    {
        EquipmentItem equipmentItem = item as EquipmentItem;

        if (equipmentItem == null)
            return;

        containedItem = item;

        InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);

        equipmentImage.sprite = itemTbCell.Icon;
        enhanceLevel.text =
            equipmentItem.Level == 0 
            ? "" 
            : equipmentItem.Level.ToString();
    }

    public void UnsetEquipment()
    {
        equipmentImage.sprite = defaultEquipmentImage;
        enhanceLevel.text = "";
        containedItem = null;
    }

    private void OnClickButton()
    {
        if (containedItem == null)
            return;
        UIManager.Instance.OpenItemDescriptionPopup(containedItem);
    }
}
