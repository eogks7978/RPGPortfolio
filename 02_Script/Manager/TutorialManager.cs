using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 튜토리얼 관리자
/// </summary>
public class TutorialManager : MonoBehaviour
{
    #region singleton
    private static TutorialManager instance;
    public static TutorialManager Instance => instance;
    #endregion

    private TutorialData firstStartTutorialData;
    private TutorialData firstOpenStorePopupTutorialData;
    private TutorialData firstOpenEnhancePopupTutorialData;
    private TutorialData firstOpenCraftingPopupTutorialData;

    private TutorialProgressData data;

    public TutorialProgressData TutorialProgressData => data;

    private void Awake()
    {
        //if (instance == null)
        //{
        instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
    }

    public void Initialize(TutorialProgressData data)
    {
        this.data = data;

        firstStartTutorialData =
            new(0, 2, new List<TutorialPageData>() {
                new TutorialPageData(
                $"환영하네 {PlaySceneMain.Instance.Player.PlayerName}. 이 게임에 대해 간략히 설명하겠네.",
                Vector2.zero,
                Vector2.zero,
                null),

                new TutorialPageData(
                "메뉴 버튼을 클릭해 보시게나.",
                UIManager.Instance.MainMenuButtonUI.GetComponent<RectTransform>().sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.MainMenuButtonUI.transform.position),
                new List<Action>{
                    () => UIManager.Instance.MainMenuButtonUI.OnClickButton()
                }),

                new TutorialPageData(
                "이 버튼을 누르면 스킬 창이 나타나지. 눌러보게나.",
                UIManager.Instance.MainMenuButtonUI.SkillBtn.GetComponent<RectTransform>().sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.MainMenuButtonUI.SkillBtn.transform.position),
                new List<Action>{
                    () => UIManager.Instance.OpenSkillPopup()
                }),

                new TutorialPageData(
                "스킬창일세. 무기가 그려져 있는 버튼들 보이나? 이 게임에는 세 종류의 무기가 있고 무기마다 사용할 수 있는 스킬이 다르다네.",
                UIManager.Instance.SkillPopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.SkillPopup.PopupBackgroundRt.transform.position),
                new List<Action>{
                    () => UIManager.Instance.SkillPopup.Close()
                }),

                new TutorialPageData(
                "다음은 상태창일세. 눌러보게나",
                UIManager.Instance.MainMenuButtonUI.StatusBtn.GetComponent<RectTransform>().sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.MainMenuButtonUI.StatusBtn.transform.position),
                new List<Action>{
                    () => UIManager.Instance.OpenStatuspopup()
                }),

                new TutorialPageData(
                "상태창은 자네의 스텟을 확인할 수 있는 곳이네. 레벨이 오를 때마다 일정량의 스탯이 오르고 스탯 포인트도 지급되니 알고 있으면 좋다네.",
                UIManager.Instance.StatusPopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.StatusPopup.PopupBackgroundRt.transform.position),
                new List<Action>{
                    () => UIManager.Instance.StatusPopup.Close()
                }),

                new TutorialPageData(
                "다음은 인벤토리일세. 눌러보게나",
                UIManager.Instance.MainMenuButtonUI.InventoryBtn.GetComponent<RectTransform>().sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.MainMenuButtonUI.InventoryBtn.transform.position),
                new List<Action>{
                    () => UIManager.Instance.OpeninventoryPopup()
                }),

                new TutorialPageData(
                "오른쪽 인벤토리는 자네가 가지고 있는 아이템을 확인할 수 있다네.",
                UIManager.Instance.ItemContainerPopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.ItemContainerPopup.PopupBackgroundRt.transform.position),
                null),

                new TutorialPageData(
                "왼쪽은 현재 착용하고 있는 장비들이 있지.",
                UIManager.Instance.InventoryPopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.InventoryPopup.PopupBackgroundRt.transform.position),
                new List<Action>{
                    () => UIManager.Instance.InventoryPopup.Close()
                }),

                new TutorialPageData(
                "다음은 도감일세.",
                UIManager.Instance.MainMenuButtonUI.IllustratedGuideBtn.GetComponent<RectTransform>().sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.MainMenuButtonUI.IllustratedGuideBtn.transform.position),
                new List<Action>{
                    () => UIManager.Instance.OpenIllustratedGuidePopup()
                }),

                new TutorialPageData(
                "도감에는 이 세계에 출몰하는 몬스터들을 기록할 수 있다네. 몬스터를 처치하면 자동 갱신되지. 사냥을 하며 도감을 완성시켜보게나.",
                UIManager.Instance.IllustratedGuidePopup.PopupBackgroundRt.rect.size,
                UIManager.Instance.GetInversePosition(UIManager.Instance.IllustratedGuidePopup.PopupBackgroundRt.transform.position),
                new List<Action>{
                    () => UIManager.Instance.IllustratedGuidePopup.Close()
                }),

                new TutorialPageData(
                "마지막은 옵션창일세.",
                UIManager.Instance.MainMenuButtonUI.OptionBtn.GetComponent<RectTransform>().sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.MainMenuButtonUI.OptionBtn.transform.position),
                new List<Action>{
                    () => UIManager.Instance.OpenOptionPopup()
                }),

                new TutorialPageData(
                "옵션창은 카메라 거리, 볼륨을 조절할 수 있고 게임 저장 버튼이 있다네.",
                UIManager.Instance.OptionPopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.OptionPopup.PopupBackgroundRt.transform.position),
                new List<Action>{
                    () => UIManager.Instance.OptionPopup.Close()
                }),

                new TutorialPageData(
                "튜토리얼이 끝났네. 지루했을 텐데 얼른 저 해골바가지를 잡으러 가 보게나! 행운을 비네.",
                Vector2.zero,
                Vector2.zero,
                null),
            });
        EventHandler.OnfirstStart += ShowFirstStartTutorial;

        firstOpenStorePopupTutorialData =  
            new(1, 0, new List<TutorialPageData>() {
                new TutorialPageData(
                "어서오게. 상점은 처음인 것 같군. 간단하게 상점을 소개하지.",
                Vector2.zero,
                Vector2.zero,
                null),

                new TutorialPageData(
                "상점엔 포션을 포함한 여러 아이템을 팔고 있지. 돈은 몬스터에게 드롭하는 재료를 상점에 팔아 얻을 수 있다네.",
                UIManager.Instance.StorePopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.StorePopup.PopupBackgroundRt.transform.position),
                null),

                new TutorialPageData(
                "오른쪽의 인벤토리의 아이템들을 눌러 아이템을 사고 팔 수 있다네.",
                UIManager.Instance.ItemContainerPopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.ItemContainerPopup.PopupBackgroundRt.transform.position),
                null),

                new TutorialPageData(
                "설명이 끝났네. 상점을 이용해 아이템을 관리 해 보도록 하지!",
                Vector2.zero,
                Vector2.zero,
                null),
    });
        if (!CheckTutorialIsDone(1)) 
            EventHandler.OnPopupOpened += ShowFirstOpenStorePopupTutorial;

        firstOpenCraftingPopupTutorialData = 
            new(2, 2, new List<TutorialPageData>() {
                new TutorialPageData(
                "그래 자네, 제작소는 처음이구만. 제작소를 소개하지.",
                Vector2.zero,
                Vector2.zero,
                null),

                new TutorialPageData(
                "제작소에서는 여러가지 아이템들을 생산할 수 있다네. 보이는 것 처럼 여러 제작식이 있고 조합엔 각각의 재료가 필요하다네.",
                UIManager.Instance.CraftingPopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.CraftingPopup.PopupBackgroundRt.transform.position),
                null),

                new TutorialPageData(
                "제작식을 하나 확인해보지.",
                UIManager.Instance.CraftingPopup.Cells.First().GetComponent<RectTransform>().sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.CraftingPopup.Cells.First().transform.position),
                new List<Action>{
                    () => UIManager.Instance.CraftingPopup.Cells.First().OnClickButton()
                }),

                new TutorialPageData(
                "여기서 제작식을 확인 한 후 조합을 할 수 있다네.",
                UIManager.Instance.CraftingInformationPopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.CraftingInformationPopup.PopupBackgroundRt.transform.position),
                null),

                new TutorialPageData(
                "필요 아이템이 충분한지 확인 후 제작하시게나.",
                UIManager.Instance.CraftingInformationPopup.MaterialCelllayoutGroup.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.CraftingInformationPopup.MaterialCelllayoutGroup.transform.position),
                new List<Action>{
                    () => UIManager.Instance.CraftingInformationPopup.Close()
                }),

                new TutorialPageData(
                "설명이 끝났네. 조합소를 이용해 아이템을 만들어 더욱 성장해 보시게나!",
                Vector2.zero,
                Vector2.zero,
                null),
});
        if (!CheckTutorialIsDone(2))
            EventHandler.OnPopupOpened += ShowFirstOpenCraftingPopupTutorial;

        firstOpenEnhancePopupTutorialData = 
            new(3, 1, new List<TutorialPageData>() {
                new TutorialPageData(
                "대장간은 처음인가보군? 간단하게 소개하지.",
                Vector2.zero,
                Vector2.zero,
                null),

                new TutorialPageData(
                "대장간에서는 장비를 강화할 수 있다네. 장비 강화에는 강화석이나 다른 재료들이 필요하고 성공 확률이 존재하지. 성공하면 장비 성능이 크게 향상된다네.",
                UIManager.Instance.EnhancePopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.EnhancePopup.PopupBackgroundRt.transform.position),
                null),

                new TutorialPageData(
                "오른쪽의 인벤토리의 아이템들을 눌러 장비를 등록할 수 있다네.",
                UIManager.Instance.ItemContainerPopup.PopupBackgroundRt.sizeDelta,
                UIManager.Instance.GetInversePosition(UIManager.Instance.ItemContainerPopup.PopupBackgroundRt.transform.position),
                null),

                new TutorialPageData(
                "설명이 끝났네. 대장간을 이용해 더욱 강력한 장비를 얻어보시게나.",
                Vector2.zero,
                Vector2.zero,
                null),
});
        if (!CheckTutorialIsDone(3))
            EventHandler.OnPopupOpened += ShowFirstOpenEnhancePopupTutorial;

    }

    public void SaveTutorialData(int code)
    {
        if (!CheckTutorialIsDone(code))
        {
            data.TutorialInProgress.Add(code);
        }
    }

    public bool CheckTutorialIsDone(int code)
    {
        return data.TutorialInProgress.Contains(code);
    }

    public void ShowFirstStartTutorial()
    {
        if (!CheckTutorialIsDone(firstStartTutorialData.TutorialCode))
        {
            EventHandler.OnfirstStart -= ShowFirstStartTutorial;
            UIManager.Instance.OpenTutorialPopup(firstStartTutorialData);
        }
    }
    public void ShowFirstOpenStorePopupTutorial(Type type)
    {
        if (type == typeof(UIStorePopup))
        {
            EventHandler.OnPopupOpened -= ShowFirstOpenStorePopupTutorial;
            UIManager.Instance.OpenTutorialPopup(firstOpenStorePopupTutorialData);
        }
    }
    public void ShowFirstOpenCraftingPopupTutorial(Type type)
    {
        if (type == typeof(UICraftingPopup))
        {
            EventHandler.OnPopupOpened -= ShowFirstOpenCraftingPopupTutorial;
            UIManager.Instance.OpenTutorialPopup(firstOpenCraftingPopupTutorialData);
        }
    }
    public void ShowFirstOpenEnhancePopupTutorial(Type type)
    {
        if (type == typeof(UIEnhancePopup))
        {
            EventHandler.OnPopupOpened -= ShowFirstOpenEnhancePopupTutorial;
            UIManager.Instance.OpenTutorialPopup(firstOpenEnhancePopupTutorialData);
        }
    }
}
