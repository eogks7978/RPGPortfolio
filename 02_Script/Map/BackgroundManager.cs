using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 타일맵 뒤에 깔리는 맵 관리자
/// </summary>
public class BackgroundManager : MonoBehaviour
{
    [SerializeField, Header("백그라운드를 적용 할 랜더러")] private SpriteRenderer bgSprite;
    [SerializeField, Header("백그라운드를 보여 줄 카메라")] private Camera scrollingBgCamera;

    private bool useScrollingBg;

    private float mainCameraHalfHeight;
    private float mainCameraHalfWidth;
    private float mainCameraMinPosX;
    private float mainCameraMinPosY;
    private float mainCameraMaxPosX;
    private float mainCameraMaxPosY;

    private float scrollingCameraHalfHeight;
    private float scrollingCameraHalfWidth;
    private float scrollingCameraMinPosX;
    private float scrollingCameraMinPosY;
    private float scrollingCameraMaxPosX;
    private float scrollingCameraMaxPosY;

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
        MoveBackgroundCamera();
    }

    private void Initialize()
    {
        if (!scrollingBgCamera) return;
    }

    public void SetScrollingBackground(Sprite backGroundSprite)
    {
        if (backGroundSprite == null)
        {
            useScrollingBg = false;
            scrollingBgCamera.gameObject.SetActive(false);
            bgSprite.gameObject.SetActive(false);
            return;
        }

        bgSprite.sprite = backGroundSprite;

        Vector2 targetSize = new Vector2(
            scrollingBgCamera.orthographicSize * scrollingBgCamera.aspect * 1.2f,
            scrollingBgCamera.orthographicSize * 1.2f);

        float magnificationX = targetSize.x / backGroundSprite.bounds.size.x;
        float magnificationY = targetSize.y / backGroundSprite.bounds.size.y;

        bgSprite.transform.localScale =
            new Vector3(
                2 * magnificationX,
                2 * magnificationY);

        UpdateInfo();

        bgSprite.gameObject.SetActive(true);
        scrollingBgCamera.gameObject.SetActive(true);
        useScrollingBg = true;
    }

    private void MoveBackgroundCamera()
    {
        if (!useScrollingBg) return;

        float ratioX = (Camera.main.transform.position.x - mainCameraMinPosX) /
            (mainCameraMaxPosX - mainCameraMinPosX);
        float ratioY = (Camera.main.transform.position.y - mainCameraMinPosY) /
            (mainCameraMaxPosY - mainCameraMinPosY);

        scrollingBgCamera.transform.position = new Vector3(
            scrollingCameraMinPosX + (scrollingCameraMaxPosX - scrollingCameraMinPosX) * ratioX,
            scrollingCameraMinPosY + (scrollingCameraMaxPosY - scrollingCameraMinPosY) * ratioY,
            -10f);
    }

    /// <summary>
    /// 카메라 설정시 한 번 씩만 호출되는 메서드
    /// </summary>
    private void UpdateInfo()
    {
        mainCameraHalfHeight = Camera.main.orthographicSize;
        mainCameraHalfWidth = Camera.main.aspect * mainCameraHalfHeight;

        mainCameraMinPosX = MapManager.Instance.CurrentMap.transform.position.x
            + mainCameraHalfWidth
            + MapManager.Instance.CurrentMap.CameraOffsetMinX;
        mainCameraMinPosY = MapManager.Instance.CurrentMap.transform.position.y
            + mainCameraHalfHeight
            + MapManager.Instance.CurrentMap.CameraOffsetMinY;
        mainCameraMaxPosX = MapManager.Instance.CurrentMap.transform.position.x
            + MapManager.Instance.CurrentMap.BackTilemap.size.x
            - mainCameraHalfWidth
            + MapManager.Instance.CurrentMap.CameraOffsetMaxX;
        mainCameraMaxPosY = MapManager.Instance.CurrentMap.transform.position.y
            + MapManager.Instance.CurrentMap.BackTilemap.size.y
            - mainCameraHalfHeight
            + MapManager.Instance.CurrentMap.CameraOffsetMaxY;

        scrollingCameraHalfHeight = scrollingBgCamera.orthographicSize;
        scrollingCameraHalfWidth = scrollingBgCamera.aspect * scrollingCameraHalfHeight;

        if (bgSprite && bgSprite.sprite != null)
        {
            scrollingCameraMinPosX = bgSprite.transform.position.x
                - bgSprite.sprite.bounds.size.x * bgSprite.transform.localScale.x / 2
                + scrollingCameraHalfWidth;
            scrollingCameraMinPosY = bgSprite.transform.position.y
                - bgSprite.sprite.bounds.size.y * bgSprite.transform.localScale.y / 2
                + scrollingCameraHalfHeight;
            scrollingCameraMaxPosX = scrollingCameraMinPosX
                + bgSprite.sprite.bounds.size.x * bgSprite.transform.localScale.x
                - scrollingCameraHalfWidth * 2;
            scrollingCameraMaxPosY = scrollingCameraMinPosY
                + bgSprite.sprite.bounds.size.y * bgSprite.transform.localScale.y
                - scrollingCameraHalfHeight * 2;
        }
    }
}
