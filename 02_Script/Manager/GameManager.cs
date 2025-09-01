using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton
    private static GameManager instance;
    public static GameManager Instance => instance;
    #endregion

    [SerializeField] private bool developmentBuild = false;
    public bool DevelopmentBuild => developmentBuild;

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
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
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
    }
    public void Initialize(OptionData optionData)
    {
        CameraController.Instance.Camera.orthographicSize = optionData.CameraSettingValue;
        SoundController.Instance.SetBgmVolume(optionData.BgmSettingValue);
        SoundController.Instance.SetSfxVolume(optionData.SfxSettingValue);
    }
}
