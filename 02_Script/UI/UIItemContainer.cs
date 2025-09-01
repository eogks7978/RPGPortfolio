using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 가지고 있는 아이템을 보여주는 팝업(다른 팝업들과 함께 Open될 때가 많을 듯)
/// </summary>
public class UIItemContainer : MonoBehaviour
{
    [SerializeField, Header("아이템 정리 버튼")] private Button sortButton;
    [SerializeField, Header("보유 돈 Text")] private TMPro.TextMeshProUGUI moneyText;
    [SerializeField, Header("ScrollView의 Content")] private RectTransform contentRt;
    [SerializeField] private RectTransform popupBackgroundRt;
    [SerializeField, Header("인스터스화 할 아이템 슬롯 뼈대 obj")] private InventorySlot inventoryCell;
    public RectTransform PopupBackgroundRt => popupBackgroundRt;

    private void Awake()
    {
        sortButton.onClick.AddListener(SortItems);
    }

    private void OnEnable()
    {
        EventHandler.OnMoneyEarned += SetMoneyUI;
        contentRt.localPosition = Vector3.zero;
        UpdatePopup();
    }

    private void OnDisable()
    {
        EventHandler.OnMoneyEarned -= SetMoneyUI;
    }

    public void Initialize(int itemCellCount)
    {
        for (int i = 0; i < itemCellCount; i++)
        {
            InventorySlot cell = Instantiate(inventoryCell);
            cell.transform.SetParent(contentRt);
            cell.transform.localPosition = Vector3.zero;
            cell.transform.localScale = Vector3.one;
        }
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void UpdatePopup()
    {
        SetMoneyUI();
    }

    public void SetMoneyUI(int money = 0)
    {
        moneyText.text = InventoryManager.Instance.Money.ToString();
    }

    public void SortItems()
    {
        InventoryManager.Instance.SortItems();
    }
}
