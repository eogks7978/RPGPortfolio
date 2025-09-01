using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 메인 Light 관리자
/// </summary>
public class LightController : MonoBehaviour
{
    #region singleton
    private static LightController instance;
    public static LightController Instance => instance;
    #endregion

    [SerializeField] private Light2D mainLight;

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

    public Light2D MainLight { get => mainLight; }

    public void ChangeLight(float brightness)
    {
        if (mainLight)
        {
            mainLight.intensity = brightness;
        }
    }
}
