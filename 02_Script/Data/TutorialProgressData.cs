using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 튜토리얼 진행도 데이터
/// </summary>
public class TutorialProgressData
{
    [Header("이미 진행 된 튜토리얼의 코드")] public List<int> TutorialInProgress;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="tutorialCode">튜토리얼 코드(테이블X) 리스트</param>
    public TutorialProgressData(List<int> tutorialCode)
    {
        TutorialInProgress = tutorialCode;
    }
}
