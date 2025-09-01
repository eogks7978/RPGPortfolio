using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 제작 팝업
/// </summary>
public class UICraftingPopup : UIPanel
{
    [SerializeField, Header("제작 아이템들 정보가 담길 셀들")] private List<CraftingButtonCell> cells;
    [SerializeField] private RectTransform popupBackgroundRt;

    public RectTransform PopupBackgroundRt => popupBackgroundRt;
    public List<CraftingButtonCell> Cells => cells;

    public void Initialize()
    {
        List<CraftingTableCell> tbCells = DataManager.Instance.TableDataTable.CraftingTable.List;

        for (int i = 0; i < tbCells.Count; i++)
        {
            if (i < cells.Count)
            {
                cells[i].SetButton(tbCells[i]);
            }
            else
                break;
        }
    }
}
