using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 메인 메뉴 리스트를 보기 위한 버튼
/// </summary>
public class UIMainMenuButton : UInotificationButton
{
    [SerializeField] private Button thisButton;
    [SerializeField] private Image buttonImage;
    [SerializeField] private GameObject buttonsPannel;

    [SerializeField, Header("메뉴 리스트가 열렸을 때 닫기 이미지")] private Sprite closeImage;  
    [SerializeField, Header("메뉴 리스트가 닫혀있을 때 열기 이미지")] private Sprite openImage;  

    [SerializeField] private Button optionBtn;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button statusBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Button illustratedGuideBtn;

    [SerializeField, Header("링크되어있는 팝업에 확인할 일이 생겼을 때 표시 가능한 버튼들(skill, inventory 등등)")]
    private List<UInotificationButton> uInotificationButtons;

    public Button OptionBtn => optionBtn;
    public Button InventoryBtn => inventoryBtn;
    public Button StatusBtn => statusBtn;
    public Button SkillBtn => skillBtn;
    public Button IllustratedGuideBtn => illustratedGuideBtn;

    private void OnEnable()
    {
        EventHandler.OnMainUIButtonUpdated += UpdateButton;
    }

    private void OnDisable()
    {
        EventHandler.OnMainUIButtonUpdated -= UpdateButton;
    }

    private void Awake()
    {
        if (!thisButton)
            thisButton = GetComponent<Button>();

        Initialize();
    }

    private void Initialize()
    {
        thisButton.onClick.AddListener(OnClickButton);

        optionBtn.onClick.AddListener(() => UIManager.Instance.OpenOptionPopup());
        inventoryBtn.onClick.AddListener(() => UIManager.Instance.OpeninventoryPopup());
        statusBtn.onClick.AddListener(() => UIManager.Instance.OpenStatuspopup());
        skillBtn.onClick.AddListener(() => UIManager.Instance.OpenSkillPopup());
        illustratedGuideBtn.onClick.AddListener(() => UIManager.Instance.OpenIllustratedGuidePopup());
    }

    /// <summary>
    /// 링크된 팝업에 확인 할 일이 생길 수 있으므로 업데이트
    /// </summary>
    public void InitializeNotificationButtons()
    {
        foreach (var button in uInotificationButtons)
        {
            button.UpdateButton();
        }
    }

    public void OnClickButton()
    {
        buttonsPannel.SetActive(buttonsPannel.activeSelf ? false : true);
        buttonImage.sprite = buttonsPannel.activeSelf ? closeImage : openImage;
    }

    public override void UpdateButton()
    {
        foreach (var button in uInotificationButtons)
        {
            if (button.IsOn)
            {
                SetNotificationUI(true);
                return;
            }
        }

        SetNotificationUI(false);
    }
}
