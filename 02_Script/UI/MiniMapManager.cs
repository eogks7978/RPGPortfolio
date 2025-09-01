using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미니맵 관리
/// </summary>
public class MiniMapManager : MonoBehaviour
{
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private Transform TargetTr;
    [SerializeField] private TMPro.TextMeshProUGUI mapNameText;

    private float miniMapCameraHalfHeight;
    private float miniMapCameraHalfWidth;
    private float miniMapCameraMinPosX;
    private float miniMapCameraMinPosY;
    private float miniMapCameraMaxPosX;
    private float miniMapCameraMaxPosY;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        EventHandler.OnCameraSizeChanged += UpdateInfo;
    }

    private void OnDisable()
    {
        EventHandler.OnCameraSizeChanged -= UpdateInfo;
    }

    private void Update()
    {
        MoveMinimapCamera();
    }

    private void Initialize()
    {
        if (!minimapCamera) return;
    }

    public void SetMiniMap(Transform target, string mapName)
    {
        TargetTr = target;
        if (mapName != null)
            mapNameText.text = mapName;
        UpdateInfo();

        minimapCamera.gameObject.SetActive(true);
    }

    private void MoveMinimapCamera()
    {
        if (!TargetTr) 
            return;

        float targetPostionX = TargetTr.position.x;
        float targetPostionY = TargetTr.position.y;

        if (targetPostionX < miniMapCameraMinPosX)
            targetPostionX = miniMapCameraMinPosX;
        else if (targetPostionX > miniMapCameraMaxPosX)
            targetPostionX = miniMapCameraMaxPosX;

        if (targetPostionY < miniMapCameraMinPosY)
            targetPostionY = miniMapCameraMinPosY;
        else if (targetPostionY > miniMapCameraMaxPosY)
            targetPostionY = miniMapCameraMaxPosY;

        minimapCamera.transform.position = new Vector3(
            targetPostionX,
            targetPostionY,
            -10f);
    }

    /// <summary>
    /// 카메라 설정이 변할 때 한 번만 실행되는 메서드
    /// </summary>
    private void UpdateInfo()
    {
        miniMapCameraHalfHeight = minimapCamera.orthographicSize;
        miniMapCameraHalfWidth = minimapCamera.aspect * miniMapCameraHalfHeight;

        miniMapCameraMinPosX = MapManager.Instance.CurrentMap.transform.position.x 
            + miniMapCameraHalfWidth 
            - MapManager.Instance.CurrentMap.CameraOffsetMinX;
        miniMapCameraMinPosY = MapManager.Instance.CurrentMap.transform.position.y 
            + miniMapCameraHalfHeight 
            - MapManager.Instance.CurrentMap.CameraOffsetMinY;
        miniMapCameraMaxPosX = miniMapCameraMinPosX
            + MapManager.Instance.CurrentMap.BackTilemap.size.x 
            - miniMapCameraHalfWidth * 2
            + MapManager.Instance.CurrentMap.CameraOffsetMaxX;
        miniMapCameraMaxPosY = miniMapCameraMinPosY
            + MapManager.Instance.CurrentMap.BackTilemap.size.y 
            - miniMapCameraHalfHeight * 2
            + MapManager.Instance.CurrentMap.CameraOffsetMaxY;
    }
}
