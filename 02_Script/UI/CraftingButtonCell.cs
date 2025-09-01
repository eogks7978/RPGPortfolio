using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 제작팝업의 제작 버튼
/// </summary>
public class CraftingButtonCell : MonoBehaviour
{
    [SerializeField] private Button thisButton;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMPro.TextMeshProUGUI itemNameText;
    private CraftingTableCell tbCell;

    private void Awake()
    {
        if (thisButton == null)
            thisButton = GetComponent<Button>();

        thisButton.onClick.AddListener(OnClickButton);
    }

    public void SetButton(CraftingTableCell tbCell)
    {
        this.tbCell = tbCell;

        InventoryItemTableCell resultItemTbCell = 
            DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(
                tbCell.ResultItemCode);

        if (resultItemTbCell != null) 
        {
            itemImage.sprite = resultItemTbCell.Icon;
            itemNameText.text = resultItemTbCell.ColouredName;
            gameObject.SetActive(true);
        }
    }

    public void OnClickButton()
    {
        UIManager.Instance.OpenCraftingInformationPopup(tbCell);
    }
}
