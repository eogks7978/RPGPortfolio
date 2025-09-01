using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 캐릭터 피격 시 머리위에 뜨는 데미지 UI의 부모
/// </summary>
public class CharacterAttackedUI : MonoBehaviour
{
    public void SetDamage(float value, Color textColor)
    {
        GameObject obj = ObjectPoolManager.Instance.GetObjectInMainPool(
            ObjectManager.Instance.FindPoolInEtcMainObjectToPool(typeof(CharacterAttackUICell)).key.name);

        if (obj)
        {
            TextMeshProUGUI tmp = obj.GetComponent<CharacterAttackUICell>().TextMeshPro;

            if (tmp)
            {
                tmp.text = value.ToString();
                tmp.color = textColor;
            }
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
        }
    }
}
