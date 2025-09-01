using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : BaseUIManager
{
    #region singleton
    static private UIManager _instance;
    static public UIManager Instance { get { return _instance; } }
    #endregion

    [SerializeField] private UIHealthBar _playerHealthBar;
    [SerializeField] private UIManaBar _playerManaBar;
    [SerializeField] private UIExpBar _playerExpBar;
    [SerializeField] private UILevelBar _playerLevelBar;
    [SerializeField] private UISkillActionBar _skillActionBar;
    [SerializeField] private UIHotbar _itemHotbar;
    [SerializeField] private UIDropItemPannel _dropItemUI;
    [SerializeField] private UIMainMenuButton _mainMenuButtonUI;
    // --------------------- Always on ---------------------
    [SerializeField] private UIOptionPopup _optionPopup;
    [SerializeField] private UIInventoryPopup _inventoryPopup;
    [SerializeField] private UIStatusPopup _statusPopup;
    [SerializeField] private UISkillPopup _skillPopup;
    [SerializeField] private UILoadingPannel _loadingPannel;
    [SerializeField] private UIPlayerDeadPopup _playerDeadPopup;
    [SerializeField] private UISkillInfoPopup _skillInfoPopup;
    [SerializeField] private UISkillSetPopup _skillSetPopup;
    [SerializeField] private UINotificationPopup _notificationPopup;
    [SerializeField] private UISimpleNotification _uISimpleNotification;
    [SerializeField] private UIItemDescriptionPopup _itemDescriptionPopup;
    [SerializeField] private UIInputFieldConfirmPopup _inputFieldConfirmPopup;
    [SerializeField] private UIItemSetPopup _itemSetPopup;
    [SerializeField] private UIIllustratedGuidePopup _illustratedGuidePopup;
    [SerializeField] private UIDialoguePopup _dialoguePopup;
    [SerializeField] private UIStorePopup _storePopup;
    [SerializeField] private UIItemInformationPopup _itemInformationPopup;
    [SerializeField] private UIMapNamePopup _mapNamePopup;
    [SerializeField] private UICraftingPopup _craftingPopup;
    [SerializeField] private UICraftingInformationPopup _craftingInformationPopup;
    [SerializeField] private UISimpleItemDescription _simpleItemDescription;
    [SerializeField] private UIItemContainer _itemContainerPopup;
    [SerializeField] private UIEnhancePopup _enhancePopup;
    [SerializeField] private UIEnhanceAnimationPopup _enhanceAnimationPopup;
    [SerializeField] private UITutorialPopup _tutorialPopup;

    [SerializeField] private Canvas _mainCanvas;

    // Dev
    [SerializeField] private GameObject _devButton;
    [SerializeField] private GameObject _devPannel;

    #region Property
    public UIOptionPopup OptionPopup => _optionPopup;
    public UIHealthBar PlayerHealthBar => _playerHealthBar;
    public UIManaBar PlayerManaBar => _playerManaBar;
    public UIExpBar PlayerExpBar => _playerExpBar;
    public UILevelBar PlayerLevelBar => _playerLevelBar;
    public UISkillActionBar SkillActionBar => _skillActionBar;
    public UIHotbar ItemHotbar => _itemHotbar;
    public UIInventoryPopup InventoryPopup => _inventoryPopup;
    public UIDialoguePopup DialoguePopup => _dialoguePopup;
    public UIStatusPopup StatusPopup => _statusPopup;
    public UIItemContainer ItemContainerPopup => _itemContainerPopup;
    public UISkillPopup SkillPopup => _skillPopup;
    public UITutorialPopup TutorialPopup => _tutorialPopup;
    public UIIllustratedGuidePopup IllustratedGuidePopup => _illustratedGuidePopup;
    public UIDropItemPannel DropItemUI => _dropItemUI;
    public UIStorePopup StorePopup => _storePopup;
    public UICraftingPopup CraftingPopup => _craftingPopup;
    public UICraftingInformationPopup CraftingInformationPopup => _craftingInformationPopup;
    public UIEnhancePopup EnhancePopup => _enhancePopup;
    public UIMainMenuButton MainMenuButtonUI => _mainMenuButtonUI;
    public Canvas MainCanvas => _mainCanvas;
    #endregion

    protected void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _devButton.SetActive(GameManager.Instance.DevelopmentBuild);
    }

    public Vector2 GetInversePosition(Vector2 pos)
    {
        return _mainCanvas.transform.InverseTransformPoint(pos);
    }

    public void OpenOptionPopup(UnityAction<object> cbClose = null)
    {
        if (_optionPopup.isActiveAndEnabled)
            return;

        AddPanel(_optionPopup);
        EventHandler.OnPopupOpened?.Invoke(_optionPopup.GetType());

        _optionPopup.Open((resultObj) =>
        {
            RemovePanel(_optionPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpeninventoryPopup(UnityAction<object> cbClose = null)
    {
        if (_inventoryPopup.isActiveAndEnabled)
            return;

        AddPanel(_inventoryPopup);
        EventHandler.OnPopupOpened?.Invoke(_inventoryPopup.GetType());

        _inventoryPopup.Open((resultObj) =>
        {
            RemovePanel(_inventoryPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenStatuspopup(UnityAction<object> cbClose = null)
    {
        if (_statusPopup.isActiveAndEnabled)
            return;

        AddPanel(_statusPopup);
        EventHandler.OnPopupOpened?.Invoke(_statusPopup.GetType());

        _statusPopup.Open((resultObj) =>
        {
            RemovePanel(_statusPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenSkillPopup(UnityAction<object> cbClose = null)
    {
        if (_skillPopup.isActiveAndEnabled)
            return;

        AddPanel(_skillPopup);
        EventHandler.OnPopupOpened?.Invoke(_skillPopup.GetType());

        _skillPopup.Open((resultObj) =>
        {
            RemovePanel(_skillPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenLoadingPannel()
    {
        _loadingPannel.ShowLoadingPannel();
    }

    public void OpenPlayerDeadPopup(float expDecrease, UnityAction<object> cbClose = null)
    {
        if (_playerDeadPopup.isActiveAndEnabled)
            return;

        AddPanel(_playerDeadPopup);
        EventHandler.OnPopupOpened?.Invoke(_playerDeadPopup.GetType());

        _playerDeadPopup.Open(expDecrease, (resultObj) =>
        {
            RemovePanel(_playerDeadPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenSkillInfoPopup(int skillCode, UnityAction<object> cbClose = null)
    {
        if (_skillInfoPopup.isActiveAndEnabled)
            return;

        AddPanel(_skillInfoPopup);
        EventHandler.OnPopupOpened?.Invoke(_skillInfoPopup.GetType());

        _skillInfoPopup.Open(skillCode, (resultObj) =>
        {
            RemovePanel(_skillInfoPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenSkillSetPopup(int skillCode, UnityAction<object> cbClose = null)
    {
        if (_skillSetPopup.isActiveAndEnabled)
            return;

        AddPanel(_skillSetPopup);
        EventHandler.OnPopupOpened?.Invoke(_skillSetPopup.GetType());

        _skillSetPopup.Open(skillCode, (resultObj) =>
        {
            RemovePanel(_skillSetPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    // var result = resultObj as UINotificationPopup.Results; 형태로 콜백 사용
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

        AddPanel(_notificationPopup);
        EventHandler.OnPopupOpened?.Invoke(_notificationPopup.GetType());

        _notificationPopup.Open(
             notification,
             noButtonOn,
             titleText = "",
             okButtonText = "",
             noButtonText = "",
             (resultObj) =>
             {
                 RemovePanel(_notificationPopup);
                 cbClose?.Invoke(resultObj);
             });
    }

    public void ShowSimpleNotification(string notification, Color color)
    {
        _uISimpleNotification.Show(notification, color);
    }

    public void OpenItemDescriptionPopup(Item itemSlot, UnityAction<object> cbClose = null)
    {
        if (_itemDescriptionPopup.isActiveAndEnabled)
            return;

        AddPanel(_itemDescriptionPopup);
        EventHandler.OnPopupOpened?.Invoke(_itemDescriptionPopup.GetType());

        _itemDescriptionPopup.Open(itemSlot, (resultObj) =>
        {
            RemovePanel(_itemDescriptionPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenInputfieldConfirmPopup(string titleText, int minValue, int maxValue, UnityAction<object> cbClose = null)
    {
        if (_inputFieldConfirmPopup.isActiveAndEnabled)
            return;

        AddPanel(_inputFieldConfirmPopup);
        EventHandler.OnPopupOpened?.Invoke(_inputFieldConfirmPopup.GetType());

        _inputFieldConfirmPopup.Open(titleText , minValue, maxValue, (resultObj) =>
        {
            RemovePanel(_inputFieldConfirmPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenItemSetPopup(Item itemSlot, UnityAction<object> cbClose = null)
    {
        if (_itemSetPopup.isActiveAndEnabled)
            return;

        AddPanel(_itemSetPopup);
        EventHandler.OnPopupOpened?.Invoke(_itemSetPopup.GetType());

        _itemSetPopup.Open(itemSlot, (resultObj) =>
        {
            RemovePanel(_itemSetPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenIllustratedGuidePopup(UnityAction<object> cbClose = null)
    {
        if (_illustratedGuidePopup.isActiveAndEnabled)
            return;

        AddPanel(_illustratedGuidePopup);
        EventHandler.OnPopupOpened?.Invoke(_illustratedGuidePopup.GetType());

        _illustratedGuidePopup.Open((resultObj) =>
        {
            RemovePanel(_illustratedGuidePopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenDialoguePopup(NPC interlocutor, string name, List<DialogueUIData> dialogueData, AudioClip openSound = null, UnityAction<object> cbClose = null)
    {
        if (_dialoguePopup.isActiveAndEnabled)
            return;

        AddPanel(_dialoguePopup);
        EventHandler.OnPopupOpened?.Invoke(_dialoguePopup.GetType());

        _dialoguePopup.Open(interlocutor, name, dialogueData, openSound, (resultObj) =>
        {
            RemovePanel(_dialoguePopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenStorePopup(List<Item> itemList, UnityAction<object> cbClose = null)
    {
        if (_storePopup.isActiveAndEnabled)
            return;

        AddPanel(_storePopup);
        EventHandler.OnPopupOpened?.Invoke(_storePopup.GetType());

        _storePopup.Open(itemList, (resultObj) =>
        {
            RemovePanel(_storePopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenItemInformationPopup(Item item, UnityAction<object> cbClose = null)
    {
        if (_itemInformationPopup.isActiveAndEnabled)
            return;

        AddPanel(_itemInformationPopup);
        EventHandler.OnPopupOpened?.Invoke(_itemInformationPopup.GetType());

        _itemInformationPopup.Open(item, (resultObj) =>
        {
            RemovePanel(_itemInformationPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenMapNamePopup(string mapName, UnityAction<object> cbClose = null)
    {
        if (_mapNamePopup.isActiveAndEnabled)
            return;

        AddPanel(_mapNamePopup);
        EventHandler.OnPopupOpened?.Invoke(_mapNamePopup.GetType());

        _mapNamePopup.Open(mapName, (resultObj) =>
        {
            RemovePanel(_mapNamePopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenCraftingPopup(UnityAction<object> cbClose = null)
    {
        if (_craftingPopup.isActiveAndEnabled)
            return;

        AddPanel(_craftingPopup);
        EventHandler.OnPopupOpened?.Invoke(_craftingPopup.GetType());

        _craftingPopup.Open((resultObj) =>
        {
            RemovePanel(_craftingPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenCraftingInformationPopup(CraftingTableCell tbCell, UnityAction<object> cbClose = null)
    {
        if (_craftingInformationPopup.isActiveAndEnabled)
            return;

        AddPanel(_craftingInformationPopup);
        EventHandler.OnPopupOpened?.Invoke(_craftingInformationPopup.GetType());

        _craftingInformationPopup.Open(tbCell, (resultObj) =>
        {
            RemovePanel(_craftingInformationPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void ShowSimpleItemDescription(string itemName, Vector2 pos)
    {
        _simpleItemDescription.Show(itemName, pos);
    }

    public void OpenItemContainer()
    {
        _itemContainerPopup.Open();
        EventHandler.OnPopupOpened?.Invoke(_itemContainerPopup.GetType());
    }
    public void CloseItemContainer()
    {
        _itemContainerPopup.Close();
    }

    public void OpenEnchantPopup(UnityAction<object> cbClose = null)
    {
        if (_enhancePopup.isActiveAndEnabled)
            return;

        AddPanel(_enhancePopup);
        EventHandler.OnPopupOpened?.Invoke(_enhancePopup.GetType());

        _enhancePopup.Open((resultObj) =>
        {
            RemovePanel(_enhancePopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenEnhanceAnimationPopup(UnityAction<object> cbClose = null)
    {
        if (_enhanceAnimationPopup.isActiveAndEnabled)
            return;

        AddPanel(_enhanceAnimationPopup);
        EventHandler.OnPopupOpened?.Invoke(_enhanceAnimationPopup.GetType());

        _enhanceAnimationPopup.Open((resultObj) =>
        {
            RemovePanel(_enhanceAnimationPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenTutorialPopup(TutorialData data,UnityAction<object> cbClose = null)
    {
        if (_tutorialPopup.isActiveAndEnabled)
            return;

        AddPanel(_tutorialPopup);
        EventHandler.OnPopupOpened?.Invoke(_tutorialPopup.GetType());

        _tutorialPopup.Open(data, (resultObj) =>
        {
            RemovePanel(_tutorialPopup);

            cbClose?.Invoke(resultObj);
        });
    }

    public void OpenDevPannel()
    {
        _devPannel.gameObject.SetActive(!_devPannel.activeSelf);
    }
}

