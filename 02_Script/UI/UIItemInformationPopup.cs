using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 상점 아이템 클릭 시 아이템 정보를 나타내는 팝업
/// </summary>
public class UIItemInformationPopup : UIPanel
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMPro.TextMeshProUGUI itemName;
    [SerializeField] private TMPro.TextMeshProUGUI itemStack;
    [SerializeField] private TMPro.TextMeshProUGUI buyPrice;

    [SerializeField] private List<TMPro.TextMeshProUGUI> statTexts;
    private List<TMPro.TextMeshProUGUI> usingStatTexts = new();

    [SerializeField, Header("장비의 능력치 표시 오브젝트")] private GameObject itemStatLayoutGroup;
    [SerializeField, Header("아이템 설명")] private TMPro.TextMeshProUGUI itemDescription;
    [SerializeField, Header("구매 버튼")] private Button buyButton;

    private Item containedItem;
    private InventoryItemTableCell containedItemTbCell;

    protected override void Awake()
    {
        base.Awake();
        buyButton.onClick.AddListener(OnClickBuyButton);
    }

    public void Open(Item item, UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);

        UpdatePopup(item);
    }

    private void InitializeTextList()
    {
        statTexts.AddRange(usingStatTexts);
        usingStatTexts.Clear();

        foreach (TMPro.TextMeshProUGUI tmp in statTexts)
        {
            tmp.gameObject.SetActive(false);
        }
    }

    private void ShowStatText(string text)
    {
        foreach (TMPro.TextMeshProUGUI tmp in statTexts)
        {
            tmp.text = text;
            usingStatTexts.Add(tmp);
            statTexts.Remove(tmp);
            tmp.gameObject.SetActive(true);
            break;
        }
    }

    private void UpdatePopup(Item item)
    {
        containedItem = item;
        containedItemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);

        if (containedItemTbCell == null)
            return;

        InitializeTextList();

        itemImage.sprite = containedItemTbCell.Icon;
        itemName.text = containedItemTbCell.ColouredName;
        itemStack.text = item.Quantity.ToString();
        buyPrice.color = InventoryManager.Instance.Money < containedItemTbCell.BuyPrice ? Color.red : Color.green;
        buyPrice.text = containedItemTbCell.BuyPrice.ToString();
        itemDescription.text = containedItemTbCell.Description;

        EquipmentItem equipmentItem = containedItem as EquipmentItem;
        if (equipmentItem != null)
        {
            var itemStatData = equipmentItem.GetEquipmentStat();

            if (itemStatData != null)
            {
                if (itemStatData.MaxHealth != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.MaxHealth))+ " + " + itemStatData.MaxHealth);
                if (itemStatData.MaxMana != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.MaxMana)) + " + " + itemStatData.MaxMana);
                if (itemStatData.AttackPoint != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.AttackPoint)) + " + " + itemStatData.AttackPoint);
                if (itemStatData.DefensePoint != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.DefensePoint)) + " + " + itemStatData.DefensePoint);
                if (itemStatData.AttackSpeed != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.AttackSpeed)) + " + " + itemStatData.AttackSpeed);
                if (itemStatData.MoveSpeed != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.MoveSpeed)) + " + " + itemStatData.MoveSpeed);
                if (itemStatData.Strength != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.Strength)) + " + " + itemStatData.Strength);
                if (itemStatData.Dexterity != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.Dexterity)) + " + " + itemStatData.Dexterity);
                if (itemStatData.Constitution != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.Constitution)) + " + " + itemStatData.Constitution);
            }
        }
    }

    /// <summary>
    /// 아이템 구매 클릭
    /// </summary>
    private void OnClickBuyButton()
    {
        if (InventoryManager.Instance.isFull())
        {
            UIManager.Instance.ShowSimpleNotification("가방이 꽉 찼습니다.", Color.red);
            return;
        }
        else if (InventoryManager.Instance.Money < containedItemTbCell.BuyPrice)
        {
            UIManager.Instance.ShowSimpleNotification("골드가 부족합니다.", Color.red);
            return;
        }

        UIManager.Instance.OpenInputfieldConfirmPopup("살 개수", 1, 99,
            (resultObj) =>
            {
                var result = resultObj as UIInputFieldConfirmPopup.Results;
                if (result != null)
                {
                    int resultPrice = containedItemTbCell.BuyPrice * result.value;

                    if (InventoryManager.Instance.Money < resultPrice)
                    {
                        UIManager.Instance.ShowSimpleNotification("골드가 부족합니다.", Color.red);
                        return;
                    }

                    Item item = Item.GetDeepCopiedItemAsChildType(containedItem);
                    item.Quantity = result.value;

                    InventoryManager.Instance.TryGetItem(item, 
                        (isSuccess) =>
                        {
                            if (isSuccess)
                            {
                                UIManager.Instance.OpenNotificationPopup("구매 완료.");
                                InventoryManager.Instance.GetMoney(-resultPrice);
                            }
                            else
                            {
                                UIManager.Instance.OpenNotificationPopup("구매 할 수 없었습니다.");
                            }
                            Close();
                        });
                };
            });
    }
}
