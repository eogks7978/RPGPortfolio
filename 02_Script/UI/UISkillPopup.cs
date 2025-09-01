using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 스킬 팝업
/// </summary>
public class UISkillPopup : UIPanel
{
    [SerializeField, Header("검 스킬들이 있는 페이지")] private GameObject swordSkill;
    [SerializeField, Header("활 스킬들이 있는 페이지")] private GameObject bowSkill;
    [SerializeField, Header("지팡이 스킬들이 있는 페이지")] private GameObject wandSkill;
    [SerializeField] private Button swordSkillBtn;
    [SerializeField] private Button BowSkillBtn;
    [SerializeField] private Button WandSkillBtn;
    [SerializeField] private Transform swordSkillContentTr;
    [SerializeField] private Transform bowSkillContentTr;
    [SerializeField] private Transform wandSkillContentTr;
    [SerializeField, Header("남은 스킬 포인트 표시")] private TMPro.TextMeshProUGUI leftSkillPointText;
    [SerializeField] private RectTransform popupBackgroundRt;

    public RectTransform PopupBackgroundRt => popupBackgroundRt;

    protected override void Awake()
    {
        base.Awake();

        swordSkillBtn.onClick.AddListener(() => OnClickOpenScrollView(swordSkill));
        BowSkillBtn.onClick.AddListener(() => OnClickOpenScrollView(bowSkill));
        WandSkillBtn.onClick.AddListener(() => OnClickOpenScrollView(wandSkill));
    }

    private void OnEnable()
    {
        EventHandler.OnSkillLevelUp += UpdateSkillPointText;
    }

    private void OnDisable()
    {
        EventHandler.OnSkillLevelUp -= UpdateSkillPointText;
    }

    public override void Open(UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);

        UpdateSkillPointText();
        DisableAllScrollView();
        OnClickOpenScrollView(swordSkill);
        ResetContent();
    }

    /// <summary>
    /// ScrollView 스크롤 초기화
    /// </summary>
    private void ResetContent()
    {
        swordSkillContentTr.localPosition = Vector3.zero;
        bowSkillContentTr.localPosition = Vector3.zero;
        wandSkillContentTr.localPosition = Vector3.zero;
    }

    private void OnClickOpenScrollView(GameObject scollViewTr)
    {
        DisableAllScrollView();

        scollViewTr.SetActive(true);
    }

    private void DisableAllScrollView()
    {
        if (swordSkill.activeSelf)
            swordSkill.SetActive(false);
        if (bowSkill.activeSelf)
            bowSkill.SetActive(false);
        if (wandSkill.activeSelf)
            wandSkill.SetActive(false);
    }

    private void UpdateSkillPointText()
    {
        leftSkillPointText.text = PlaySceneMain.Instance.Player.SkillPoint.ToString();
    }
}
