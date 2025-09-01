using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 간단한 알림을 표시하기 위한 UI
/// </summary>
public class UISimpleNotification : MonoBehaviour
{
    [SerializeField] private Transform showPositionTr;

    public void Show(string notification, Color color)
    {
        GameObject objInPool = ObjectManager.Instance.FindPoolInEtcMainObjectToPool(typeof(UISimpleNotificationCell)).key;
        if (!objInPool) return;

        GameObject resultObj = ObjectPoolManager.Instance.GetObjectInMainPool(objInPool.name);
        if (resultObj == null) return;

        resultObj.GetComponent<UISimpleNotificationCell>().NotificationTMP.text = notification;
        resultObj.GetComponent<UISimpleNotificationCell>().NotificationTMP.color = color;

        resultObj.transform.SetParent(showPositionTr);
        resultObj.transform.position = showPositionTr.position;
        resultObj.transform.localScale = Vector3.one;
    }
}
