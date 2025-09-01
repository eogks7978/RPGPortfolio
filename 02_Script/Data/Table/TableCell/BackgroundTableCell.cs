using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundTableCell", menuName = "Scriptable Object/BackgroundTableCell", order = int.MaxValue)]
public class BackgroundTableCell : ScriptableObject
{
    public int Code;
    public Sprite Sprite;
}
