using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundTable", menuName = "Scriptable Object/SoundTable", order = int.MaxValue)]
public class SoundTable : ScriptableObject
{
    public List<SoundTableCell> Bgm;
    public List<SoundTableCell> Sfx;

    public SoundTableCell FindBgmWithCode(int code)
    {
        foreach (var cell in Bgm)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }

    public SoundTableCell FindSfxWithCode(int code)
    {
        foreach (var cell in Sfx)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }
}
