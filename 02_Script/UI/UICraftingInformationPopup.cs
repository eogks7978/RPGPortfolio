using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 제작 아이템 정보 팝업
/// </summary>
public class UICraftingInformationPopup : UIPanel
{
    [SerializeField] private Button craftButton;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMPro.TextMeshProUGUI itemNameText;
    [SerializeField, Header("재료 아이템 셀")] private List<UICraftingMaterialCell> craftingMaterialCells;
    [SerializeField] private RectTransform popupBackgroundRt;
    [SerializeField] private RectTransform materialCelllayoutGroup;

    [Tooltip("조합식 TableCell")] private CraftingTableCell tbCell;
    [Tooltip("결과 아이템 TableCell")] private InventoryItemTableCell resultItemTbCell;

    public RectTransform PopupBackgroundRt => popupBackgroundRt;
    public RectTransform MaterialCelllayoutGroup => materialCelllayoutGroup;

    protected override void Awake()
    {
        base.Awake();

        craftButton.onClick.AddListener(OnClickCraftButton);
    }

    public void Open(CraftingTableCell tbCell, UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);
        this.tbCell = tbCell;

        InitializeButtons();
        UpdatePopup();
        UIManager.Instance.OpenItemContainer();
    }

    public override void Close()
    {
        base.Close();
        UIManager.Instance.CloseItemContainer();
    }

    private void InitializeButtons()
    {
        foreach (var btn in craftingMaterialCells)
        {
            btn.gameObject.SetActive(false);
        }
    }

    private void UpdatePopup()
    {
        resultItemTbCell =
            DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(
                tbCell.ResultItemCode);

        if (resultItemTbCell != null)
        {
            itemImage.sprite = resultItemTbCell.Icon;
            itemNameText.text = resultItemTbCell.ColouredName;
        }

        for (int i = 0; i < tbCell.MaterialItemCodeAndRequiredNumber.Count; ++i)
        {
            if (i < craftingMaterialCells.Count)
            {
                InventoryItemTableCell craftingMaterialTbCell =
                    DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(
                        tbCell.MaterialItemCodeAndRequiredNumber[i].key);

                craftingMaterialCells[i].SetButton(craftingMaterialTbCell,
                    tbCell.MaterialItemCodeAndRequiredNumber[i].value);
            }
            else
                break;
        }
    }

    private void OnClickCraftButton()
    {
        if (!HasSufficientMaterials())
        {
            UIManager.Instance.OpenNotificationPopup("재료가 부족합니다.");
            return;
        }

        foreach (var cell in tbCell.MaterialItemCodeAndRequiredNumber)
        {
            RemoveItems(cell.key, cell.value);
        }

        Item createdItem = resultItemTbCell._ItemType switch
        {
            InventoryItemTableCell.ItemType.Equipment => new EquipmentItem(resultItemTbCell.Code, 1),
            InventoryItemTableCell.ItemType.Consumable => new ConsumableItem(resultItemTbCell.Code, 1),
            InventoryItemTableCell.ItemType.Etc => new EtcItem(resultItemTbCell.Code, 1),
            InventoryItemTableCell.ItemType.None => new Item(resultItemTbCell.Code, 1),
            _ => new Item(resultItemTbCell.Code, 1)
        };

        InventoryManager.Instance.TryGetItem(createdItem, null);
    }

    /// <summary>
    /// 재료를 모두 보유하고 있는지 검사
    /// </summary>
    /// <returns></returns>
    private bool HasSufficientMaterials()
    {
        return tbCell.MaterialItemCodeAndRequiredNumber.All(cell =>
        {
            var materialItemList = InventoryManager.Instance.GetItem(cell.key);
            int totalCount = materialItemList.Sum(item => item.Quantity);
            return totalCount >= cell.value;
        });
    }

    /// <summary>
    /// 제작 시 재료 아이템 감소
    /// </summary>
    /// <param name="itemKey"></param>
    /// <param name="requiredQuantity"></param>
    private void RemoveItems(int itemKey, int requiredQuantity)
    {
        var itemList = InventoryManager.Instance.GetItem(itemKey);
        itemList = itemList.OrderBy(n => n.Quantity).ToList();

        foreach (var item in itemList)
        {
            if (requiredQuantity <= 0) break;

            int quantityToRemove = Math.Min(item.Quantity, requiredQuantity);
            InventoryManager.Instance.RemoveItem(item, quantityToRemove, null);
            requiredQuantity -= quantityToRemove;
        }
    }
}
