using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 팝업이지만 단일로는 장비창 UI
/// </summary>
public class UIInventoryPopup : UIPanel
{
    [SerializeField] private UIEquipmentCell weaponSlotImage;
    [SerializeField] private UIEquipmentCell helmetSlotImage;
    [SerializeField] private UIEquipmentCell armorSlotImage;
    [SerializeField] private UIEquipmentCell pantsSlotImage;
    [SerializeField] private UIEquipmentCell bootsSlotImage;
    [SerializeField] private UIEquipmentCell necklessSlotImage;
    [SerializeField] private UIEquipmentCell ringSlotImage_01;
    [SerializeField] private UIEquipmentCell ringSlotImage_02;
    [SerializeField] private UIItemContainer itemContainerUI;
    [SerializeField] private RectTransform popupBackgroundRt;

    public RectTransform PopupBackgroundRt => popupBackgroundRt;

    public override void Open(UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);
        // ItemContainerUI와 같이 Open
        UIManager.Instance.OpenItemContainer();
    }

    public override void Close()
    {
        base.Close();
        // ItemContainerUI와 같이 Close
        UIManager.Instance.CloseItemContainer();
    }

    public void SetEquipmentUI(Item item)
    {
        InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);
        EquipmentTableCell tbCell = itemTbCell as EquipmentTableCell;

        if (tbCell == null)
            return;

        switch (tbCell.equipmentTag)
        {
            case EquipmentTableCell.EquipmentTag.Sword:
                weaponSlotImage.SetEquipment(item);
                break;
            case EquipmentTableCell.EquipmentTag.Bow:
                weaponSlotImage.SetEquipment(item);
                break;
            case EquipmentTableCell.EquipmentTag.Wand:
                weaponSlotImage.SetEquipment(item);
                break;
            case EquipmentTableCell.EquipmentTag.Helmet:
                helmetSlotImage.SetEquipment(item);
                break;
            case EquipmentTableCell.EquipmentTag.Armor:
                armorSlotImage.SetEquipment(item);
                break;
            case EquipmentTableCell.EquipmentTag.Pants:
                pantsSlotImage.SetEquipment(item);
                break;
            case EquipmentTableCell.EquipmentTag.Boots:
                bootsSlotImage.SetEquipment(item);
                break;
            case EquipmentTableCell.EquipmentTag.Ring:
                if (ringSlotImage_01.ContainedItem == null)
                    ringSlotImage_01.SetEquipment(item);
                else if (ringSlotImage_02.ContainedItem == null)
                    ringSlotImage_02.SetEquipment(item);
                else
                {
                    ringSlotImage_01.SetEquipment(item);
                }
                break;
            case EquipmentTableCell.EquipmentTag.Neckless:
                necklessSlotImage.SetEquipment(item);
                break;
        }
    }

    public void UnSetEquipmentUI(Item item)
    {
        InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);
        EquipmentTableCell tbCell = itemTbCell as EquipmentTableCell;

        if (tbCell == null)
            return;

        switch (tbCell.equipmentTag)
        {
            case EquipmentTableCell.EquipmentTag.Sword:
                weaponSlotImage.UnsetEquipment();
                break;
            case EquipmentTableCell.EquipmentTag.Bow:
                weaponSlotImage.UnsetEquipment();
                break;
            case EquipmentTableCell.EquipmentTag.Wand:
                weaponSlotImage.UnsetEquipment();
                break;
            case EquipmentTableCell.EquipmentTag.Helmet:
                helmetSlotImage.UnsetEquipment();
                break;
            case EquipmentTableCell.EquipmentTag.Armor:
                armorSlotImage.UnsetEquipment();
                break;
            case EquipmentTableCell.EquipmentTag.Pants:
                pantsSlotImage.UnsetEquipment();
                break;
            case EquipmentTableCell.EquipmentTag.Boots:
                bootsSlotImage.UnsetEquipment();
                break;
            case EquipmentTableCell.EquipmentTag.Ring:
                if (ringSlotImage_01.ContainedItem == item)
                    ringSlotImage_01.UnsetEquipment();
                else if (ringSlotImage_02.ContainedItem == item)
                    ringSlotImage_02.UnsetEquipment();
                break;
            case EquipmentTableCell.EquipmentTag.Neckless:
                necklessSlotImage.UnsetEquipment();
                break;
        }
    }
}
