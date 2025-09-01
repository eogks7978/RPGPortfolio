using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 숫자를 입력받기 위한 InputField Popup
/// </summary>
public class UIInputFieldConfirmPopup : UIPanel
{
    [Serializable]
    public class Results
    {
        public int value;
    }

    [SerializeField] private TMPro.TextMeshProUGUI Title;
    [SerializeField] private TMP_InputField inputField;
    private int minValue;
    private int maxValue;

    [SerializeField] private Button btnOk;
    [SerializeField] private Button btnClose;

    private bool isChanging = false; // 이벤트 중복 호출 방지용 플래그

    private new void Awake()
    {
        foreach (var button in closeButtons)
        {
            button.onClick.AddListener(OnClickCancel);
        }

        if (btnOk != null)
            btnOk.onClick.AddListener(OnClickConfirm);
        if (btnClose != null)
            btnClose.onClick.AddListener(OnClickCancel);

        inputField.onValueChanged.AddListener(ValidateInput);
    }

    public void Open(string titleText, int minValue, int maxValue, UnityAction<object> cbClose = null)
    {
        Title.text = titleText;
        this.minValue = minValue;
        this.maxValue = maxValue;
        inputField.text = string.Empty;

        base.Open(cbClose);
    }

    // 입력값이 숫자인지 확인하고, 범위 내에 있는지 검사하는 메서드
    void ValidateInput(string input)
    {
        if (isChanging)
            return;

        isChanging = true; // 이벤트가 중복 호출되지 않도록 플래그 설정

        // 값이 비어있지 않고, 숫자로 변환 가능한 경우
        if (int.TryParse(input, out int value))
        {
            // 입력된 숫자가 범위 밖이라면 제한된 값으로 설정
            if (value < minValue)
            {
                inputField.text = minValue.ToString();
            }
            else if (value > maxValue)
            {
                inputField.text = maxValue.ToString();
            }
        }
        else
        {
            // 숫자가 아닌 값이 입력되면 빈 문자열로 초기화
            inputField.text = string.Empty;
        }

        isChanging = false; // 이벤트가 완료되면 플래그 해제
    }

    protected virtual void OnClickConfirm()
    {
        Close(true);
    }

    protected virtual void OnClickCancel()
    {
        Close(false);
    }

    protected void Close(bool isOK = false)
    {
        if (inputField.text.Equals(string.Empty) || !isOK)
        {
            base.Close();
            return;
        }

        int value = Int32.Parse(inputField.text);

        Results result = new Results();
        result.value = value;
        _results = result;

        base.Close();
    }
}
