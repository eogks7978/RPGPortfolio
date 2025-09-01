using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 도감 팝업
/// </summary>
public class UIIllustratedGuidePopup : UIPanel
{
    [SerializeField, Header("스크롤 초기화를 위한 ScrollView의 Content")] private Transform contentTr;
    [SerializeField] private List<IllustratedGuideCell> cells;
    private List<IllustratedGuideCell> usingCells = new();
    [SerializeField] private RectTransform popupBackgroundRt;

    public RectTransform PopupBackgroundRt => popupBackgroundRt;

    public override void Open(UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);
        Initialize();
        UpdatePopup();
    }

    public override void Close()
    {
        base.Close();

        foreach(var uiCell in usingCells)
        {
            uiCell.ResetCell();
        }
    }

    private void Initialize()
    {
        cells.AddRange(usingCells);
        usingCells.Clear();
        contentTr.localPosition = Vector3.zero;
    }

    private void UpdatePopup()
    {
        foreach(var cell in DataManager.Instance.TableDataTable.MonsterTable.List)
        {
            if (cells == null || cells.Count == 0)
                return;

            IllustratedGuideCell uiCell = cells.First();
            usingCells.Add(uiCell);
            cells.Remove(uiCell);
            uiCell.SetCell(cell);
        }
    }
}
