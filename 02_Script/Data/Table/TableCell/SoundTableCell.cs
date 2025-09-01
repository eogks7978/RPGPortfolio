using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundTableCell", menuName = "Scriptable Object/SoundTableCell", order = int.MaxValue)]
public class SoundTableCell : ScriptableObject
{
    public int Code;
    public AudioClip Clip;
}
