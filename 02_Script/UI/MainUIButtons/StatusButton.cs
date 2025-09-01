using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusButton : UInotificationButton
{
    [SerializeField] private ImageBlinker statNotiObj;
    private void Awake()
    {
        EventHandler.OnPlayerStatChanged += UpdateButton;
    }

    public override void UpdateButton()
    {
        bool isStatPointAvailable = PlaySceneMain.Instance.Player.StatPoint > 0;
        SetNotificationUI(isStatPointAvailable ? true : false);
        statNotiObj.Blink(isStatPointAvailable);
        base.UpdateButton();
    }
}
