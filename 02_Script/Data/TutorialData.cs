using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 튜토리얼 데이터 형식
/// </summary>
[System.Serializable]
public class TutorialData
{
    [Header("튜토리얼 코드(테이블X)")] public int TutorialCode;
    [Header("튜토리얼에 나타날 NPC 테이블 코드")] public int NPCCode;
    [Header("튜토리얼 페이지 데이터")] public List<TutorialPageData> TutorialPageDatas;

    public TutorialData(int tutorialCode, int npcCode, List<TutorialPageData> tutorialPageData)
    {
        TutorialCode = tutorialCode;
        NPCCode = npcCode;
        TutorialPageDatas = tutorialPageData;
    }
}

/// <summary>
/// 튜토리얼 한 페이지당의 데이터
/// </summary>
public class TutorialPageData
{
    [Header("NPC 대화 내용")] public string Comment;
    [Header("구멍이 나 보일 부분의 사이즈")] public Vector2 MaskSize;
    [Header("구멍이 나 보일 부분 위치")] public Vector2 MaskPosition;
    [Header("구멍을 누르면 할 일들")] public List<Action> MaskButtonAction;

    public TutorialPageData(string comment, Vector2 maskSize, Vector2 maskPosition, List<Action> maskButtonAction)
    {
        Comment = comment;
        MaskSize = maskSize;
        MaskPosition = maskPosition;
        MaskButtonAction = maskButtonAction;
    }
}