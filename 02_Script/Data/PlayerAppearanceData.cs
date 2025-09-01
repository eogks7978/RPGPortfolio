using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 외형 데이터
/// </summary>
[System.Serializable]
public class PlayerAppearanceData
{
    [Header("머리 외형 테이블 코드")] public int HairCode;
    public float HairColorR;
    public float HairColorG;
    public float HairColorB;

    [Header("수염 외형 테이블 코드")] public int FacehairCode;
    public float FacehairColorR;
    public float FacehairColorG;
    public float FacehairColorB;

    [Header("피부 외형 테이블 코드")] public int SkinCode;

    [Header("눈 외형 테이블 코드")] public int EyesCode;
    public float EyesColorR;
    public float EyesColorG;
    public float EyesColorB;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hairCode">머리 외형 테이블 코드</param>
    /// <param name="hairColor"></param>
    /// <param name="facehairCode">수염 외형 테이블 코드</param>
    /// <param name="facehairColor"></param>
    /// <param name="skinCode">피부 외형 테이블 코드</param>
    /// <param name="eyesCode">눈 외형 테이블 코드</param>
    /// <param name="eyesColor"></param>
    public PlayerAppearanceData(
        int hairCode, 
        Color hairColor, 
        int facehairCode,
        Color facehairColor, 
        int skinCode, 
        int eyesCode,
        Color eyesColor)
    {
        HairCode = hairCode;
        HairColorR = hairColor.r;
        HairColorG = hairColor.g;
        HairColorB = hairColor.b;

        FacehairCode = facehairCode;
        FacehairColorR = facehairColor.r;
        FacehairColorG = facehairColor.g;
        FacehairColorB = facehairColor.b;

        SkinCode = skinCode;

        EyesCode = eyesCode;
        EyesColorR = eyesColor.r;
        EyesColorG = eyesColor.g;
        EyesColorB = eyesColor.b;
    }
}
