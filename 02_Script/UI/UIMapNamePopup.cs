using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 맵 진입 시 맵 이름을 나타내 주는 UI
/// </summary>
public class UIMapNamePopup : UIPanel
{
    public enum State
    {
        None, Show, Idle, Hide
    }
    private State state = State.None;

    [SerializeField] private Image backGround;
    [SerializeField] private TMPro.TextMeshProUGUI mapNameText;
    [SerializeField, Header("(애니메이션)알파값 변하는 속도")] private float alphaChangeSpeed = 0.5f;
    [SerializeField, Header("알파값이 변하는 도중 불변하는 시간")] private float IdleTime = 1f;
    [SerializeField, Header("타겟 알파값")] private float targetAlpha = .5f;
    private float currentAlpha = 0;
    private float passedTime;

    private void Update()
    {
        StateProcess();
    }

    public void Open(string mapName, UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);
        SetText(mapName);
        ChangeState(State.Show);
    }

    public override void Close()
    {
        base.Close();
        ChangeState(State.None);
    }

    private void StateProcess()
    {
        switch (state)
        {
            case State.None:
                break;
            case State.Show:
                currentAlpha += Time.deltaTime * alphaChangeSpeed;
                mapNameText.alpha = currentAlpha;
                backGround.color = new Color(0f, 0f, 0f, currentAlpha);

                if (currentAlpha >= targetAlpha)
                    ChangeState(State.Idle);
                break;
            case State.Idle:
                passedTime += Time.deltaTime;

                if (passedTime >= IdleTime)
                    ChangeState(State.Hide);
                break;
            case State.Hide:
                currentAlpha -= Time.deltaTime * alphaChangeSpeed;
                mapNameText.alpha = currentAlpha;
                backGround.color = new Color(0f, 0f, 0f, currentAlpha);

                if (currentAlpha <= 0f)
                    Close();
                break;
        }
    }

    public void ChangeState(State s)
    {
        if (state == s)
            return;

        state = s;

        switch (state)
        {
            case State.None:
                break;
            case State.Show:
                currentAlpha = 0f;
                backGround.color = new Color (1f, 1f, 0f);
                mapNameText.alpha = currentAlpha;
                break;
            case State.Idle:
                passedTime = 0f;
                break;
            case State.Hide:
                break;
        }
    }

    private void SetText(string mapName)
    {
        mapNameText.text = mapName;
    }
}
