using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UInotificationButton : MonoBehaviour
{
    [SerializeField] private GameObject notificationUI;
    [SerializeField] private bool isOn;

    public bool IsOn => isOn;

    protected virtual void SetNotificationUI(bool isOn)
    {
        if (this.isOn == isOn)
            return;

        notificationUI.SetActive(isOn);
        this.isOn = isOn;
    }

    public virtual void UpdateButton()
    {
        EventHandler.OnMainUIButtonUpdated?.Invoke();
    }
}
