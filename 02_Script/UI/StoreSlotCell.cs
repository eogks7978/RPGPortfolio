using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// StorePopup의 하나의 아이템을 담은 셀
/// </summary>
public class StoreSlotCell : MonoBehaviour
{
    [SerializeField] private Item containItem;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMPro.TextMeshProUGUI itemNameText;
    [SerializeField] private TMPro.TextMeshProUGUI quantityText;
    [SerializeField] private Button thisButton;

    private void Awake()
    {
        thisButton.onClick.AddListener(OnClickButton);
    }

    public void SetItemCell(Item item)
    {
        containItem = item;

        var tbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);
        itemImage.sprite = tbCell.Icon;
        itemNameText.text = tbCell.ColouredName;
        quantityText.text = item.Quantity.ToString();
    }

    private void OnClickButton()
    {
        UIManager.Instance.OpenItemInformationPopup(containItem);
    }
}
