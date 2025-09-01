using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 스테이터스 팝업
/// </summary>
public class UIStatusPopup : UIPanel
{
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI skillPointText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI mpText;
    [SerializeField] private TextMeshProUGUI attackPointText;
    [SerializeField] private TextMeshProUGUI defensePointText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;

    [SerializeField] private TextMeshProUGUI strStatText;
    [SerializeField] private TextMeshProUGUI dexStatText;
    [SerializeField] private TextMeshProUGUI conStatText;

    [SerializeField] private Button strButton;
    [SerializeField] private Button dexButton;
    [SerializeField] private Button conButton;

    [SerializeField] private Button statSetButton;
    [SerializeField] private Button statReturnButton;

    [SerializeField] private TextMeshProUGUI currentStatPointText;

    [SerializeField] private GameObject currentStatPoint;

    [SerializeField] private bool isStatAdding;
    [SerializeField] private int tempLeftStatPoint;
    [SerializeField] private int tempAddedStr;
    [SerializeField] private int tempAddedDex;
    [SerializeField] private int tempAddedCon;

    [SerializeField] private RectTransform popupBackgroundRt;

    public RectTransform PopupBackgroundRt => popupBackgroundRt;

    protected override void Awake()
    {
        base.Awake();

        strButton.onClick.AddListener(OnClickAddStr);
        dexButton.onClick.AddListener(OnClickAddDex);
        conButton.onClick.AddListener(OnClickAddCon);
        statSetButton.onClick.AddListener(OnClickSetButton);
        statReturnButton.onClick.AddListener(ResetAddedStat);
    }

    public override void Open(UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);
        UpdatePopup();
    }

    private void OnEnable()
    {
        // 플레이어 스탯 변화 이벤트에 걸어서 팝업 오픈 도중 스탯 변화에 대비
        EventHandler.OnPlayerStatChanged += UpdatePopup;
    }

    private void OnDisable()
    {
        EventHandler.OnPlayerStatChanged -= UpdatePopup;
    }


    private void UpdatePopup()
    {
        currentLevelText.text = PlaySceneMain.Instance.Player.Level.ToString();
        skillPointText.text = PlaySceneMain.Instance.Player.SkillPoint.ToString();
        expText.text = PlaySceneMain.Instance.Player.CurrentExp.ToString("F1")
            + " / " + PlaySceneMain.Instance.Player.MaxExp.ToString("F0");

        UpdateStat();

        tempLeftStatPoint = PlaySceneMain.Instance.Player.StatPoint;

        ResetAddedStat();
        UpdateOriginStat();
    }

    private void UpdateStat()
    {
        hpText.text = PlaySceneMain.Instance.Player.CurrentHealth.ToString("F1")
            + " / " + PlaySceneMain.Instance.Player.MaxHealth.ToString("F0");
        hpText.color = Color.white;
        mpText.text = PlaySceneMain.Instance.Player.CurrentMana.ToString("F1")
            + " / " + PlaySceneMain.Instance.Player.MaxMana.ToString("F0");
        mpText.color = Color.white;
        attackPointText.text = PlaySceneMain.Instance.Player.AttackPoint.ToString("F0");
        attackPointText.color = Color.white;
        defensePointText.text = PlaySceneMain.Instance.Player.DefensePoint.ToString("F1");
        defensePointText.color = Color.white;
        attackSpeedText.text = PlaySceneMain.Instance.Player.AttackSpeed.ToString("F2");
        attackSpeedText.color = Color.white;
        moveSpeedText.text = PlaySceneMain.Instance.Player.MoveSpeed.ToString("F1");
        moveSpeedText.color = Color.white;
    }

    private void UpdateOriginStat()
    {
        bool statPointLeft = tempLeftStatPoint == 0;

        strButton.gameObject.SetActive(statPointLeft ? false : true);
        dexButton.gameObject.SetActive(statPointLeft ? false : true);
        conButton.gameObject.SetActive(statPointLeft ? false : true);

        currentStatPoint.SetActive(statPointLeft ? false : true);
        currentStatPointText.text = tempLeftStatPoint.ToString();

        statSetButton.gameObject.SetActive(isStatAdding);
        statReturnButton.gameObject.SetActive(isStatAdding);
    }

    /// <summary>
    /// 스탯을 임시로 올렸 을 때의 적용된 능력치 표시
    /// </summary>
    /// <param name="statName"></param>
    private void ShowStatDiffrenceByAddStat(string statName)
    {
        switch(statName)
        {
            case nameof(PlaySceneMain.Instance.Player.Strength):
                attackPointText.text = (PlaySceneMain.Instance.Player.AttackPoint + tempAddedStr).ToString("F0");
                attackPointText.color = Color.blue;
                break;
            case nameof(PlaySceneMain.Instance.Player.Dexterity):
                attackSpeedText.text = (PlaySceneMain.Instance.Player.AttackSpeed + tempAddedDex * 0.01f).ToString("F2");
                attackSpeedText.color = Color.blue;
                break;
            case nameof(PlaySceneMain.Instance.Player.Constitution):
                hpText.text = (PlaySceneMain.Instance.Player.CurrentHealth + tempAddedCon * 10f).ToString()
                    + " / " + (PlaySceneMain.Instance.Player.MaxHealth + tempAddedCon * 10f).ToString();
                hpText.color = Color.blue;
                defensePointText.text = (PlaySceneMain.Instance.Player.DefensePoint + tempAddedCon * 0.4f).ToString("F1");
                defensePointText.color = Color.blue;
                break;
        }
    }

    private void OnClickAddStr()
    {
        if (!isStatAdding)
            isStatAdding = true;

        tempLeftStatPoint -= 1;
        tempAddedStr += 1;
        strStatText.text = (PlaySceneMain.Instance.Player.Strength + tempAddedStr).ToString();
        strStatText.color = Color.blue;
        ShowStatDiffrenceByAddStat(nameof(PlaySceneMain.Instance.Player.Strength));
        UpdateOriginStat();
    }

    private void OnClickAddDex()
    {
        if (!isStatAdding)
            isStatAdding = true;

        tempLeftStatPoint -= 1;
        tempAddedDex += 1;
        dexStatText.text = (PlaySceneMain.Instance.Player.Dexterity + tempAddedDex).ToString();
        dexStatText.color = Color.blue;
        ShowStatDiffrenceByAddStat(nameof(PlaySceneMain.Instance.Player.Dexterity));
        UpdateOriginStat();
    }

    private void OnClickAddCon()
    {
        if (!isStatAdding)
            isStatAdding = true;

        tempLeftStatPoint -= 1;
        tempAddedCon += 1;
        conStatText.text = (PlaySceneMain.Instance.Player.Constitution + tempAddedCon).ToString();
        conStatText.color = Color.blue;
        ShowStatDiffrenceByAddStat(nameof(PlaySceneMain.Instance.Player.Constitution));
        UpdateOriginStat();
    }

    /// <summary>
    /// 임시로 올린 스탯 적용
    /// </summary>
    private void OnClickSetButton()
    {
        PlaySceneMain.Instance.Player.SetStatus(new Dictionary<string, float> {
            { nameof(PlaySceneMain.Instance.Player.StatPoint), -AddedPoint() },
            { nameof(PlaySceneMain.Instance.Player.Strength), tempAddedStr },
            { nameof(PlaySceneMain.Instance.Player.Dexterity), tempAddedDex },
            { nameof(PlaySceneMain.Instance.Player.Constitution), tempAddedCon } 
        });
        ResetAddedStat();
    }

    /// <summary>
    /// 임시로 올려 본 스탯 리셋
    /// </summary>
    private void ResetAddedStat()
    {
        isStatAdding = false;
        tempLeftStatPoint = PlaySceneMain.Instance.Player.StatPoint;
        tempAddedStr = 0;
        tempAddedDex = 0;
        tempAddedCon = 0;

        strStatText.text = PlaySceneMain.Instance.Player.Strength.ToString();
        strStatText.color = Color.white;
        dexStatText.text = PlaySceneMain.Instance.Player.Dexterity.ToString();
        dexStatText.color = Color.white;
        conStatText.text = PlaySceneMain.Instance.Player.Constitution.ToString();
        conStatText.color = Color.white;
        UpdateStat();
        UpdateOriginStat();
    }

    private int AddedPoint()
    {
        return tempAddedStr + tempAddedDex + tempAddedCon;
    }
}
