using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 Level 표시 바
/// </summary>
public class UILevelBar : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textMeshPro;
    private Player player;

    private void OnEnable()
    {
        EventHandler.OnPlayerLevelUp += UpdateUI;
    }

    private void OnDisable()
    {
        EventHandler.OnPlayerLevelUp -= UpdateUI;
    }

    public void Initailize(Player player)
    {
        this.player = player;
        UpdateUI();
    }

    private void UpdateUI()
    {
        textMeshPro.text = "Lv " + player.Level.ToString();
    }
}
