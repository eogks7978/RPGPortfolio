using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 스킬의 정보를 보여주는 UI
/// </summary>
public class UISkillInfoPopup : UIPanel
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private GameObject nextAbilityarrow;
    [SerializeField, Header("스킬 레벨 업 버튼")] private Button levelUpBtn;
    [SerializeField, Header("스킬 퀵슬롯 장착 버튼")] private Button setBtn;
    [SerializeField, Header("스킬 퀵슬롯 해제 버튼")] private Button unSetBtn;
    [SerializeField, Header("스킬 이름 Text")] private TMPro.TextMeshProUGUI skillNameText;
    [SerializeField, Header("현재 스킬 레벨을 표시")] private TMPro.TextMeshProUGUI skillCurrentLevelText;

    [SerializeField, Header("현재 스킬 레벨에 대한 능력치")] private Transform CurrentSkillDataTr;
    [SerializeField, Header("다음 스킬 레벨에 대한 능력치")] private Transform NextSkillDataTr;

    [SerializeField, Header("현재 스킬 레벨의 능력치 표시")] private TMPro.TextMeshProUGUI currentLevelskillAbillityText;
    [SerializeField, Header("다음 스킬 레벨의 능력치 표시 ")] private TMPro.TextMeshProUGUI nextLevelskillAbillityText;

    [SerializeField, Header("현재 스킬 레벨의 쿨타임")] private TMPro.TextMeshProUGUI currentLevelskillCooltimeText;
    [SerializeField, Header("다음 스킬 레벨의 쿨타임")] private TMPro.TextMeshProUGUI nextLevelskillCooltimeText;

    [SerializeField, Header("현재 스킬 레벨의 필요 마나")] private TMPro.TextMeshProUGUI currentLevelRequiredManaText;
    [SerializeField, Header("다음재 스킬 레벨의 필요 마나")] private TMPro.TextMeshProUGUI nextLevelRequiredManaText;

    [SerializeField] private TMPro.TextMeshProUGUI skillDescriptionText;
    [SerializeField] private TMPro.TextMeshProUGUI setBtnText;
    [SerializeField] private TMPro.TextMeshProUGUI AvailableLevelText;

    [SerializeField, Header("장착중인 스킬 표시")] private string statusUsing;
    [SerializeField, Header("장착 string 표시")] private string statusNotUsing;
    [SerializeField, Header("사용불가 무기라는 표시")] private string statusNotFitWeapon;
    [SerializeField, Header("스킬레벨 부족하다는 표시")] private string notEnoughSkillLevel;

    private Skill containedSkill;
    private int skillCode;

    private void Start()
    {
        setBtn.onClick.AddListener(OnClickOpenSkillSetPopup);
        unSetBtn.onClick.AddListener(OnClickUnSetSkill);
        levelUpBtn.onClick.AddListener(OnClickLevelUp);
    }

    private void OnEnable()
    {
        EventHandler.OnSkillLevelUp += UpdateInfo;
        EventHandler.OnPlayerLevelUp += UpdateInfo;
    }

    private void OnDisable()
    {
        EventHandler.OnSkillLevelUp -= UpdateInfo;
        EventHandler.OnPlayerLevelUp -= UpdateInfo;
    }

    public void Open(int code, UnityAction<object> cbClose = null)
    {
        containedSkill = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithCode(code)
            .Skill;
        skillCode = code;
        UpdateInfo();
        base.Open(cbClose);
    }

    private void UpdateInfo()
    {
        int currentSkillLevel = SkillManager.Instance.GetSkillLevel(skillCode);

        skillIcon.sprite = containedSkill.Icon;
        skillNameText.text = containedSkill.SkillName;
        skillCurrentLevelText.text = currentSkillLevel.ToString();
        skillDescriptionText.text = string.Copy(containedSkill.Description);

        var skillCurrentLevelData = containedSkill.SkillLevelData.SkillLevelAndStatus.Find(n => n.key == currentSkillLevel);
        bool existSkillCurrentLevelData = skillCurrentLevelData != null;

        if (currentSkillLevel != 0)
        {
            if (existSkillCurrentLevelData)
            {
                string resultText = "";

                for (int i = 0; i < skillCurrentLevelData.value.StatMagnification.Count; ++i)
                {
                    resultText +=
                        TranslateHandler.Translate(skillCurrentLevelData.value.StatMagnification[i].statName)
                        + " x " 
                        + skillCurrentLevelData.value.StatMagnification[i].value;

                    if (i < skillCurrentLevelData.value.StatMagnification.Count - 1)
                        resultText += " + ";
                }

                currentLevelskillAbillityText.text = resultText;
                currentLevelskillCooltimeText.text = skillCurrentLevelData.value.Cooldown + "초";
                currentLevelRequiredManaText.text = skillCurrentLevelData.value.requiredMana.ToString();
            }
        }

        var skillNextLevelData = containedSkill.SkillLevelData.SkillLevelAndStatus.Find(n => n.key == currentSkillLevel + 1);
        bool existSkillNextLevelData = skillNextLevelData != null;

        if (existSkillNextLevelData)
        {
            string resultText = "";

            for (int i = 0; i < skillNextLevelData.value.StatMagnification.Count; ++i)
            {
                resultText +=
                    TranslateHandler.Translate(skillNextLevelData.value.StatMagnification[i].statName)
                    + " x "
                    + skillNextLevelData.value.StatMagnification[i].value;

                if (i < skillNextLevelData.value.StatMagnification.Count - 1)
                    resultText += " + ";
            }

            nextLevelskillAbillityText.text = resultText;
            nextLevelskillCooltimeText.text = skillNextLevelData.value.Cooldown + "초";
            nextLevelRequiredManaText.text = skillNextLevelData.value.requiredMana.ToString();
        }

        CurrentSkillDataTr.gameObject.SetActive(existSkillCurrentLevelData);
        NextSkillDataTr.gameObject.SetActive(existSkillNextLevelData);
        nextAbilityarrow.SetActive(existSkillCurrentLevelData && existSkillNextLevelData);

        int availableLevel = containedSkill.AvailableLevel;
        bool isAvailable = PlaySceneMain.Instance.Player.Level
            >= availableLevel;
        bool isMaxSkillLevel = currentSkillLevel >= containedSkill.SkillLevelData.maxSkillLevel;

        levelUpBtn.gameObject.SetActive(isAvailable && !isMaxSkillLevel);
        AvailableLevelText.text = availableLevel.ToString();
        AvailableLevelText.color = isAvailable ? Color.green : Color.red;
        SkillSlotChanged();
    }

    /// <summary>
    /// 스킬 슬롯들에 변화가 생겼을 때 업데이트
    /// </summary>
    private void SkillSlotChanged()
    {
        InventoryItemTableCell weaponTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(InventoryManager.Instance.UsingWeapon.Code);

        if (!weaponTbCell)
            return;

        var tbCell = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithCode(skillCode);
        var tbCellWeapon = InventoryManager.Instance.UsingWeapon != null 
            ? weaponTbCell as EquipmentTableCell
            : null;

        bool skillAlreadyInSlot = SkillManager.Instance.IsSkillSloted(skillCode);
        bool notFitWeapon = tbCellWeapon != null && tbCellWeapon.equipmentTag != tbCell.availableEquipmentTag;

        int skillLevel = SkillManager.Instance.GetSkillLevel(skillCode);

        setBtn.enabled = !skillAlreadyInSlot && skillLevel != 0 && !notFitWeapon;
        unSetBtn.gameObject.SetActive(skillAlreadyInSlot);

        if (skillAlreadyInSlot)
        {
            setBtnText.text = statusUsing;
            setBtnText.color = Color.red;
            return;
        }
        else if (skillLevel == 0)
        {
            setBtnText.text = notEnoughSkillLevel;
            setBtnText.color = Color.red;
            return;
        }
        else
        {
            if (!tbCellWeapon || notFitWeapon)
            {
                setBtnText.text = statusNotFitWeapon;
                setBtnText.color = Color.red;
                return;
            }
            setBtnText.text = statusNotUsing;
            setBtnText.color = Color.yellow;
        }
    }

    private void OnClickUnSetSkill()
    {
        UIManager.Instance.SkillActionBar.UnSetSlot(skillCode);
        SkillSlotChanged();
    }

    private void OnClickOpenSkillSetPopup()
    {
        UIManager.Instance.OpenSkillSetPopup(skillCode, (cbClose) => UpdateInfo());
    }

    private void OnClickLevelUp()
    {
        SkillManager.Instance.LevelUpSkill(skillCode);
    }
}
