using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 상점 팝업
/// </summary>
public class UIStorePopup : UIPanel
{
    [SerializeField, Header("아이템들의 셀")] private List<StoreSlotCell> sellingItemCells;
    [Header("사용중인(active on) 아이템 셀들")]private List<StoreSlotCell> usingSellingItemCells = new();

    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField, Header("스크롤 초기화를 위한 content 할당")] private RectTransform contentRecttransform;
    [SerializeField] private RectTransform popupBackgroundRt;

    [Header("판매중인 아이템들")] private List<Item> itemList;

    public RectTransform PopupBackgroundRt => popupBackgroundRt;

    public void Open(List<Item> itemList, UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);
        this.itemList = itemList;
        contentRecttransform.localPosition = Vector3.zero;
        UpdatePopup();
        UIManager.Instance.OpenItemContainer();
    }

    private void UpdatePopup()
    {
        if (itemList == null) 
            return;

        int usingCellCount = 0;
        foreach (var item in itemList)
        {
            SetItemCell(item);
            usingCellCount++;
        }

        int cellFloor = 1 + usingSellingItemCells.Count / 4;
        float totalContentSpacingHeight = gridLayoutGroup.spacing.y * (cellFloor - 1);
        float cellHeight = usingSellingItemCells.First().GetComponent<RectTransform>().rect.height;

        contentRecttransform.sizeDelta = new Vector2(
            contentRecttransform.sizeDelta.x, cellHeight * cellFloor + totalContentSpacingHeight + 100f);
    }

    private void SetItemCell(Item item)
    {
        if (sellingItemCells != null 
            && sellingItemCells.Count > 0)
        {
            var firstCell = sellingItemCells.First();
            usingSellingItemCells.Add(firstCell);
            sellingItemCells.Remove(firstCell);
            firstCell.SetItemCell(item);
            firstCell.gameObject.SetActive(true);
        }
    }

    private void ResetPopup()
    {
        if (usingSellingItemCells != null 
            && usingSellingItemCells.Count != 0)
        {
            foreach (var cell in usingSellingItemCells)
            {
                cell.gameObject.SetActive(false);
            }

            if (usingSellingItemCells.Count != 0)
            {
                for (int i = usingSellingItemCells.Count - 1; i >= 0; i--)
                {
                    sellingItemCells.Insert(0, usingSellingItemCells[i]);
                }
                usingSellingItemCells.Clear();
            }
        }
    }

    public override void Close()
    {
        base.Close();
        UIManager.Instance.CloseItemContainer();
        ResetPopup();
    }
}
