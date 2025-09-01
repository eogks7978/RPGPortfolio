using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Skill UI의 버튼형태 UI
/// </summary>
public class UISkillSlotCell : MonoBehaviour
{
    [SerializeField, Header("관리중인 스킬의 테이블 코드")] private int containedSkillCode;
    [SerializeField] private Button thisButton;
    [SerializeField] private Image buttonImage;
    [SerializeField] private GameObject pannel;
    [SerializeField, Header("현재 스킬의 레벨 표시")] private TMPro.TextMeshProUGUI containedSkillLevelText;

    private void Awake()
    {
        if (!thisButton)
            thisButton = GetComponent<Button>();
        Initialize();
    }

    private void Start()
    {
        thisButton.onClick.AddListener(OnClickButton);
    }

    private void OnEnable()
    {
        EventHandler.OnSkillLevelUp += UpdateCell;
        UpdateCell();
    }

    private void OnDisable()
    {
        EventHandler.OnSkillLevelUp -= UpdateCell;
    }

    private void Initialize()
    {
        buttonImage.sprite = DataManager.Instance.TableDataTable.SkillTable.
            FindTableCellWithCode(containedSkillCode)
            .Skill.Icon;
    }

    private void OnClickButton()
    {
        UIManager.Instance.OpenSkillInfoPopup(containedSkillCode);
    }

    private void UpdateCell()
    {
        int skillLevel = SkillManager.Instance.GetSkillLevel(containedSkillCode);

        containedSkillLevelText.text = skillLevel.ToString();
        pannel.SetActive(skillLevel == 0 ? true : false);
    }
}
