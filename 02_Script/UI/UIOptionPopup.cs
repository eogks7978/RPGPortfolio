using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 옵션 팝업
/// </summary>
public class UIOptionPopup : UIPanel
{
    [SerializeField] private Slider cameraSettingSlider;
    [SerializeField] private Slider bgmSettingSlider;
    [SerializeField] private Slider sfxSettingSlider;
    [SerializeField] private Button exitGameButton;
    [SerializeField] private Button saveGameButton;
    [SerializeField] private RectTransform popupBackgroundRt;

    public RectTransform PopupBackgroundRt => popupBackgroundRt;

    public float CameraSettingValue => cameraSettingSlider.value;
    public float BgmSettingValue => bgmSettingSlider.value;
    public float SfxSettingValue => sfxSettingSlider.value;

    private new void Awake()
    {
        base.Awake();

        cameraSettingSlider.onValueChanged.AddListener(OnCameraSliderValueChanged);
        bgmSettingSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
        sfxSettingSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);
        exitGameButton.onClick.AddListener(OnClickExitGameButton);
        saveGameButton.onClick.AddListener(OnClickSaveButton);
    }

    public override void Open(UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);

        cameraSettingSlider.value = CameraController.Instance.Camera.orthographicSize;
        bgmSettingSlider.value = SoundController.Instance.BgmAudioSource.volume;
        sfxSettingSlider.value = SoundController.Instance.SfxAudioSource.First().volume;
    }

    private void OnCameraSliderValueChanged(float value)
    {
        float replacedValue = Mathf.Round(value * 2) / 2f;
        cameraSettingSlider.value = replacedValue;

        CameraController.Instance.ChangeCameraSize(replacedValue);
    }

    private void OnBgmSliderValueChanged(float value)
    {
        SoundController.Instance.SetBgmVolume(value);
    }

    private void OnSfxSliderValueChanged(float value)
    {
        SoundController.Instance.SetSfxVolume(value);
    }

    private void OnClickExitGameButton()
    {
        UIManager.Instance.OpenNotificationPopup("종료하시겠습니까?", true, "알림", "예", "아니오",
            (resultObj) =>
            {
                var result = resultObj as UINotificationPopup.Results;
                if (result.isConfirm)
                {
                    Application.Quit();
                }
            });
    }

    private void OnClickSaveButton()
    {
        PlaySceneMain.Instance.SaveGameData((success) =>
        {
            if (success)
            {
                UIManager.Instance.OpenNotificationPopup("저장되었습니다.");
            }
        });
    }
}
