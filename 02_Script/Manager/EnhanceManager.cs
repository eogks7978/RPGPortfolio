using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 강화 관리자
/// </summary>
public class EnhanceManager : Singleton<EnhanceManager>
{
    private string greenHex = ColorUtility.ToHtmlStringRGB(Color.green);
    private string yellowHex = ColorUtility.ToHtmlStringRGB(Color.yellow);

    /// <summary>
    /// 강화 시도
    /// </summary>
    /// <param name="item">강화 대상 아이템</param>
    /// <returns></returns>
    public string Enhance(EquipmentItem item)
    {
        EquipmentTableCell tbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code) as EquipmentTableCell;
        EquipmentStatData enhanceStatData = tbCell.EnhanceData.EnhanceBonus.Find(n => n.Level == item.Level);

        float randomValue = UnityEngine.Random.Range(0f, 1f);

        string resultContent = string.Empty;

        // 성공
        if (randomValue <= enhanceStatData.SuccessChance)
        {
            EquipmentStat newStat = item.BonusStat;

            if (newStat == null)
                item.BonusStat = new();

            resultContent += $"<color=#{greenHex}>{"강화 성공!"}</color>" + Environment.NewLine + Environment.NewLine;

            // ItemTableCell 마다 강화시 오르는 능력치 검사 후 증감
            foreach (var statData in enhanceStatData.EnhanceDatas)
            {
                var newStatField = item.BonusStat.GetType().GetField(statData.StatToEnhance);
                bool isTypeSingle = newStatField.GetValue(item.BonusStat).GetType() == typeof(System.Single);

                if (isTypeSingle)
                {
                    float randomStatValue = UnityEngine.Random.Range(statData.MinValue, statData.MaxValue);
                    newStatField.SetValue(item.BonusStat, Convert.ToSingle(newStatField.GetValue(item.BonusStat)) + randomStatValue);
                    resultContent += TranslateHandler.Translate(statData.StatToEnhance) + " + " + $"<color=#{yellowHex}>{randomStatValue.ToString("F1")}</color>" + Environment.NewLine;
                }
                else
                {
                    int randomStatValue = UnityEngine.Random.Range((int)statData.MinValue, (int)statData.MaxValue + 1);
                    newStatField.SetValue(item.BonusStat, Convert.ToInt32(newStatField.GetValue(item.BonusStat)) + randomStatValue);
                    resultContent += TranslateHandler.Translate(statData.StatToEnhance) + " + " + $"<color=#{yellowHex}>{randomStatValue}</color>" + Environment.NewLine;
                }
            }

            item.Level += 1;
        }

        foreach (var cell in enhanceStatData.MaterialItemCodeAndRequiredNumber)
        {
            RemoveItems(cell.key, cell.value);
        }

        return resultContent;
    }

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
