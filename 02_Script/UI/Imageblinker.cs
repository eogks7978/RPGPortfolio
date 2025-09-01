using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 체력, 스킬 포인트 등이 남아있을 때 나타나서 알려주는 UI
/// </summary>
public class ImageBlinker : MonoBehaviour
{
    [SerializeField] private float blinkTic;
    [SerializeField] private Image thisImage;

    private void Awake()
    {
        if (!thisImage)
            thisImage = GetComponent<Image>();
    }

    public void Blink(bool isOn)
    {
        if (thisImage.enabled == isOn)
            return;

        thisImage.enabled = isOn;

        if (isOn)
        {
            StartCoroutine(BlinkCor());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator BlinkCor()
    {
        while (true)
        {
            thisImage.color = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b, 1f);

            yield return new WaitForSeconds(blinkTic);

            thisImage.color = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b, 0f);

            yield return new WaitForSeconds(blinkTic);
        }
    }
}
