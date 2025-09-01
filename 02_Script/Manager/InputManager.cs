using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 2D 오브젝트 클릭 관리자
/// </summary>
public class InputManager : MonoBehaviour
{
    #region singleton
    private static InputManager instance;
    public static InputManager Instance => instance;

    #endregion

    [SerializeField] private int uiLayerToIgnore;
    [SerializeField] private int uiLayerToNotWork;

    [SerializeField] private Monster selectedMonster;
    public Monster SelectedMonster => selectedMonster;

    private void Awake()
    {
        //if (instance == null)
        //{
              instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // UI충돌유무 검사
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> uiHits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, uiHits);

            foreach (var hitUI in uiHits)
            {
                if (hitUI.gameObject.layer != uiLayerToIgnore)
                    return;
            }
            
            // 터치된 위치 검사
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPosition.z = 0;

            // 충돌된 모든 Collider 반환
            List<Collider2D> colliders = Physics2D.OverlapPointAll(touchPosition).ToList();

            if (colliders != null)
            {
                // NPC 터치 검사
                for (int i = 0; i < colliders.Count; ++i)
                {
                    NPC npc = colliders[i].GetComponentInParent<NPC>();
                    if (npc != null)
                    {
                        if(Vector2.Distance(npc.transform.position, PlaySceneMain.Instance.Player.transform.position) < 2.5f)
                        {
                            npc.OnClickNPC();
                        }
                        else
                        {
                            UIManager.Instance.ShowSimpleNotification("더 가까이 가야합니다.", Color.red);
                        }
                        return;
                    }
                }

                // 몬스터 터치 검사
                if (!PlaySceneMain.Instance.Player.IsAttacking)
                {
                    for (int i = 0; i < colliders.Count; ++i)
                    {
                        Monster monster = colliders[i].GetComponentInParent<Monster>();
                        if (monster != null)
                        {
                            SetSelectedMonster(monster);
                            return;
                        }
                    }
                }
            }
        }
    }

    public void SetSelectedMonster(Monster monster)
    {
        selectedMonster = monster;
        EventHandler.OnMonsterSelected?.Invoke();
    }
}
