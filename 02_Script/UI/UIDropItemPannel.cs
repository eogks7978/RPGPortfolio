using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Always On UI. æ∆¿Ã≈€ »πµÊ ∏Ò∑œ¿ª ∫∏ø©¡‹
/// </summary>
public class UIDropItemPannel : MonoBehaviour
{
    [SerializeField] private int maxTextCount;
    [SerializeField] private string textGet;
    List<GameObject> managedCellList = new List<GameObject>();

    public void SetText(string itemTextToShow)
    {
        var pooledObjName = ObjectManager.Instance.FindPoolInEtcMainObjectToPool(typeof(DropItemUICell)).key.name;
        GameObject obj = ObjectPoolManager.Instance.GetObjectInMainPool(pooledObjName);

        if (obj == null)
            return;

        managedCellList.Add(obj);

        UpdateTexts();

        obj.transform.SetParent(transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.SetAsFirstSibling();

        TMPro.TextMeshProUGUI text = obj.GetComponent<TMPro.TextMeshProUGUI>();
        text.text = itemTextToShow + " " + textGet;
    }

    public void UpdateTexts()
    {
        if (managedCellList.Count > maxTextCount)
        {
            GameObject obj = managedCellList.First();

            if (obj)
            {
                managedCellList.Remove(obj);
                ObjectPoolManager.Instance.ReturnObjectToMainPool(obj);
            }
        }
    }
}
