using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OptionData
{
    public float CameraSettingValue;
    public float BgmSettingValue;
    public float SfxSettingValue;

    public OptionData(
        float cameraSettingValue,
        float bgmSettingValue,
        float sfxSettingValue)
    {
        CameraSettingValue = cameraSettingValue;
        BgmSettingValue = bgmSettingValue;
        SfxSettingValue = sfxSettingValue;
    }
}
