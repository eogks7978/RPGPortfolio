using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

/// <summary>
/// 튜토리얼 팝업
/// </summary>
public class UITutorialPopup : UIPanel
{
    [SerializeField, Header("밝게 보여 줄 사각 영역 위의 버튼")] 
    private Button maskButton;
    [SerializeField, Header("어두운 배경 겸 버튼")] 
    private Button backgroundButton;
    [SerializeField, Header("마스크 버튼 Recttransform")] 
    private RectTransform maskButtonRt;
    [SerializeField, Header("밝게 보여 줄 사각 영역")] 
    private RectTransform maskImageRt;
    [SerializeField, Header("NPC 오브젝트를 불러 올 영역")]
    private Transform npcTr;
    [SerializeField, Header("불러 올 NPC의 스케일")] 
    private Vector3 npcScale;
    [SerializeField, Header("대화 텍스트")] 
    private TMPro.TextMeshProUGUI tutorialCommentText;
    [SerializeField, Header("밝은 영역을 강조 할 이펙트")]
    private GameObject maskSuqareOutline;
    [Tooltip("현재 불러 온 튜토리얼 데이터")] 
    private TutorialData data;
    [Tooltip("불러 올 NPC")]
    private Character interlocutor;
    [Tooltip("현재 진행중인 튜토리얼 데이터 인덱스")] 
    private int currentIndex;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Open(TutorialData data, UnityAction<object> cbClose = null)
    {
        if (data == null)
            return;

        this.data = data; 

        currentIndex = 0;
        var npcTbCell = DataManager.Instance.TableDataTable.NPCTable.FindTableCellWithCode(data.NPCCode);
        if (npcTbCell)
        {
            interlocutor = ObjectPoolManager.Instance.GetObjectInSubPool(npcTbCell.Prefab.name).GetComponent<NPC>();
            interlocutor.transform.SetParent(npcTr);
            interlocutor.transform.localPosition = Vector3.zero;
            interlocutor.transform.localScale = npcScale;
            interlocutor.SetAsUI(true);
        }

        UpdatePopup();
        base.Open(cbClose);
    }

    public override void Close()
    {
        TutorialManager.Instance.SaveTutorialData(data.TutorialCode);

        base.Close();
        if (interlocutor)
        {
            ObjectPoolManager.Instance.ReturnObjectToSubPool(interlocutor.gameObject);
            interlocutor.SetAsUI(false);
        }
    }

    public void UpdatePopup()
    {
        backgroundButton.interactable = data.TutorialPageDatas[currentIndex].MaskSize == Vector2.zero;

        var pageData = data.TutorialPageDatas[currentIndex];

        tutorialCommentText.text = pageData.Comment;
        maskImageRt.localPosition = pageData.MaskPosition;
        maskButtonRt.localPosition = pageData.MaskPosition;
        maskImageRt.sizeDelta = pageData.MaskSize;
        maskButtonRt.sizeDelta = pageData.MaskSize;

        if (pageData.MaskButtonAction != null)
        {
            foreach (var action in pageData.MaskButtonAction)
            {
                maskButton.onClick.AddListener(() => action());
            }
        }

        maskSuqareOutline.SetActive(pageData.MaskSize != Vector2.zero);
    }

    /// <summary>
    /// 데이터의 다음 인덱스로 넘어감
    /// </summary>
    public void ShowNextIndex()
    {
        int nextIndex = currentIndex + 1;
        if (data.TutorialPageDatas.Count <= nextIndex)
        {
            Close();
            return;
        }

        currentIndex = nextIndex;
        maskButton.onClick.RemoveAllListeners();
        UpdatePopup();
    }
}
