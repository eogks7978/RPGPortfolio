using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAppearanceTableCell", menuName = "Scriptable Object/CharacterAppearanceTableCell", order = int.MaxValue)]
public class CharacterAppearanceTableCell : ScriptableObject
{
    public int Code;
    public List<Sprite> Sprite;
}
