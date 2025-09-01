using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 제작 재료 표시 셀
/// </summary>
public class UICraftingMaterialCell : MonoBehaviour
{
    [SerializeField] private Button thisButton;
    [SerializeField] private Image thisImage;
    [SerializeField] private TMPro.TextMeshProUGUI itemRequiredNumber;
    [SerializeField] private Transform itemDescriptionTr;
    private InventoryItemTableCell tbCell;
    private int requiredNumber;

    private void Awake()
    {
        if (!thisButton)
            thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnClickButton);
    }
    private void OnEnable()
    {
        EventHandler.OnItemsUpdated += UpdateButton;
    }

    private void OnDisable()
    {
        EventHandler.OnItemsUpdated -= UpdateButton;
    }

    public void UpdateButton()
    {
        var itemList = InventoryManager.Instance.GetItem(tbCell.Code);

        int totalItemCount = 0;
        foreach (var item in itemList)
        {
            totalItemCount += item.Quantity;
        }

        bool isNotEnough = totalItemCount < requiredNumber;
        string hexColor = ColorUtility.ToHtmlStringRGB(isNotEnough ? Color.red : Color.green);

        itemRequiredNumber.text = $"<color=#{hexColor}>{totalItemCount + "/" + requiredNumber}</color>";
    }

    public void SetButton(InventoryItemTableCell tbCell, int requiredNumber)
    {
        this.tbCell = tbCell;
        this.requiredNumber = requiredNumber;
        thisImage.sprite = tbCell.Icon;
        UpdateButton();
        gameObject.SetActive(true);
    }

    private void OnClickButton()
    {
        UIManager.Instance.ShowSimpleItemDescription(tbCell.Name, itemDescriptionTr.position);
    }
}
