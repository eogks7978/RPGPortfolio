using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpEffect : SpriteEffect
{
    [SerializeField] private AudioClip levelUpSoundClip;
    protected override void Initialize()
    {
        base.Initialize();
        SoundController.Instance.PlaySfx(levelUpSoundClip);
    }
}
