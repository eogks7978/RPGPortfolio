using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 플레이어가 죽으면 나오는 팝업
/// </summary>
public class UIPlayerDeadPopup : UIPanel
{
    [SerializeField] private Button resurrectionButton;
    [SerializeField] private TMPro.TextMeshProUGUI expDecreaseText;
    [SerializeField] private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        resurrectionButton.onClick.AddListener(ResurrectionPlayer);
    }

    public void Open(float expDecrease, UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);
        anim.Rebind();
        expDecreaseText.text = Mathf.Abs(expDecrease).ToString("F1");
    }

    private void ResurrectionPlayer()
    {
        PlaySceneMain.Instance.Player.Resurrection();
        UIManager.Instance.CloseAll();
    }
}
