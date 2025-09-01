using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 맵 이동 시 로딩 판넬
/// </summary>
public class UILoadingPannel : MonoBehaviour
{
    [SerializeField] private Animator loadingCharacterAnim;

    public void ShowLoadingPannel()
    {
        gameObject.SetActive(true);

        StartCoroutine(OnLoading());
    }

    private IEnumerator OnLoading()
    {
        // 최소 팝업 Open 시간
        yield return new WaitForSeconds(.5f);

        // 데이터 로딩 기다림
        yield return new WaitWhile(() => !MapManager.Instance.MapLoadDone);

        gameObject.SetActive(false);
    }

}
