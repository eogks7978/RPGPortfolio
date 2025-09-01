using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 월드 접속 전 캐릭터 데이터 선택 팝업
/// </summary>
public class SelectDataButton : MonoBehaviour
{
    [SerializeField] private Button thisButton;
    [SerializeField] private TMPro.TextMeshProUGUI textMeshPro;
    [SerializeField] private int playerDataCode;
    [SerializeField, Header("데이터가 없음을 표시하는 string")] private string noDataText;
    [SerializeField] private Color textColor;
    private bool isDataEmpty;

    private void Awake()
    {
        if (!thisButton)
            thisButton = GetComponent<Button>();

        thisButton.onClick.AddListener(OnClickButton);

        UpdatePopup();
    }

    private void OnEnable()
    {
        CharacterCustomManager.OnCharacterCreated += UpdatePopup;
        FileManager.OnDataDeleted += UpdatePopup;
    }

    private void OnDisable()
    {
        CharacterCustomManager.OnCharacterCreated -= UpdatePopup;
        FileManager.OnDataDeleted -= UpdatePopup;
    }

    private void UpdatePopup()
    {
        PlayerData data = DataManager.LoadData<PlayerData>(DataManager.DefaultPlayerDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + playerDataCode);
        isDataEmpty = data == null;

        textMeshPro.text =
        !isDataEmpty
            ? $"<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{data.PlayerName}</color>"
            : $"<color=#{ColorUtility.ToHtmlStringRGB(Color.white)}>{noDataText}</color>";
    }

    private void OnClickButton()
    {
        DataManager.Instance.SelectedSaveSlot = playerDataCode;

        LobbySceneMain.Instance.OpenDataInfoPopup(playerDataCode);

        if (isDataEmpty)
            LobbySceneMain.Instance.OpenCreateCharacterPannel();
    }
}
