using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAppearanceTable", menuName = "Scriptable Object/CharacterAppearanceTable", order = int.MaxValue)]
public class CharacterAppearanceTable : ScriptableObject
{
    public List<CharacterAppearanceTableCell> HairList;
    public List<CharacterAppearanceTableCell> FacehairList;
    public List<CharacterAppearanceTableCell> EyesList;
    public List<CharacterAppearanceTableCell> SkinList;

    public CharacterAppearanceTableCell FindHairTableCellWithCode(int code)
    {
        foreach (var cell in HairList)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }
    public CharacterAppearanceTableCell FindFacehairTableCellWithCode(int code)
    {
        foreach (var cell in FacehairList)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }
    public CharacterAppearanceTableCell FindEyesTableCellWithCode(int code)
    {
        foreach (var cell in EyesList)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }
    public CharacterAppearanceTableCell FindSkinTableCellWithCode(int code)
    {
        foreach (var cell in SkinList)
        {
            if (cell.Code == code)
                return cell;
        }
        return null;
    }
}
