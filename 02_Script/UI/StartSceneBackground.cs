using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// StartScene 움직이는 배경화면
/// </summary>
public class StartSceneBackground : MonoBehaviour
{
    public enum State
    {
        None, MoveToPoint1, MoveToPoint2, MoveToPoint3, MoveToPoint4
    }
    public State state = State.None;

    [SerializeField] private SpriteRenderer thisRenderer;
    [SerializeField] private float moveSpeed;
    private float cameraHalfWidth;
    private float cameraHalfHeight;
    private float thisRendererHalfSizeX;
    private float thisRendererHalfSizeY;
    private Vector3 targetPos1;
    private Vector3 targetPos2;
    private Vector3 targetPos3;
    private Vector3 targetPos4;

    private void Awake()
    {
        if (thisRenderer == null)
            thisRenderer = GetComponent<SpriteRenderer>();

        Initialize();
        ChangeState(State.MoveToPoint1);
    }

    private void Initialize()
    {
        cameraHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        cameraHalfHeight = Camera.main.orthographicSize;
        thisRendererHalfSizeX = thisRenderer.bounds.size.x / 2f;
        thisRendererHalfSizeY = thisRenderer.bounds.size.y / 2f;
        targetPos1 = new Vector2(cameraHalfWidth - thisRendererHalfSizeX, cameraHalfHeight - thisRendererHalfSizeY);
        targetPos2 = new Vector2(thisRendererHalfSizeX - cameraHalfWidth, cameraHalfHeight - thisRendererHalfSizeY);
        targetPos3 = new Vector2(thisRendererHalfSizeX - cameraHalfWidth, thisRendererHalfSizeY - cameraHalfHeight);
        targetPos4 = new Vector2(cameraHalfWidth - thisRendererHalfSizeX, thisRendererHalfSizeY - cameraHalfHeight);

        transform.position = targetPos4;
    }

    void Update()
    {
        StateProcess();
    }

    private void StateProcess()
    {
        switch (state)
        {
            case State.None:
                break;
            case State.MoveToPoint1:
                transform.position = Vector2.MoveTowards(transform.position, targetPos1, moveSpeed * Time.deltaTime);
                if (transform.position == targetPos1)
                    ChangeState(State.MoveToPoint2);
                break;
            case State.MoveToPoint2:
                transform.position = Vector2.MoveTowards(transform.position, targetPos2, moveSpeed * Time.deltaTime);
                if (transform.position == targetPos2)
                    ChangeState(State.MoveToPoint3);
                break;
            case State.MoveToPoint3:
                transform.position = Vector2.MoveTowards(transform.position, targetPos3, moveSpeed * Time.deltaTime);
                if (transform.position == targetPos3)
                    ChangeState(State.MoveToPoint4);
                break;
            case State.MoveToPoint4:
                transform.position = Vector2.MoveTowards(transform.position, targetPos4, moveSpeed * Time.deltaTime);
                if (transform.position == targetPos4)
                    ChangeState(State.MoveToPoint1);
                break;
        }
    }

    private void ChangeState(State s)
    {
        if (state == s)
            return;

        state = s;

        switch (s)
        {
            case State.None:
                break;
            case State.MoveToPoint1:
                break;
            case State.MoveToPoint2:
                break;
            case State.MoveToPoint3:
                break;
            case State.MoveToPoint4:
                break;
        }
    }
}
