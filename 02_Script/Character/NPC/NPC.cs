using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// NPC의 부모격 클래스
/// </summary>
public class NPC : Character
{
    [SerializeField, Header("클릭 Collider")] protected BoxCollider2D boxCollider;
    [SerializeField, Header("이름 Text가 포함된 Object")] protected GameObject nameTextObj;
    [SerializeField, Header("이름 TextMeshPro")] protected TMPro.TextMeshProUGUI nameText;
    [SerializeField, Header("음성(대화창 오픈시 사운드)")] protected AudioClip openDialogueSound;
    [Tooltip("Dialogue 오픈시 대화, 버튼 등의 상호작용 데이터")] protected List<DialogueUIData> dialogueUIData;

    protected virtual void Awake()
    {
        dialogueUIData = new List<DialogueUIData>() { new DialogueUIData(0, "....", null) };
    }

    /// <summary>
    /// NPC클릭 시 할 일(Default == Open Dialogue Popup)
    /// </summary>
    public virtual void OnClickNPC()
    {
        NPCTableCell tbCell = DataManager.Instance.TableDataTable.NPCTable.FindTableCellWithName(name);

        if (tbCell == null)
            return;

        GameObject obj = ObjectPoolManager.Instance.GetObjectInSubPool(tbCell.Prefab.name);

        if (obj)
        {
            NPC npc = obj.GetComponent<NPC>();

            if (npc)
                UIManager.Instance.OpenDialoguePopup(npc, tbCell.NPCName, dialogueUIData, openDialogueSound);
        }
    }

    public virtual void Spawn(Vector2 pos, string name)
    {
        nameText.text = name;
        transform.position = pos;
        transform.localScale = Vector2.one;
        nameTextObj.gameObject.SetActive(true);
        boxCollider.enabled = true;
    }

    /// <summary>
    /// 2d 오브젝트를 UI로서 사용하게 하는 세팅. 도감, 튜토리얼, 다이얼로그 등에 사용 가능. 
    /// </summary>
    /// <param name="isOn"></param>
    public override void SetAsUI(bool isOn)
    {
        base.SetAsUI(isOn);
        boxCollider.enabled = !isOn;
        nameTextObj.gameObject.SetActive(!isOn);
    }
}
