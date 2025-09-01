using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPPotion : Potion
{
    public override void UseItem(Action<bool> cbDone)
    {
        PlaySceneMain.Instance.Player.SetStatus(new Dictionary<string, float> { { nameof(PlaySceneMain.Instance.Player.CurrentMana), value } });
        SoundController.Instance.PlaySfx(sound);
        cbDone?.Invoke(true);
    }
}
