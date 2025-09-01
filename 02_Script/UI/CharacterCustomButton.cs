using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 캐릭터 외형 커스텀 버튼
/// </summary>
public class CharacterCustomButton : MonoBehaviour
{
    [SerializeField] private Button thisButton;
    [SerializeField] private Image thisImage;

    public Button ThisButton => thisButton;
    public Image ThisImage => thisImage;

    public void SetColor(Color color)
    {
        thisImage.color = color;
    }
}
