using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 강화 시도 시 나타나는 애니메이션 팝업
/// </summary>
public class UIEnhanceAnimationPopup : UIPanel
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip enhanceSound;

    private void AnimationSoundEvent()
    {
        SoundController.Instance.PlaySfx(enhanceSound);
    }

    private void AnimationDoneEvent()
    {
        Close();
    }
}
