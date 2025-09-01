using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Splash 이후 최초 씬 메인
/// </summary>
public class StartSceneMain : MonoBehaviour
{
    [SerializeField, Header("게임 시작 버튼")] private Button startButton;
    [SerializeField, Header("게임 종료 버튼")] private Button exitButton;
    [SerializeField, Header("씬 Bgm")] private AudioClip bgmClip;

    // ---- UI ----
    // UIManager를 사용하기엔 씬에 안쓰는 UI가 너무 많아서 따로 프리펩 할당으로 대체
    [SerializeField] private UINotificationPopup _notificationPopup;
    //-------------

    private void Awake()
    {
        Application.targetFrameRate = 60;

        exitButton.onClick.AddListener(OpenGameExitPopup);
        startButton.onClick.AddListener(LoadNextScene);
        Initialize();
    }

    private void Initialize()
    {
        SoundController.Instance.PlayBgm(bgmClip);
    }

    private void OpenGameExitPopup()
    {
        OpenNotificationPopup("종료하시겠습니까?", true, "게임 종료", "예", "아니오",
            (resultObj) =>
            {
                var result = resultObj as UINotificationPopup.Results;

                if (result.isConfirm)
                {
                    Application.Quit();
                }
            });
    }

    private void LoadNextScene()
    {
        ChangeScene(1);
    }

    private void ChangeScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void OpenNotificationPopup(
    string notification,
    bool noButtonOn = false,
    string titleText = "",
    string okButtonText = "",
    string noButtonText = "",
    UnityAction<object> cbClose = null)
    {
        if (_notificationPopup.isActiveAndEnabled)
            return;

        _notificationPopup.Open(
             notification,
             noButtonOn,
             titleText = "",
             okButtonText = "",
             noButtonText = "",
             (resultObj) =>
             {
                 cbClose?.Invoke(resultObj);
             });
    }
}
