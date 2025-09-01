using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillTableCell", menuName = "Scriptable Object/SkillTableCell", order = int.MaxValue)]
public class SkillTableCell : ScriptableObject
{
    public int Code;
    public Skill Skill;
    public EquipmentTableCell.EquipmentTag availableEquipmentTag;
}
