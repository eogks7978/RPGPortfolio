using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 캐릭터 정보 요약 팝업, 접속 버튼
/// </summary>
public class LobbySceneDataInfoPopup : UIPanel
{
    [SerializeField] private TMPro.TextMeshProUGUI playerStatText01;
    [SerializeField] private TMPro.TextMeshProUGUI playerStatText02;
    [SerializeField] private TMPro.TextMeshProUGUI playerStatText03;
    [SerializeField] private TMPro.TextMeshProUGUI playerStatText04;

    [SerializeField] private Button startButton;
    [SerializeField] private Button resetDataButton;

    protected override void Awake()
    {
        base.Awake();
        startButton.onClick.AddListener(StartGame);
        resetDataButton.onClick.AddListener(ResetData);
    }

    public void Open(int dataCode, UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);
        UpdatePopup(dataCode);
    }

    public void UpdatePopup(int dataCode)
    {
        PlayerData data = DataManager.LoadData<PlayerData>(DataManager.DefaultPlayerDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + dataCode);

        if (data != null)
        {
            string mapFileName = DataManager.Instance.TableDataTable.MapTables
                .FindTableCellWithCode(data.CurrentMapCode).Data.name;
            MapData mapData = DataManager.LoadData<MapData>(mapFileName);

            string mapName = mapData != null ? mapData.MapName : "";

            string greenHex = ColorUtility.ToHtmlStringRGB(Color.green);
            playerStatText01.text = $"닉네임: <color=#{greenHex}>{data.PlayerName}</color>";
            playerStatText02.text = $"레벨: <color=#{greenHex}>{data.Level.ToString()}</color>";
            playerStatText03.text = $"경험치: <color=#{greenHex}>{data.CurrentExp.ToString("F1")}</color>";
            playerStatText04.text = $"마지막 플레이 맵: <color=#{greenHex}>{mapName}</color>";
        }
        else
            Close();
    }

    private void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    private void ResetData()
    {
        LobbySceneMain.Instance.OpenNotificationPopup("현재 슬롯의 데이터를 초기화 하시겠습니까?", true, "알림", "예", "아니오",
            (resultObj) =>
            {
                var result = resultObj as UINotificationPopup.Results;
                if (result.isConfirm)
                {
                    FileManager.DeleteFilesInFolder(DataManager.DefaultDataFolderName + DataManager.Instance.SelectedSaveSlot);
                    Close();
                }
            });

    }
}
