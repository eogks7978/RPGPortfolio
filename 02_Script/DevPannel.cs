using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


public class DevPannel : MonoBehaviour
{
    [SerializeField] private TMP_InputField addItemNumberInputField;
    [SerializeField] private TMP_InputField addStatStringInputField;
    [SerializeField] private TMP_InputField addItemValueInputField;
    [SerializeField] private TMP_InputField addStatValueInputField;
    [SerializeField] private Button addItemButton;
    [SerializeField] private Button addStatButton;
    private bool isChanging;

    private void Awake()
    {
        gameObject.SetActive(false);

        addItemButton.onClick.AddListener(AddItem);
        addStatButton.onClick.AddListener(AddStat);

        addItemNumberInputField.onValueChanged.AddListener(ValidateInputItemCode);
        addStatStringInputField.onValueChanged.AddListener(ValidateInputStatName);
        addItemValueInputField.onValueChanged.AddListener(ValidateInputItemValue);
        addStatValueInputField.onValueChanged.AddListener(ValidateInputStatValue);
    }

    private void AddItem()
    {
        if (int.TryParse(addItemNumberInputField.text, out int value01))
        {
            InventoryItemTableCell tbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(value01);

            if (tbCell == null)
                return;

            if (int.TryParse(addItemValueInputField.text, out int value02))
            {
                Item createdItem = tbCell._ItemType switch
                {
                    InventoryItemTableCell.ItemType.Equipment => new EquipmentItem(tbCell.Code, value02),
                    InventoryItemTableCell.ItemType.Consumable => new ConsumableItem(tbCell.Code, value02),
                    InventoryItemTableCell.ItemType.Etc => new EtcItem(tbCell.Code, value02),
                    InventoryItemTableCell.ItemType.None => new Item(tbCell.Code, value02),
                    _ => new Item(tbCell.Code, value02)
                };

                InventoryManager.Instance.TryGetItem(createdItem, null);
            }
        }
    }

    private void AddStat()
    {
        if (float.TryParse(addStatValueInputField.text, out float value01))
        {
            PlaySceneMain.Instance.Player.SetStatus(new() { { addStatStringInputField.text, value01 } });
        }
    }

    void ValidateInputItemCode(string input)
    {
        if (isChanging)
            return;

        isChanging = true; // 이벤트가 중복 호출되지 않도록 플래그 설정

        // 값이 비어있지 않고, 숫자로 변환 가능한 경우
        if (!int.TryParse(input, out int value))
            addItemNumberInputField.text = string.Empty;

        isChanging = false; // 이벤트가 완료되면 플래그 해제
    }
    

    void ValidateInputStatName(string text)
    {
        if (isChanging)
            return;

        isChanging = true; // 이벤트가 중복 호출되지 않도록 플래그 설정

        string pattern = @"^[a-zA-Z]*$";
        if (!Regex.IsMatch(text, pattern))
        {
            // 유효하지 않으면 마지막 문자를 삭제
            text = text.Substring(0, text.Length - 1);
            addStatStringInputField.text = text;
        }

        isChanging = false; // 이벤트가 완료되면 플래그 해제
    }

    void ValidateInputItemValue(string text)
    {
        if (!int.TryParse(text, out int value))
            addItemValueInputField.text = string.Empty;
    }

    void ValidateInputStatValue(string text)
    {
        if (!int.TryParse(text, out int value))
            addStatValueInputField.text = string.Empty;
    }
}
