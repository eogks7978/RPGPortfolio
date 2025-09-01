using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 강화 팝업
/// </summary>
public class UIEnhancePopup : UIPanel
{
    [SerializeField] private Button enhanceButton;
    [SerializeField] private Button itemImageButton;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMPro.TextMeshProUGUI itemNameText;
    [SerializeField] private TMPro.TextMeshProUGUI itemLevelText;
    [SerializeField, Tooltip("강화 확률 표시 UI")] 
    private TMPro.TextMeshProUGUI enhanceChanceText;
    [SerializeField] private List<TMPro.TextMeshProUGUI> statTexts;
    [SerializeField, Tooltip("아이템 재료 목록 UI")] 
    private List<UICraftingMaterialCell> craftingMaterialCells;
    [SerializeField] private RectTransform popupBackgroundRt;

    private EquipmentItem containedItem;
    private EquipmentTableCell tbCell;
    private EquipmentStatData enhanceStatData;

    private string yellowHex = ColorUtility.ToHtmlStringRGB(Color.yellow);
    private string blueHex = ColorUtility.ToHtmlStringRGB(Color.blue);

    public RectTransform PopupBackgroundRt => popupBackgroundRt;

    protected override void Awake()
    {
        base.Awake();
        itemImageButton.onClick.AddListener(UnsetItem);
        enhanceButton.onClick.AddListener(OnClickEnhanceButton);
    }

    public override void Open(UnityAction<object> cbClose = null)
    {
        ResetPopup();
        base.Open(cbClose);
        UIManager.Instance.OpenItemContainer();
    }

    public void ResetPopup()
    {
        containedItem = null;
        itemImage.sprite = null;
        itemImage.gameObject.SetActive(false);
        itemNameText.gameObject.SetActive(false);
        itemLevelText.gameObject.SetActive(false);
        enhanceChanceText.gameObject.SetActive(false);
        tbCell = null;
        enhanceStatData = null;

        foreach (var cell in craftingMaterialCells)
        {
            cell.gameObject.SetActive(false);
        }

        ResetStatTexts();
    }

    private void ResetStatTexts()
    {
        foreach (var text in statTexts)
        {
            text.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 아이템 등록(강화 시도시에도 업데이트를 위해 재호출중. 분리해야하나?)
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(EquipmentItem item)
    {
        tbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code) as EquipmentTableCell;

        if (tbCell == null)
            return;

        EquipmentStat itemStat = item.GetEquipmentStat();
        enhanceStatData = tbCell.EnhanceData.EnhanceBonus.Find(n => n.Level == item.Level);
        bool isNextEnhanceLevelExist = tbCell.EnhanceData.EnhanceBonus.Find(n => n.Level == item.Level) != null;

        if (enhanceStatData == null)
        {
            ResetPopup();
            return;
        }

        ResetStatTexts();

        containedItem = item;
        itemImage.sprite = tbCell.Icon;
        itemImage.gameObject.SetActive(true);
        itemNameText.text = tbCell.ColouredName;
        itemNameText.gameObject.SetActive(true);
        itemLevelText.text = isNextEnhanceLevelExist ? item.Level + " -> " + $"<color=#{yellowHex}>{item.Level + 1}</color>" : item.Level.ToString();
        itemLevelText.gameObject.SetActive(true);
        enhanceChanceText.text = "성공 확률" + $"<color=#{yellowHex}>{enhanceStatData.SuccessChance * 100f}</color>" + "%";
        enhanceChanceText.gameObject.SetActive(true);

        foreach (EquipmentEnhanceData data in enhanceStatData.EnhanceDatas)
        {
            float statValue = Convert.ToSingle(itemStat.GetType().GetField(data.StatToEnhance).GetValue(itemStat));
            SetStatText(data.StatToEnhance, statValue, data.MinValue, data.MaxValue);
        }

        for (int i = 0; i < enhanceStatData.MaterialItemCodeAndRequiredNumber.Count; ++i)
        {
            if (i >= craftingMaterialCells.Count)
                break;

            InventoryItemTableCell materialItemTbCell =
                DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(
                    enhanceStatData.MaterialItemCodeAndRequiredNumber[i].key);

            craftingMaterialCells[i].SetButton(materialItemTbCell, enhanceStatData.MaterialItemCodeAndRequiredNumber[i].value);
        }
    }

    public void SetStatText(string statName, float beforeLevelValue, float minValue, float maxValue)
    {
        foreach (var tmp in statTexts)
        {
            tmp.text = TranslateHandler.Translate(statName)
                + " "
                + $"<color=#{blueHex}>{beforeLevelValue.ToString("F1")}</color>"
                + "   +"
                + $"<color=#{yellowHex}>{minValue + " ~ " + maxValue}</color>";

            tmp.gameObject.SetActive(true);

            statTexts.Remove(tmp);
            statTexts.Add(tmp);
            break;
        }
    }

    /// <summary>
    /// 아이템을 강화 대상에서 해제
    /// </summary>
    public void UnsetItem()
    {
        ResetPopup();
    }

    public override void Close()
    {
        base.Close();
        UIManager.Instance.CloseItemContainer();
    }

    private void OnClickEnhanceButton()
    {
        if (containedItem == null)
        {
            UIManager.Instance.OpenNotificationPopup("아이템을 등록 해 주세요.");
            return;
        }

        if (enhanceStatData == null)
        {
            UIManager.Instance.OpenNotificationPopup("최대 레벨입니다.");
            return;
        }

        if (!HasSufficientMaterials())
        {
            UIManager.Instance.OpenNotificationPopup("재료가 부족합니다.");
            return;
        }

        string resultString = EnhanceManager.Instance.Enhance(containedItem);
        SetItem(containedItem);

        UIManager.Instance.OpenEnhanceAnimationPopup(
            (result) =>
            {
                if (resultString != string.Empty)
                {
                    UIManager.Instance.OpenNotificationPopup(resultString);
                }
                else
                {
                    UIManager.Instance.OpenNotificationPopup("강화에 실패하였습니다.");
                }
            });
    }

    /// <summary>
    /// 강화 재료를 가지고 있는지 검사
    /// </summary>
    /// <returns></returns>
    private bool HasSufficientMaterials()
    {
        return enhanceStatData.MaterialItemCodeAndRequiredNumber.All(cell =>
        {
            var materialItemList = InventoryManager.Instance.GetItem(cell.key);
            int totalCount = materialItemList.Sum(item => item.Quantity);
            return totalCount >= cell.value;
        });
    }
}
