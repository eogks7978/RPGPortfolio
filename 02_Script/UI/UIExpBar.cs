using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어 경험치 바
/// </summary>
public class UIExpBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Player player;
    [SerializeField] private TMPro.TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Initailize(Player player)
    {
        this.player = player;
        UpdateSlider();
    }

    public void UpdateSlider()
    {
        slider.value = player.CurrentExp / player.MaxExp;
        textMeshPro.text = player.CurrentExp.ToString("F1") + " / " + player.MaxExp + " (" + (int)(player.CurrentExp / player.MaxExp * 100f) + "%" + ")";
    }
}
