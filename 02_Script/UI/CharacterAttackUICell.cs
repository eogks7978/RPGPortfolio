using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 캐릭터 피격 시 머리위에 뜨는 데미지 UI
/// </summary>
public class CharacterAttackUICell : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    public TextMeshProUGUI TextMeshPro => textMeshPro;

    public void OnAnimDone()
    {
        ObjectPoolManager.Instance.ReturnObjectToMainPool(gameObject);
    }
}
