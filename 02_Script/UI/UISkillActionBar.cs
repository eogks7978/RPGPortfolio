using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ½ºÅ³ Äü½½·Ô
/// </summary>
public class UISkillActionBar : MonoBehaviour
{
    [SerializeField] private List<ActionBarSkillSlot> skillSlots;

    public List<ActionBarSkillSlot> SkillSlots => skillSlots;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        SkillData skillData = DataManager.LoadData<SkillData>(
            DataManager.DefaultSkillDataFileName,
            Application.persistentDataPath + "/" + DataManager.DefaultDataFolderName + DataManager.Instance.SelectedSaveSlot);

        List<SlotedSkillData> slotedSkillLevelInfos = skillData.SlotedSkillLevelInfos;

        if (slotedSkillLevelInfos == null)
            return;

        foreach(SlotedSkillData data in slotedSkillLevelInfos)
        {
            skillSlots[data.SlotNumberAndSkill.key].SetSkill(
                data.SlotNumberAndSkill.value.skillCode);
        }
    }

    /// <summary>
    /// Äü½½·Ô UI ³Ö±â
    /// </summary>
    /// <param name="skillCode"></param>
    public void UnSetSlot(int skillCode)
    {
        ActionBarSkillSlot slot = skillSlots.Find(n => n.ContainedSkillCode == skillCode);
        if (slot)
            slot.UnsetSkill();
    }

    /// <summary>
    /// Äü½½·Ô UI ÇØÁ¦
    /// </summary>
    public void UnSetAll()
    {
        foreach(ActionBarSkillSlot slot in skillSlots)
        {
            slot.UnsetSkill();
        }
    }

    public void ShowCooldown(int skillCode)
    {
        ActionBarSkillSlot slot = skillSlots.Find(n => n.ContainedSkillCode == skillCode);

        if (slot)
        {
            slot.ShowCooldown();
        }
    }
}
