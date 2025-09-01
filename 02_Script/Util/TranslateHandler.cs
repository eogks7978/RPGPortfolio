using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI에 스탯명을 한글로 표기하기 위한 메서드가 있는 클래스
/// </summary>
public class TranslateHandler : MonoBehaviour
{
    public static Dictionary<string, string> List = new()
    {
        { "MaxHealth", "최대 체력" },
        { "MaxMana", "최대 마나" },
        { "AttackPoint", "공격력" },
        { "DefensePoint", "방어력" },
        { "AttackSpeed", "공격속도" },
        { "MoveSpeed", "이동속도" },
        { "Strength", "힘" },
        { "Dexterity", "민첩" },
        { "Constitution", "건강" },
    };

    public static string Translate(string stringToTranslate)
    {
        foreach (var name in List)
        {
            if (name.Key == stringToTranslate)
                return name.Value;
        }

        return string.Empty;
    }
}
