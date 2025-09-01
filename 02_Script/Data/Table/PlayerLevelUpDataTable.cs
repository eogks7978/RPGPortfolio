using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLevelUpDataTable", menuName = "Scriptable Object/PlayerLevelUpDataTable", order = int.MaxValue)]
public class PlayerLevelUpDataTable : ScriptableObject
{
    public List<PlayerLevelUpData> List;

    public PlayerLevelUpData FindTableCellWithLevel(float level)
    {
        foreach (var cell in List)
        {
            if (cell.Level == level)
                return cell;
        }
        return null;
    }
}
