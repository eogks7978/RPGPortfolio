using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

/// <summary>
/// 대화 팝업
/// </summary>
public class UIDialoguePopup : UIPanel
{
    [SerializeField, Header("대화 주체 이름 Text")] private TMPro.TextMeshProUGUI nameText;
    [SerializeField, Header("대화 내용 Text")] private TMPro.TextMeshProUGUI dialogueText;
    [SerializeField, Header("대화 주체 Transform")] private Transform interlocutorObjTr;
    [SerializeField, Header("표시될 대화 주체 Scale")] private Vector3 interlocutorScale;
    [SerializeField, Header("대화 페이즈가 진행되고 나올 버튼들")] private List<SerializableKeyValuePair<Button, TMPro.TextMeshProUGUI>> dialoguebuttons;
    [SerializeField, Header("버튼들의 리스너에 추가 할 메서드들")] private List<UnityAction> methodInButton;
    [SerializeField, Header("대화 속도(낮을수록 빠름)")] private float letterDelay = 0.1f;
    [SerializeField] private Button nextButton;

    [Tooltip("대화 주체")] private Character interlocutor;
    [Tooltip("다이얼로그 데이터")] private List<DialogueUIData> dialogueData;
    [Tooltip("사용중인 다이얼로그 버튼")] private List<SerializableKeyValuePair<Button, TMPro.TextMeshProUGUI>> usingDialogueButtons = new();
    [Tooltip("현재 다이얼로그 데이터 인덱스")] private int currentDialogueIndex;

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(OnClickNext);
    }

    public void Open(NPC interlocutor, string name, List<DialogueUIData> dialogueData, AudioClip openSound = null, UnityAction<object> cbClose = null)
    {
        base.Open();

        if (interlocutor)
        {
            this.interlocutor = interlocutor;
            this.dialogueData = dialogueData;

            nameText.text = name;
            interlocutor.transform.SetParent(interlocutorObjTr);
            interlocutor.transform.localPosition = Vector3.zero;
            interlocutor.transform.localScale = interlocutorScale;
            interlocutor.SetAsUI(true);

            if (openSound)
                SoundController.Instance.PlaySfx(openSound);

            ChangeDialogueIndex(0);
        }
    }

    public override void Close()
    {
        base.Close();
        StopAllCoroutines();
        nextButton.interactable = false;
        if (interlocutor)
        {
            ObjectPoolManager.Instance.ReturnObjectToSubPool(interlocutor.gameObject);
            interlocutor.SetAsUI(false);
        }
    }

    public void ChangeDialogueIndex(int index)
    {
        ReSetAllDialogueButton();

        if (dialogueData.Count <= index)
            Close();

        currentDialogueIndex = index;

        DialogueUIData data = dialogueData[index];

        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogueData[index].mainString));

        nextButton.interactable = true;

        if (data.buttonStringAndMethod != null)
        {
            foreach (var buttonStringAndMethod in data.buttonStringAndMethod)
            {
                SetDialogueButton(buttonStringAndMethod);
            }
        }

        if (index == dialogueData.Count - 1)
        {
            SetDialogueButton(new KeyValuePair<string, List<Action>>("닫기", new List<Action> { () => Close() }));
        }
    }

    /// <summary>
    /// 버튼 Listener에 메서드 추가
    /// </summary>
    /// <param name="buttonStringAndMethod"></param>
    private void SetDialogueButton(KeyValuePair<string, List<Action>> buttonStringAndMethod)
    {
        foreach (var button in dialoguebuttons)
        {
            foreach(var action in buttonStringAndMethod.Value)
            {
                button.key.onClick.AddListener(() => action());
            }
            button.value.text = buttonStringAndMethod.Key;
            dialoguebuttons.Remove(button);
            usingDialogueButtons.Add(button);
            button.key.gameObject.SetActive(true);
            break;
        }
    }

    private void ReSetAllDialogueButton()
    {
        if (usingDialogueButtons == null)
            return;

        foreach (var button in usingDialogueButtons)
        {
            button.key.onClick.RemoveAllListeners();
            button.key.gameObject.SetActive(false);
        }

        if (usingDialogueButtons.Count != 0)
        {
            for (int i = usingDialogueButtons.Count - 1; i >= 0; i--)
            {
                dialoguebuttons.Insert(0, usingDialogueButtons[i]);
            }
            usingDialogueButtons.Clear();
        }
    }

    private void OnClickNext()
    {
        StopAllCoroutines();

        dialogueText.text = dialogueData[currentDialogueIndex].mainString;
    }

    // 대화 텍스트를 한 글자씩 출력하는 코루틴
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }

        yield break;
    }
}
