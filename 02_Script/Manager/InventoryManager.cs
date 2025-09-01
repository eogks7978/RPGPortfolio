using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이벤토리 관리자
/// </summary>
public class InventoryManager : MonoBehaviour
{
    #region singleton
    private static InventoryManager instance;
    public static InventoryManager Instance => instance;
    #endregion

    [SerializeField] private InventoryTableCell playerInventory;
    public InventoryTableCell PlayerInventory => playerInventory;

    [SerializeField, Header("장비 장착 사운드")] private AudioClip equipSound;
    [SerializeField, Header("돈 획득 사운드")] private AudioClip moneySound;

    private int money;

    // 사용중인 장비들
    private Item usingWeapon;
    private Item usingHelmet;
    private Item usingArmor;
    private Item usingPants;
    private Item usingBoots;
    private Item usingNeckless;
    private Item usingRing_01;
    private Item usingRing_02;

    #region Property
    public int Money => money;
    public Item UsingWeapon => usingWeapon;
    public Item UsingHelmet => usingHelmet;
    public Item UsingArmor => usingArmor;
    public Item UsingPants => usingPants;
    public Item UsingBoots => usingBoots;
    public Item UsingNeckless => usingNeckless;
    public Item UsingRing_01 => usingRing_01;
    public Item UsingRing_02 => usingRing_02;
    #endregion

    private void Awake()
    {
        //if (instance == null)
        //{
        instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
    }

    public void Initialize(InventoryData data)
    {
        playerInventory.Initialize(data);
        money = data.Money;
        UIManager.Instance.ItemContainerPopup.Initialize(data.ItemList.Count);
    }

    public void TryGetItem(Item item, Action<bool> successAdd = null)
    {
        playerInventory.ItemContainer.AddItem(item, successAdd);
    }

    public void TryDestroyItem(Item item, int count, Action<bool> cbResult)
    {
        EquipmentItem equipmentItem = item as EquipmentItem;
        if (equipmentItem != null && equipmentItem.IsEquiped)
        {
            UIManager.Instance.OpenNotificationPopup(
                "장착중인 아이템입니다.");
            return;
        }

        UIManager.Instance.OpenNotificationPopup(
            "정말 버리시겠습니까? 버린 아이템은 영구적으로 제거됩니다.",
            noButtonOn: true,
            okButtonText: "예",
            noButtonText: "아니오",
            cbClose:
            (resultObj) =>
            {
                var result = resultObj as UINotificationPopup.Results;

                if (result.isConfirm)
                {
                    playerInventory.ItemContainer.RemoveItem(item, count);
                }
                cbResult?.Invoke(result.isConfirm);
            });
    }

    public void RemoveItem(Item item, int count, Action<bool> cbResult)
    {
        playerInventory.ItemContainer.RemoveItem(item, count);
    }

    public void GetMoney(int money)
    {
        int resultMoney = this.money += money;

        if (resultMoney < 0)
            resultMoney = 0;

        if (resultMoney > int.MaxValue)
            resultMoney = int.MaxValue;

        this.money = resultMoney;
        SoundController.Instance.PlaySfx(moneySound);
        EventHandler.OnMoneyEarned?.Invoke(money);
    }

    public void UseItem(Item item, int value)
    {
        if (item == null)
            return;

        InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);
        ConsumableTableCell tbCell = itemTbCell as ConsumableTableCell;

        if (tbCell == null)
            return;

        tbCell.item.UseItem();
        playerInventory.ItemContainer.RemoveItem(item, value);
    }

    /// <summary>
    /// 아이템 장착
    /// </summary>
    /// <param name="item"></param>
    /// <param name="playSound"></param>
    public void SetEquipment(Item item, bool playSound = false)
    {
        if (item == null)
            return;

        InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);
        EquipmentTableCell tbCell = itemTbCell as EquipmentTableCell;

        if (tbCell == null)
            return;

        if (usingWeapon != null)
        {
            InventoryItemTableCell usingWeaponTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(usingWeapon.Code);
            EquipmentTableCell parsedUsingWeaponTbCell = usingWeaponTbCell as EquipmentTableCell;
            if (tbCell.equipmentType == EquipmentTableCell.EquipmentType.Weapon 
                && tbCell.equipmentTag != parsedUsingWeaponTbCell.equipmentTag)
                UIManager.Instance.SkillActionBar.UnSetAll();
        }

        switch (tbCell.equipmentTag)
        {
            case EquipmentTableCell.EquipmentTag.Sword:
                if (usingWeapon != null)
                    UnSetEquipment(usingWeapon, false);
                usingWeapon = item;
                break;
            case EquipmentTableCell.EquipmentTag.Bow:
                if (usingWeapon != null)
                    UnSetEquipment(usingWeapon, false);
                usingWeapon = item;
                break;
            case EquipmentTableCell.EquipmentTag.Wand:
                if (usingWeapon != null)
                    UnSetEquipment(usingWeapon, false);
                usingWeapon = item;
                break;
            case EquipmentTableCell.EquipmentTag.Helmet:
                if (usingHelmet != null)
                    UnSetEquipment(usingHelmet, false);
                usingHelmet = item;
                break;
            case EquipmentTableCell.EquipmentTag.Armor:
                if (usingArmor != null)
                    UnSetEquipment(usingArmor, false);
                usingArmor = item;
                break;
            case EquipmentTableCell.EquipmentTag.Pants:
                if (usingPants != null)
                    UnSetEquipment(usingPants, false);
                usingPants = item;
                break;
            case EquipmentTableCell.EquipmentTag.Boots:
                if (usingBoots != null)
                    UnSetEquipment(usingBoots, false);
                usingBoots = item;
                break;
            case EquipmentTableCell.EquipmentTag.Ring:
                if (usingRing_01 == null)
                    usingRing_01 = item;
                else if (usingRing_02 == null)
                    usingRing_02 = item;
                else
                {
                    UnSetEquipment(usingRing_01, false);
                    usingRing_01 = item;
                }
                break;
            case EquipmentTableCell.EquipmentTag.Neckless:
                if (usingNeckless != null)
                    UnSetEquipment(usingNeckless, false);
                usingNeckless = item;
                break;
        }

        EquipmentItem equipmentItem = item as EquipmentItem;
        equipmentItem.IsEquiped = true;

        UIManager.Instance.InventoryPopup.SetEquipmentUI(item);
        PlaySceneMain.Instance.Player.ChangePlayerEquipment(item, true);

        if (playSound)
            SoundController.Instance.PlaySfx(equipSound);

        EventHandler.OnItemsUpdated?.Invoke();
    }
    
    /// <summary>
    /// 아이템 해제
    /// </summary>
    /// <param name="item"></param>
    /// <param name="initializeSkillSet"></param>
    public void UnSetEquipment(Item item, bool initializeSkillSet = true)
    {
        InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);
        EquipmentTableCell tbCell = itemTbCell as EquipmentTableCell;

        if (tbCell == null)
            return;

        switch (tbCell.equipmentTag)
        {
            case EquipmentTableCell.EquipmentTag.Sword:
                usingWeapon = null;
                break;
            case EquipmentTableCell.EquipmentTag.Bow:
                usingWeapon = null;
                break;
            case EquipmentTableCell.EquipmentTag.Wand:
                usingWeapon = null;
                break;
            case EquipmentTableCell.EquipmentTag.Helmet:
                usingHelmet = null;
                break;
            case EquipmentTableCell.EquipmentTag.Armor:
                usingArmor = null;
                break;
            case EquipmentTableCell.EquipmentTag.Pants:
                usingPants = null;
                break;
            case EquipmentTableCell.EquipmentTag.Boots:
                usingBoots = null;
                break;
            case EquipmentTableCell.EquipmentTag.Ring:
                if (usingRing_01 == item)
                    usingRing_01 = null;
                else
                    usingRing_02 = null;
                break;
            case EquipmentTableCell.EquipmentTag.Neckless:
                usingNeckless = null;
                break;
        }

        EquipmentItem equipmentItem = item as EquipmentItem;
        equipmentItem.IsEquiped = false;

        UIManager.Instance.InventoryPopup.UnSetEquipmentUI(item);
        PlaySceneMain.Instance.Player.ChangePlayerEquipment(item, false);

        if (initializeSkillSet 
            && tbCell.equipmentType == EquipmentTableCell.EquipmentType.Weapon)
            UIManager.Instance.SkillActionBar.UnSetAll();

        EventHandler.OnItemsUpdated?.Invoke();
    }

    /// <summary>
    /// Get/Set의 Get 처럼 사용하는 메서드.
    /// </summary>
    public List<Item> GetItem(int itemCode)
    {
        return playerInventory.ItemContainer.GetItem(itemCode);
    }

    /// <summary>
    /// 인벤토리 조건 정렬
    /// </summary>
    public void SortItems()
    {
        playerInventory.ItemContainer.SortItems();
    }

    /// <summary>
    /// 인벤토리가 가득 찼는지 반환
    /// </summary>
    /// <returns></returns>
    public bool isFull()
    {
        return playerInventory.ItemContainer.isFull();
    }
}