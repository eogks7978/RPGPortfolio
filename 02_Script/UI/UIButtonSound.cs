using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버튼을 눌렀을 때 소리나게 하는 클래스
/// </summary>
public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;

    public void Sound()
    {
        SoundController.Instance.PlaySfx(clickSound);
    }
}
