using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LobbySceneMain : Singleton<LobbySceneMain>
{
    // ---- UI ----
    [SerializeField] private LobbySceneDataInfoPopup _lobbySceneDataInfoPopup;
    [SerializeField] private UICreateCharacterPannel _createCharacterPannel;
    [SerializeField] private UINotificationPopup _notificationPopup;
    // ------------

    public UICreateCharacterPannel CreateCharacterPannel => _createCharacterPannel;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OpenNotificationPopup("종료하시겠습니까?", true, "알림", "예", "아니오",
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

    public void OpenDataInfoPopup(
        int dataCode,
        UnityAction<object> cbClose = null)
    {
        if (_lobbySceneDataInfoPopup.isActiveAndEnabled)
        {
            _lobbySceneDataInfoPopup.UpdatePopup(dataCode);
            return;
        }

        _lobbySceneDataInfoPopup.Open(dataCode, (resultObj) =>
        {
            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenCreateCharacterPannel(UnityAction<object> cbClose = null)
    {
        if (_createCharacterPannel.isActiveAndEnabled)
            return;

        _createCharacterPannel.Open((resultObj) =>
        {
            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenNotificationPopup(
        string notification,
        bool noButtonOn = false,
        string titleText = "",
        string okButtonText = "",
        string noButtonText = "",
        UnityAction<object> cbClose = null)
    {
        if (_notificationPopup.isActiveAndEnabled)
            return;

        _notificationPopup.Open(
             notification,
             noButtonOn,
             titleText = "",
             okButtonText = "",
             noButtonText = "",
             (resultObj) =>
             {
                 cbClose?.Invoke(resultObj);
             });
    }
}
