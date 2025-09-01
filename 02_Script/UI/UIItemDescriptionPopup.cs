using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 인벤토리에서 아이템 클릭시 아이템 설명 팝업
/// </summary>
public class UIItemDescriptionPopup : UIPanel
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMPro.TextMeshProUGUI itemName;
    [SerializeField] private TMPro.TextMeshProUGUI enhanceText;
    [SerializeField] private TMPro.TextMeshProUGUI itemStack;
    [SerializeField] private TMPro.TextMeshProUGUI sellPrice;

    [SerializeField, Header("스탯 표시 위한 Text 오브젝트들")] private List<TMPro.TextMeshProUGUI> statTexts;
    [Header("사용중(active true)인 스탯 표시 Text 오브젝트들")]private List<TMPro.TextMeshProUGUI> usingStatTexts = new();

    [SerializeField] private GameObject itemStatLayoutGroup;

    [SerializeField] private Button EquipmentSetBtn;
    [SerializeField] private Button equipmentUnSetBtn;
    [SerializeField] private Button consumableUnSetBtn;
    [SerializeField] private Button destroyBtn;
    [SerializeField] private Button useBtn;
    [SerializeField] private Button quickSlotBtn;
    [SerializeField] private Button sellBtn;
    [SerializeField] private Button setEnhanceBtn;
    [SerializeField] private TMPro.TextMeshProUGUI itemDescription;

    [Header("현재 이 객체에서 관리중인 아이템")]private Item containedItem;

    private void Start()
    {
        EquipmentSetBtn.onClick.AddListener(OnClickSetButton);
        equipmentUnSetBtn.onClick.AddListener(OnClickEquipmentUnSetButton);
        consumableUnSetBtn.onClick.AddListener(OnClickConsumableUnSetButton);
        destroyBtn.onClick.AddListener(OnClickDestroyButton);
        useBtn.onClick.AddListener(OnClickUseButton);
        quickSlotBtn.onClick.AddListener(OnClickQuickSlotButton);
        sellBtn.onClick.AddListener(OnClickSellButton);
        setEnhanceBtn.onClick.AddListener(OnClickSetEnhanceButton);
    }

    public void Open(Item item, UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);

        InventoryItemTableCell tbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);

        if (tbCell == null)
            return;

        Initialize();

        containedItem = item;
        itemImage.sprite = tbCell.Icon;
        itemName.text = tbCell.ColouredName;
        itemStack.text = item.Quantity.ToString();
        sellPrice.text = tbCell.SellPrice.ToString();
        itemDescription.text = tbCell.Description;

        ConsumableItem consumableItem = item as ConsumableItem;
        EquipmentItem equipmentItem = item as EquipmentItem;

        enhanceText.text = equipmentItem != null && equipmentItem.Level != 0 ? "+" + equipmentItem.Level : "";

        bool isStoreOpened = UIManager.Instance.StorePopup.gameObject.activeSelf;
        bool isEnhancePopupOpened = UIManager.Instance.EnhancePopup.gameObject.activeSelf;

        itemStatLayoutGroup.SetActive(equipmentItem != null);
        EquipmentSetBtn.gameObject.SetActive(!isEnhancePopupOpened && equipmentItem != null && !equipmentItem.IsEquiped);
        equipmentUnSetBtn.gameObject.SetActive(equipmentItem != null && equipmentItem.IsEquiped);
        consumableUnSetBtn.gameObject.SetActive(!isEnhancePopupOpened && !isStoreOpened && consumableItem != null && consumableItem.IsSlotted);
        useBtn.gameObject.SetActive(consumableItem != null);
        quickSlotBtn.gameObject.SetActive(!isStoreOpened && consumableItem != null);
        destroyBtn.gameObject.SetActive(!isEnhancePopupOpened && (equipmentItem == null || equipmentItem != null && !equipmentItem.IsEquiped));
        sellBtn.gameObject.SetActive(isStoreOpened && (equipmentItem != null && !equipmentItem.IsEquiped || equipmentItem == null));
        setEnhanceBtn.gameObject.SetActive(isEnhancePopupOpened && (equipmentItem != null && !equipmentItem.IsEquiped));

        if (equipmentItem != null)
        {
            var itemStatData = equipmentItem.GetEquipmentStat();

            if (itemStatData != null)
            {
                if (itemStatData.MaxHealth != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.MaxHealth)) + " + " + itemStatData.MaxHealth);
                if (itemStatData.MaxMana != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.MaxMana)) + " + " + itemStatData.MaxMana);
                if (itemStatData.AttackPoint != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.AttackPoint)) + " + " + itemStatData.AttackPoint);
                if (itemStatData.DefensePoint != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.DefensePoint)) + " + " + itemStatData.DefensePoint);
                if (itemStatData.AttackSpeed != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.AttackSpeed)) + " + " + itemStatData.AttackSpeed.ToString("F2"));
                if (itemStatData.MoveSpeed != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.MoveSpeed)) + " + " + itemStatData.MoveSpeed.ToString("F2"));
                if (itemStatData.Strength != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.Strength)) + " + " + itemStatData.Strength);
                if (itemStatData.Dexterity != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.Dexterity)) + " + " + itemStatData.Dexterity);
                if (itemStatData.Constitution != 0f)
                    ShowStatText(TranslateHandler.Translate(nameof(itemStatData.Constitution)) + " + " + itemStatData.Constitution);
            }
        }
    }

    private void Initialize()
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
        foreach(TMPro.TextMeshProUGUI tmp in statTexts)
        {
            tmp.text = text;
            usingStatTexts.Add(tmp);
            statTexts.Remove(tmp);
            tmp.gameObject.SetActive(true);
            break;
        }
    }

    private void OnClickSetButton()
    {
        InventoryManager.Instance.SetEquipment(containedItem, true);
        Close();
    }

    private void OnClickEquipmentUnSetButton()
    {
        InventoryManager.Instance.UnSetEquipment(containedItem);
        Close();
    }

    private void OnClickConsumableUnSetButton()
    {
        UIManager.Instance.ItemHotbar.Remove(containedItem);
        Close();
    }

    private void OnClickDestroyButton()
    {
        if (containedItem.Quantity <= 1)
        {
            InventoryManager.Instance.TryDestroyItem(containedItem, containedItem.Quantity,
                (resultObj2) =>
                {
                    if (resultObj2)
                        Close();
                });
        }
        else
        {
            UIManager.Instance.OpenInputfieldConfirmPopup("버릴 개수", 1, containedItem.Quantity,
                (resultObj) =>
                {
                    var result = resultObj as UIInputFieldConfirmPopup.Results;
                    if (result != null)
                    {
                        InventoryManager.Instance.TryDestroyItem(containedItem, result.value,
                            (resultObj2) =>
                            {
                                if (resultObj2)
                                    Close();
                            });
                    }
                }
                );
        }
    }

    private void OnClickUseButton()
    {
        InventoryManager.Instance.UseItem(containedItem, 1);
        Close();
    }

    private void OnClickQuickSlotButton()
    {
        UIManager.Instance.OpenItemSetPopup(containedItem);
        Close();
    }

    private void OnClickSellButton()
    {
        InventoryItemTableCell tbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(containedItem.Code);

        if (tbCell == null)
            return;

        if (containedItem.Quantity <= 1)
        {
            UIManager.Instance.OpenNotificationPopup(
                "정말 판매하시겠습니까?",
                noButtonOn: true,
                okButtonText: "예",
                noButtonText: "아니오",
                cbClose:
                (resultObj) =>
                {
                    var result = resultObj as UINotificationPopup.Results;
                    if (result.isConfirm)
                    {
                        int earnedMoney = tbCell.SellPrice;
                        InventoryManager.Instance.PlayerInventory.ItemContainer.RemoveItem(containedItem, 1);
                        InventoryManager.Instance.GetMoney(earnedMoney);
                        Close();
                    }
                });
        }
        else
        {
            UIManager.Instance.OpenInputfieldConfirmPopup("판매 개수", 1, containedItem.Quantity,
                (resultObj) =>
                {
                    var result = resultObj as UIInputFieldConfirmPopup.Results;
                    if (result != null)
                    {
                        int earnedMoney = tbCell.SellPrice * result.value;
                        InventoryManager.Instance.PlayerInventory.ItemContainer.RemoveItem(containedItem, result.value);
                        InventoryManager.Instance.GetMoney(earnedMoney);
                        UIManager.Instance.OpenNotificationPopup("판매하였습니다.");

                        Close();
                    }
                });
        }
    }

    private void OnClickSetEnhanceButton()
    {
        UIManager.Instance.EnhancePopup.SetItem(containedItem as EquipmentItem);
        Close();
    }
}
