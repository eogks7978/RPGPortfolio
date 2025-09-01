using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어 마나 표시 UI 바
/// </summary>
public class UIManaBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private ISkillable iSkillable;
    [SerializeField] private TMPro.TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Initailize(ISkillable iSkillable)
    {
        this.iSkillable = iSkillable;
        UpdateSlider();
    }

    public void UpdateSlider()
    {
        slider.value = iSkillable.CurrentMana / iSkillable.MaxMana;
        textMeshPro.text = iSkillable.CurrentMana.ToString("F0") + " / " + iSkillable.MaxMana;
    }
}
