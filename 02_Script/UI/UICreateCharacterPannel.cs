using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Ä³¸¯ÅÍ Ä¿½ºÅÒ ¹× »ý¼º ÆË¾÷
/// </summary>
public class UICreateCharacterPannel : UIPanel
{
    [SerializeField] private List<CharacterCustomButton> hairButtons;
    [SerializeField] private List<CharacterCustomButton> hairColorButtons;
    [SerializeField] private List<CharacterCustomButton> facehairButtons;
    [SerializeField] private List<CharacterCustomButton> facehairColorButtons;
    [SerializeField] private List<CharacterCustomButton> eyesButtons;
    [SerializeField] private List<CharacterCustomButton> eyesColorButtons;
    [SerializeField] private List<CharacterCustomButton> skinButtons;

    [SerializeField] private SpriteRenderer hairRenderer;
    [SerializeField] private SpriteRenderer facehairRenderer;
    [SerializeField] private SpriteRenderer leftEyeBack;
    [SerializeField] private SpriteRenderer rightEyeBack;
    [SerializeField] private SpriteRenderer leftEyeFront;
    [SerializeField] private SpriteRenderer rightEyeFront;
    [SerializeField] private List<SpriteRenderer> skinRenderers;

    [SerializeField] private RectTransform hairContentRt;
    [SerializeField] private RectTransform hairColorContentRt;
    [SerializeField] private RectTransform facehairContentRt;
    [SerializeField] private RectTransform facehairColorContentRt;
    [SerializeField] private RectTransform eyesContentRt;
    [SerializeField] private RectTransform eyesColorContentRt;
    [SerializeField] private RectTransform skinContentRt;

    [SerializeField] private TMP_InputField nickNameInputField;

    [SerializeField] private Button buttonCreateCharacter;

    private List<CharacterAppearanceTableCell> hairList;
    private List<CharacterAppearanceTableCell> facehairList;
    private List<CharacterAppearanceTableCell> eyesList;
    private List<CharacterAppearanceTableCell> skinList;

    private int hairCode;
    private int facehairCode;
    private int eyesCode;
    private int skinCode;

    private bool isChangingText;

    protected override void Awake()
    {
        base.Awake();
        nickNameInputField.onValueChanged.AddListener(OnNickNameInputFieldValueChanged);
        buttonCreateCharacter.onClick.AddListener(OnClickCreateCharacter);
        Initialize();
    }

    public override void Open(UnityAction<object> cbClose = null)
    {
        base.Open(cbClose);
        ResetUI();
    }

    public void ResetUI()
    {
        hairContentRt.localPosition = Vector2.zero;
        hairColorContentRt.localPosition = Vector2.zero;
        facehairContentRt.localPosition = Vector2.zero;
        facehairColorContentRt.localPosition = Vector2.zero;
        eyesContentRt.localPosition = Vector2.zero;
        eyesColorContentRt.localPosition = Vector2.zero;
        skinContentRt.localPosition = Vector2.zero;
        nickNameInputField.text = string.Empty;
        ChangeHair(hairList[0]);
        ChangeFacehair(facehairList[0]);
        ChangeEyes(eyesList[0]);
        ChangeSkin(skinList[0]);
        ChangeHairColor(CharacterCustomManager.Instance.Colors.First() / 255f);
        ChangeFacehairColor(CharacterCustomManager.Instance.Colors.First() / 255f);
        ChangeEyesColor(CharacterCustomManager.Instance.Colors.First() / 255f);
    }

    public void Initialize()
    {
        hairList = DataManager.Instance.TableDataTable.CharacterAppearanceTable.HairList;
        facehairList = DataManager.Instance.TableDataTable.CharacterAppearanceTable.FacehairList;
        eyesList = DataManager.Instance.TableDataTable.CharacterAppearanceTable.EyesList;
        skinList = DataManager.Instance.TableDataTable.CharacterAppearanceTable.SkinList;

        for (int i = 0; i < hairButtons.Count; ++i)
        {
            if (i < hairList.Count)
            {
                CharacterAppearanceTableCell tbCell = hairList[i];
                if (!tbCell) return;

                hairButtons[i].ThisButton.onClick.AddListener(() => ChangeHair(tbCell));
                hairButtons[i].ThisImage.sprite = tbCell.Sprite[0];
                hairButtons[i].ThisButton.gameObject.SetActive(true);
            }
            else
                break;
        }
        for (int i = 0; i < facehairButtons.Count; ++i)
        {
            if (i < facehairList.Count)
            {
                CharacterAppearanceTableCell tbCell = facehairList[i];
                if (!tbCell) return;

                facehairButtons[i].ThisButton.onClick.AddListener(() => ChangeFacehair(tbCell));
                facehairButtons[i].ThisImage.sprite = tbCell.Sprite[0];
                facehairButtons[i].ThisButton.gameObject.SetActive(true);
            }
            else
                break;
        }
        for (int i = 0; i < eyesButtons.Count; ++i)
        {
            if (i < eyesList.Count)
            {
                CharacterAppearanceTableCell tbCell = eyesList[i];
                if (!tbCell) return;

                eyesButtons[i].ThisButton.onClick.AddListener(() => ChangeEyes(tbCell));
                eyesButtons[i].ThisImage.sprite = tbCell.Sprite[0];
                eyesButtons[i].ThisButton.gameObject.SetActive(true);
            }
            else
                break;
        }
        for (int i = 0; i < skinButtons.Count; ++i)
        {
            if (i < skinList.Count)
            {
                CharacterAppearanceTableCell tbCell = skinList[i];
                if (!tbCell) return;

                skinButtons[i].ThisButton.onClick.AddListener(() => ChangeSkin(tbCell));
                skinButtons[i].ThisImage.sprite = tbCell.Sprite[0];
                skinButtons[i].ThisButton.gameObject.SetActive(true);
            }
            else
                break;
        }

        for (int i = 0; i < hairColorButtons.Count; ++i)
        {
            if (CharacterCustomManager.Instance.Colors.Count > i)
            {
                Color color = CharacterCustomManager.Instance.Colors[i] / 255f;
                hairColorButtons[i].ThisButton.onClick.AddListener(() => ChangeHairColor(color));
                hairColorButtons[i].ThisButton.gameObject.SetActive(true);
                hairColorButtons[i].SetColor(color);
            }
            else
                break;
        }
        for (int i = 0; i < facehairColorButtons.Count; ++i)
        {
            if (CharacterCustomManager.Instance.Colors.Count > i)
            {
                Color color = CharacterCustomManager.Instance.Colors[i] / 255f;
                facehairColorButtons[i].ThisButton.onClick.AddListener(() => ChangeFacehairColor(color));
                facehairColorButtons[i].ThisButton.gameObject.SetActive(true);
                facehairColorButtons[i].SetColor(color);
            }
            else
                break;
        }
        for (int i = 0; i < eyesColorButtons.Count; ++i)
        {
            if (CharacterCustomManager.Instance.Colors.Count > i)
            {
                Color color = CharacterCustomManager.Instance.Colors[i] / 255f;
                eyesColorButtons[i].ThisButton.onClick.AddListener(() => ChangeEyesColor(color));
                eyesColorButtons[i].ThisButton.gameObject.SetActive(true);
                eyesColorButtons[i].SetColor(color);
            }
            else
                break;
        }

        float buttonSize = hairButtons.First().GetComponent<RectTransform>().rect.width;
        float spacing = hairContentRt.GetComponent<HorizontalLayoutGroup>().spacing;
        hairContentRt.sizeDelta = new Vector2(buttonSize * hairButtons.FindAll(n => n.gameObject.activeSelf).Count + (hairButtons.Count - 1) * spacing, 0f);
        hairColorContentRt.sizeDelta = new Vector2(buttonSize * hairColorButtons.FindAll(n => n.gameObject.activeSelf).Count + (hairColorButtons.Count - 1) * spacing, 0f);
        facehairContentRt.sizeDelta = new Vector2(buttonSize * facehairButtons.FindAll(n => n.gameObject.activeSelf).Count + (facehairButtons.Count - 1) * spacing, 0f);
        facehairColorContentRt.sizeDelta = new Vector2(buttonSize * facehairColorButtons.FindAll(n => n.gameObject.activeSelf).Count + (facehairColorButtons.Count - 1) * spacing, 0f);
        eyesContentRt.sizeDelta = new Vector2(buttonSize * eyesButtons.FindAll(n => n.gameObject.activeSelf).Count + (eyesButtons.Count - 1) * spacing, 0f);
        eyesColorContentRt.sizeDelta = new Vector2(buttonSize * eyesColorButtons.FindAll(n => n.gameObject.activeSelf).Count + (eyesColorButtons.Count - 1) * spacing, 0f);
        skinContentRt.sizeDelta = new Vector2(buttonSize * skinButtons.FindAll(n => n.gameObject.activeSelf).Count + (skinButtons.Count - 1) * spacing, 0f);
    }

    private void ChangeHair(CharacterAppearanceTableCell tbCell)
    {
        hairCode = tbCell.Code;
        hairRenderer.sprite = tbCell.Sprite[0];
    }
    private void ChangeFacehair(CharacterAppearanceTableCell tbCell)
    {
        facehairCode = tbCell.Code;
        facehairRenderer.sprite = tbCell.Sprite[0];
    }
    private void ChangeEyes(CharacterAppearanceTableCell tbCell)
    {
        eyesCode = tbCell.Code;
        leftEyeBack.sprite = tbCell.Sprite[0];
        rightEyeBack.sprite = tbCell.Sprite[0];
        leftEyeFront.sprite = tbCell.Sprite[1];
        rightEyeFront.sprite = tbCell.Sprite[1];
    }
    private void ChangeSkin(CharacterAppearanceTableCell tbCell)
    {
        skinCode = tbCell.Code;
        for (int i = 0; i < skinRenderers.Count; ++i)
        {
            if (i < tbCell.Sprite.Count)
                skinRenderers[i].sprite = tbCell.Sprite[i];
            else
                break;
        }
    }

    private void ChangeHairColor(Color color)
    {
        hairRenderer.color = color;
    }
    private void ChangeFacehairColor(Color color)
    {
        facehairRenderer.color = color;
    }
    private void ChangeEyesColor(Color color)
    {
        leftEyeFront.color = color;
        rightEyeFront.color = color;
    }

    private void OnNickNameInputFieldValueChanged(string text)
    {
        // 1. ¿µ¾î, ÇÑ±Û, ¼ýÀÚ¸¸ Çã¿ëÇÏ´Â Á¤±Ô½Ä
        string pattern = @"^[a-zA-Z0-9°¡-ÆR¤¡-¤¾¤¿-¤Ó]*$";
        if (!Regex.IsMatch(text, pattern))
        {
            // À¯È¿ÇÏÁö ¾ÊÀ¸¸é ¸¶Áö¸· ¹®ÀÚ¸¦ »èÁ¦
            text = text.Substring(0, text.Length - 1);
            nickNameInputField.text = text;
        }

        // 2. ¹ÙÀÌÆ® Å©±â°¡ 20¹ÙÀÌÆ®¸¦ ³ÑÁö ¾Ê°Ô Á¦ÇÑ
        if (GetByteLength(text) > 16)
        {
            // 20¹ÙÀÌÆ®¸¦ ³ÑÀ¸¸é ÀÚ¸¥´Ù
            text = text.Substring(0, text.Length - 1);
            nickNameInputField.text = text;
        }
    }

    // ¹®ÀÚ¿­ÀÇ ¹ÙÀÌÆ® ±æÀÌ¸¦ °è»êÇÏ´Â ¸Þ¼­µå (ÇÑ±ÛÀº 2¹ÙÀÌÆ®, ¿µ¾î´Â 1¹ÙÀÌÆ®)
    private int GetByteLength(string text)
    {
        int byteLength = 0;
        foreach (char c in text)
        {
            if (System.Text.Encoding.Default.GetByteCount(c.ToString()) > 1)
            {
                byteLength += 2;  // ÇÑ±ÛÀº 2¹ÙÀÌÆ®
            }
            else
            {
                byteLength += 1;  // ¿µ¾î, ¼ýÀÚ´Â 1¹ÙÀÌÆ®
            }
        }
        return byteLength;
    }

    private bool isAvailableName(string text)
    {
        string pattern = @"^[a-zA-Z0-9°¡-ÆR]*$";
        if (!Regex.IsMatch(text, pattern))
        {
            return false;
        }

        return true;
    }

    private void OnClickCreateCharacter()
    {
        if (nickNameInputField.text == string.Empty)
        {
            LobbySceneMain.Instance.OpenNotificationPopup("´Ð³×ÀÓÀ» ÀÔ·Â ÇØ ÁÖ½Ê½Ã¿À.");
            return;
        }

        if (!isAvailableName(nickNameInputField.text))
        {
            LobbySceneMain.Instance.OpenNotificationPopup("À¯È¿ÇÏÁö ¾ÊÀº ÀÌ¸§ÀÔ´Ï´Ù.");
            return;
        }

        LobbySceneMain.Instance.OpenNotificationPopup("ÀÌ´ë·Î »ý¼ºÇÏ½Ã°Ú½À´Ï±î?", true, "¾Ë¸²", "¿¹", "¾Æ´Ï¿À",
            (resultObj) =>
            {
                var result = resultObj as UINotificationPopup.Results;
                if (result.isConfirm)
                {
                    PlayerAppearanceData playerAppearanceData = new PlayerAppearanceData(
                        hairCode,
                        hairRenderer.color,
                        facehairCode,
                        facehairRenderer.color,
                        skinCode, 
                        eyesCode,
                        leftEyeFront.color);

                    CharacterCustomManager.Instance.CreatePlayerData(nickNameInputField.text, playerAppearanceData);
                    Close();
                }
            });
    }
}
