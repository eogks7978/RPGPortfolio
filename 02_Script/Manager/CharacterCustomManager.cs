using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터 커스터마이징 클래스
/// </summary>
public class CharacterCustomManager : Singleton<CharacterCustomManager>
{
    [SerializeField] private PlayerData initializedPlayerData;

    private List<Color> colors = new List<Color>() { 
        new Color(255, 223, 100, 255),
        new Color(204, 153, 51, 255),
        new Color(192, 192, 192, 255),
        new Color(229, 228, 226, 255),
        new Color(183, 110, 121, 255),
        new Color(255, 215, 125, 255),
        new Color(255, 255, 240, 255),
        new Color(102, 51, 25, 255),
        new Color(139, 69, 19, 255),
        new Color(123, 63, 43, 255),
        new Color(153, 101, 21, 255),
        new Color(139, 0, 0, 255),
        new Color(195, 176, 145, 255),
        new Color(0, 0, 139, 255),
        new Color(0, 128, 128, 255),
        new Color(255, 182, 193, 255),
        new Color(230, 230, 250, 255),
        new Color(208, 207, 170, 255),
        new Color(152, 255, 152, 255),
        new Color(200, 162, 200, 255),
    };

    public List<Color> Colors => colors; 

    public static Action OnCharacterCreated;

    public void CreatePlayerData(string nickName, PlayerAppearanceData data)
    {
        bool isCreateSuccess = FileManager.CreateFolder(DataManager.DefaultDataFolderName + DataManager.Instance.SelectedSaveSlot);

        if (!isCreateSuccess) 
            return;

        PlayerData newPlayerData = new PlayerData(
            initializedPlayerData.IsFirstStart,
            nickName,
            initializedPlayerData.CurrentMapCode,
            initializedPlayerData.PositionX,
            initializedPlayerData.PositionY,
            initializedPlayerData.Level,
            initializedPlayerData.SkillPoint,
            initializedPlayerData.StatPoint,
            initializedPlayerData.CurrentExp,
            initializedPlayerData.PlayerStatData, data);
        DataManager.SaveData(
            newPlayerData, 
            DataManager.DefaultPlayerDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + DataManager.Instance.SelectedSaveSlot);
        OnCharacterCreated?.Invoke();
    }
}
