using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// 플레이어 이동을 위한 조이스틱
/// </summary>
public class JoyStick : Singleton<JoyStick>, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField, Header("스틱(핸들) Recttransform")] private RectTransform stickRt;
    [SerializeField, Header("스틱 배경 ")] private RectTransform backGroundRt;
    [SerializeField, Header("스틱 이미지")] private Image stickImage;
    [SerializeField, Header("스틱 배경 이미지")] private Image backGroundImage;
    [SerializeField, Header("스틱 최대 범위(중심으로부터 거리)")] private float stickRange = 80f;
    [SerializeField, Header("입력 방향")] private Vector2 inputDirection;
    [SerializeField, Header("조이스틱 입력중인지")] private bool isMoving;

    public Vector2 InputDirection { get => inputDirection; }
    public bool IsMoving { get => isMoving; }

    private void Awake()
    {
        ResetJoyStick();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        backGroundRt.anchoredPosition = eventData.position;
        stickImage.enabled = true;
        backGroundImage.enabled = true;
        isMoving = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dir = eventData.position - backGroundRt.anchoredPosition;
        Vector2 normalizedDir = dir.normalized;

        var clampedDir = dir.magnitude < stickRange ? dir
            : normalizedDir * stickRange;

        stickRt.anchoredPosition = clampedDir;
        inputDirection = normalizedDir;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        inputDirection = Vector2.zero;
        isMoving = false;
        EventHandler.OnJoyStickDragEnd?.Invoke();
        ResetJoyStick();
    }

    private void ResetJoyStick()
    {
        stickRt.anchoredPosition = Vector3.zero;
        backGroundRt.anchoredPosition = Vector2.zero;
        stickImage.enabled = false;
        backGroundImage.enabled = false;
    }
}
