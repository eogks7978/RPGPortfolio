using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 버튼을 누르면 간단하게 나오는 아이템 정보
/// </summary>
public class UISimpleItemDescription : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI itemNameText;

    private void Update()
    {
        if (Input.anyKey)
            Disappear();
    }

    public void Show(string itemName, Vector2 pos)
    {
        itemNameText.text = itemName;
        transform.position = pos;
        gameObject.SetActive(true);
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }
}
