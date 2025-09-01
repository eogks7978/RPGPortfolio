using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 메인 카메라 컨트롤러
/// </summary>
public class CameraController : MonoBehaviour
{
    #region singleton
    private static CameraController instance;
    public static CameraController Instance => instance;
    #endregion

    [SerializeField] private Camera _camera;
    public Camera Camera { get => _camera; }

    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private Vector3 offset;
    private float limitMinX, limitMaxX, limitMinY, limitMaxY;
    private float cameraWidth;
    private float cameraHeight;

    private void Awake()
    {
        //if (instance == null)
        //{
        instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        Initialize();
    }

    private void Start()
    {
        cameraWidth = _camera.aspect * _camera.orthographicSize;
        cameraHeight = _camera.orthographicSize;
    }

    private void Update()
    {
        FollowTarget();
    }

    private void Initialize()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    private void FollowTarget()
    {
        if (target)
        {
            Vector3 desiredPosition = GetDesiredPosition();

            _camera.transform.position = Vector3.Lerp(_camera.transform.position, desiredPosition, Time.deltaTime * moveSpeed);
        }
    }

    public void ChangeTarget(
        Transform target,
        float limitMinX,
        float limitMaxX, 
        float limitMinY,
        float limitMaxY)
    {
        this.target = target;

        this.limitMinX = limitMinX;
        this.limitMaxX = limitMaxX;
        this.limitMinY = limitMinY;
        this.limitMaxY = limitMaxY;
    }

    /// <summary>
    /// 타겟을 애니메이션 없이 바로 비추는 메서드
    /// </summary>
    /// <param name="target"></param>
    public void ImmediatelyShowTarget(Transform target)
    {
        Vector2 targetPos = target.position;

        if (target.position.x < limitMinX + cameraWidth)
            targetPos.x = limitMinX + cameraWidth;
        else if (target.position.x > limitMaxX - cameraWidth)
            targetPos.x = limitMaxX - cameraWidth;
        if (target.position.y < limitMinY + cameraHeight)
            targetPos.y = limitMinY + cameraHeight;
        else if (target.position.y > limitMaxY - cameraHeight)
            targetPos.y = limitMaxY - cameraHeight;

        _camera.transform.position = targetPos;
    }

    public void ChangeCameraSize(float size)
    {
        if (_camera.orthographicSize == size) 
            return;

        _camera.orthographicSize = size;
        cameraWidth = _camera.aspect * _camera.orthographicSize;
        cameraHeight = _camera.orthographicSize;

        Vector3 desiredPosition = GetDesiredPosition();

        _camera.transform.position = desiredPosition;

        EventHandler.OnCameraSizeChanged?.Invoke();
    }

    /// <summary>
    /// 맵의 카메라 제한 위치와 카메라 크기를 고려 해서 보간된 카메라 위치
    /// </summary>
    /// <returns></returns>
    private Vector3 GetDesiredPosition()
    {
        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(target.position.x + offset.x,
            limitMinX + _camera.aspect * _camera.orthographicSize,
            limitMaxX - _camera.aspect * _camera.orthographicSize),
            Mathf.Clamp(target.position.y + offset.y,
            limitMinY + _camera.orthographicSize,
            limitMaxY - _camera.orthographicSize),
            -10);

        return desiredPosition;
    }
}
