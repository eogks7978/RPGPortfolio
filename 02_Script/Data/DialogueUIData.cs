using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dialogue UI에 매개변수로 전달 될 데이터
/// </summary>
public class DialogueUIData
{
    [Header("다이얼로그 페이지 인덱스")]public int dialogueIndex;
    [Header("대화 내용")] public string mainString;
    [Header("버튼에 표시될 string과 눌렀을 때 메서드")] public Dictionary<string, List<Action>> buttonStringAndMethod;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="dialogueIndex">다이얼로그 페이지 인덱스</param>
    /// <param name="mainString">대화 내용</param>
    /// <param name="buttonStringAndMethod">버튼에 표시될 string과 눌렀을 때 메서드</param>
    public DialogueUIData(
        int dialogueIndex,
        string mainString, 
        Dictionary<string, List<Action>> buttonStringAndMethod)
    {
        this.dialogueIndex = dialogueIndex;
        this.mainString = mainString;
        this.buttonStringAndMethod = buttonStringAndMethod;
    }
}
