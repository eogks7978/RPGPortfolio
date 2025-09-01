using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 스킬을 퀵슬롯에 넣기 위한 중간팝업
/// </summary>
public class UISkillSetPopup : UIPanel
{
    [SerializeField] private List<Button> skillSlots;
    [SerializeField] private List<Image> skillSlotImages;
    [SerializeField] private int selectedSkillCode;

    protected override void Awake()
    {
        Initialize();
        base.Awake();
    }

    public void Open(int skillCode, UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);
        selectedSkillCode = skillCode;
        UpdateSlot();
    }

    private void Initialize()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            // 클로저 때문에 생기는 문제 때문에 index변수를 선언. 지우면 안됨.
            int index = i;
            skillSlots[i].onClick.AddListener(() => SetSkill(ref selectedSkillCode, index));
        }
    }

    private void SetSkill(ref int skillCode, int index)
    {
        ActionBarSkillSlot slot = UIManager.Instance.SkillActionBar.SkillSlots[index];

        if (slot != null)
        {
            slot.SetSkill(skillCode);
            SkillManager.Instance.AddSkillToSlot(skillCode, index);
        }
    }

    private void UpdateSlot()
    {
        for (int i = 0; i < skillSlots.Count; i++) 
        {
            ActionBarSkillSlot slot = UIManager.Instance.SkillActionBar.SkillSlots[i];

            bool skillContained = slot.ContainedSkillCode != 0;

            skillSlotImages[i].sprite = skillContained ? slot.SkillImage.sprite : null;
            skillSlotImages[i].enabled = skillContained ? true : false;
        }
    }
}
