using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : UInotificationButton
{
    [SerializeField] private ImageBlinker skillNotiObj;

    private void Awake()
    {
        EventHandler.OnPlayerStatChanged += UpdateButton;
    }

    public override void UpdateButton()
    {
        bool isSkillPointAvailable = PlaySceneMain.Instance.Player.SkillPoint > 0;
        SetNotificationUI(isSkillPointAvailable ? true : false);
        skillNotiObj.Blink(isSkillPointAvailable);
        base.UpdateButton();
    }
}
