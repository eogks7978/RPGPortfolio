using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 정보 알림 팝업
/// </summary>
public class UINotificationPopup : UIPanel
{
    [Serializable]
    public class Results
    {
        public bool isConfirm;
    }

    [SerializeField] private TMPro.TextMeshProUGUI notificationTMP;
    [SerializeField] private TMPro.TextMeshProUGUI titleTMP;
    [SerializeField] private TMPro.TextMeshProUGUI okButtonTMP;
    [SerializeField] private TMPro.TextMeshProUGUI noButtonTMP;

    [SerializeField] private string defalutTitleString;
    [SerializeField] private string defalutOkButtonString;
    [SerializeField] private string defalutNoButtonString;

    [SerializeField] private Button btnOk; 
    [SerializeField] private Button btnClose;

    protected override void Awake()
    {
        foreach (var button in closeButtons)
        {
            button.onClick.AddListener(OnClickCancel);
        }

        if (btnOk != null)
            btnOk.onClick.AddListener(OnClickConfirm);
        if (btnClose != null)
            btnClose.onClick.AddListener(OnClickCancel);
    }

    public void Open(
        string notification,
        bool noButtonOn = false,
        string titleText = "",
        string okButtonText = "",
        string noButtonText = "",
        UnityAction<object> cbClose = null)
    {
        notificationTMP.text = notification;
        titleTMP.text = titleText != "" ? titleText : defalutTitleString;
        okButtonTMP.text = okButtonText != "" ? okButtonText : defalutOkButtonString;
        noButtonTMP.text = noButtonText != "" ? noButtonText : defalutNoButtonString;

        btnClose.gameObject.SetActive(noButtonOn);

        base.Open(cbClose);
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
        Results result = new Results();
        result.isConfirm = isOK;
        _results = result;

        base.Close();
    }
}
