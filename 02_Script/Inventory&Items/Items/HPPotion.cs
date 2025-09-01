using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPotion : Potion
{
    public override void UseItem(Action<bool> cbDone)
    {
        PlaySceneMain.Instance.Player.SetStatus(new Dictionary<string, float> { { nameof(PlaySceneMain.Instance.Player.CurrentHealth), value } });
        SoundController.Instance.PlaySfx(sound);
        cbDone?.Invoke(true);
    }
}
