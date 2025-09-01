using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 간단한 알림을 표시하기 위한 UI
/// </summary>
public class UISimpleNotificationCell : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI notificationTMP;

    public TextMeshProUGUI NotificationTMP => notificationTMP;

    public void OnAnimDone()
    {
        ObjectPoolManager.Instance.ReturnObjectToMainPool(gameObject);
    }
}
