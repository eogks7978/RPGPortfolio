using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어 HP 바
/// </summary>
public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private IDamagable iDamagable;
    [SerializeField] private TMPro.TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Initailize(IDamagable iDamagable)
    {
        this.iDamagable = iDamagable;
        UpdateSlider();
    }

    public void UpdateSlider()
    {
        slider.value = iDamagable.CurrentHealth / iDamagable.MaxHealth;
        textMeshPro.text = iDamagable.CurrentHealth.ToString("F0") + " / " + iDamagable.MaxHealth;
    }
}
