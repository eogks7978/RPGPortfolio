using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 맵 밖으로 나가는 일 방지하는 클래스
/// </summary>
public class OffmapDefender : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlaySceneMain playSceneMainInst = PlaySceneMain.Instance;

        if (playSceneMainInst != null)
        {
            if (playSceneMainInst.Player)
                playSceneMainInst.Player.transform.position =
                MapManager.Instance.CurrentMap.PlayerDefaultPosTr.position;
        }
    }
}
