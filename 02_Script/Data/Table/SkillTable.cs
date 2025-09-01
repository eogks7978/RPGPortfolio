using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillTable", menuName = "Scriptable Object/SkillTable", order = int.MaxValue)]
public class SkillTable : ScriptableObject
{
    public List<SkillTableCell> List;

    public SkillTableCell FindTableCellWithCode(int code)
    {
        foreach (var cell in List)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }

    public SkillTableCell FindTableCellWithSkillName(string name)
    {
        foreach (var cell in List)
        {
            if (cell.Skill.SkillName == name)
                return cell;
        }
        return null;
    }
}
