using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;
using System;

public class PlaySceneMain : Singleton<PlaySceneMain>
{
    [SerializeField, Header("메인 플레이어")] private Player player;
    [SerializeField, Header("플레이어 초기 데이터")] private PlayerData initializedPlayerData;
    [SerializeField, Header("스킬 초기 데이터")] private SkillData initializedSkillData;
    [SerializeField, Header("최초 맵 데이터")] private TextAsset initializedMapData;
    [SerializeField, Header("도감 초기 데이터")] private IllustratedGuideData initializedIllustratedGuideData;
    [SerializeField, Header("옵션 초기 데이터")] private OptionData initializedOptionData;

    public Player Player { get => player; }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        DataManager.Instance.TableDataTable.TilemapTable.Initialize();
        PathFinder.Instance.Initialize();

        int SaveSlot = DataManager.Instance.SelectedSaveSlot;

        PlayerData playerData = DataManager.LoadData<PlayerData>(DataManager.DefaultPlayerDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);
        if (playerData == null)
        {
            playerData = initializedPlayerData;

            FileManager.CreateFolder(DataManager.DefaultDataFolderName + SaveSlot);
            DataManager.SaveData(playerData, DataManager.DefaultPlayerDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);
        }
        if (playerData.PlayerStatData.CurrentHealth <= 0)
        {
            playerData.CurrentMapCode = 0;
            playerData.PlayerStatData.CurrentHealth = playerData.PlayerStatData.MaxHealth;
            playerData.PlayerStatData.CurrentMana = playerData.PlayerStatData.MaxMana;
            playerData.PositionX = initializedPlayerData.PositionX;
            playerData.PositionY = initializedPlayerData.PositionY;
        }
        Player.Initialize(playerData);

        InventoryData inventoryData = DataManager.LoadData<InventoryData>(DataManager.DefaultInventoryDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);
        if (inventoryData == null)
        {
            inventoryData = new InventoryData(0, new List<object>(new List<Item>(new Item[100])));
            inventoryData.ItemList[0] = (new EquipmentItem(1, 1, 0, null, true));
            inventoryData.ItemList[1] = (new EquipmentItem(2, 1, 0, null, false));
            inventoryData.ItemList[2] = (new EquipmentItem(3, 1, 0, null, false));
            DataManager.SaveData(inventoryData, DataManager.DefaultInventoryDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);
        }
        InventoryManager.Instance.Initialize(inventoryData);

        SkillData skillData = DataManager.LoadData<SkillData>(
            DataManager.DefaultSkillDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);
        if (skillData == null)
        {
            skillData = initializedSkillData;
            DataManager.SaveData(skillData, DataManager.DefaultSkillDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);
        }
        SkillManager.Instance.Initialize(skillData);

        MapData mapData = DataManager.LoadData<MapData>("Map_" + playerData.CurrentMapCode);
        if (mapData == null)
        {
            mapData = DataManager.ParseData<MapData>(initializedMapData.text);
            DataManager.SaveData(mapData, initializedMapData.name);
        }
        MapManager.Instance.ChangeMap(mapData.Code, new Vector3(playerData.PositionX, playerData.PositionY, 0f));

        IllustratedGuideData illustratedGuideData = DataManager.LoadData<IllustratedGuideData>(DataManager.DefaultIllustratedGuideData,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);
        if (illustratedGuideData == null)
        {
            illustratedGuideData = initializedIllustratedGuideData;
            DataManager.SaveData(illustratedGuideData, DataManager.DefaultIllustratedGuideData,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);
        }
        IllustratedGuideManager.Instance.Initialize(illustratedGuideData);

        TutorialProgressData tutorialProgressData = DataManager.LoadData<TutorialProgressData>(DataManager.DefaultTutorialProgressData,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);
        if (tutorialProgressData == null)
        {
            tutorialProgressData = new TutorialProgressData(new List<int>());
            DataManager.SaveData(tutorialProgressData, DataManager.DefaultTutorialProgressData,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);
        }
        TutorialManager.Instance.Initialize(tutorialProgressData);

        OptionData optionData = DataManager.LoadData<OptionData>(DataManager.DefaultOptionData);
        if (optionData == null)
        {
            optionData = initializedOptionData;
            DataManager.SaveData(optionData, DataManager.DefaultOptionData);
        }
        GameManager.Instance.Initialize(optionData);

        UIManager.Instance.MainMenuButtonUI.InitializeNotificationButtons();
        ObjectManager.Instance.Initialize();

        if (playerData.IsFirstStart)
        {
            playerData.IsFirstStart = false;
            EventHandler.OnfirstStart?.Invoke();
        }
    }

    public void SaveGameData(Action<bool> success = null)
    {
        int SaveSlot = DataManager.Instance.SelectedSaveSlot;

        PlayerData playerData = new(
            false,
            player.PlayerName,
            MapManager.Instance.CurrentMap.MapData.Code,
            player.transform.position.x,
            player.transform.position.y,
            player.Level,
            player.SkillPoint,
            player.StatPoint,
            player.CurrentExp,
            new PlayerStatData(
            player.BaseMaxHealth,
            player.CurrentHealth,
            player.BaseMaxMana,
            player.CurrentMana,
            player.BaseAttackPoint,
            player.BaseDefensePoint,
            player.BaseAttackSpeed,
            player.BaseMoveSpeed,
            player.BaseStrength,
            player.BaseDexterity,
            player.BaseConstitution), player.AppearanceData);
        DataManager.SaveData(playerData, DataManager.DefaultPlayerDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);

        InventoryData inventoryData = new InventoryData(InventoryManager.Instance.Money, new List<object>(InventoryManager.Instance.PlayerInventory.ItemContainer.Items));
        DataManager.SaveData(inventoryData, DataManager.DefaultInventoryDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);

        SkillData skillData = new(
            SkillManager.Instance.ManagedSkills,
            SkillManager.Instance.SlotedSkills);
        DataManager.SaveData(skillData, DataManager.DefaultSkillDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);

        IllustratedGuideData guildData = IllustratedGuideManager.Instance.IllustratedGuideData;
        DataManager.SaveData(guildData, DataManager.DefaultIllustratedGuideData,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);

        TutorialProgressData tutorialProgressData = TutorialManager.Instance.TutorialProgressData;
        DataManager.SaveData(tutorialProgressData, DataManager.DefaultTutorialProgressData,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + SaveSlot);

        OptionData optionData = new(
            CameraController.Instance.Camera.orthographicSize,
            SoundController.Instance.BgmAudioSource.volume,
            SoundController.Instance.SfxAudioSource.Peek().volume);
        DataManager.SaveData(optionData, DataManager.DefaultOptionData);

        success?.Invoke(true);
    }

    void OnApplicationQuit()
    {
        SaveGameData();
    }
}
