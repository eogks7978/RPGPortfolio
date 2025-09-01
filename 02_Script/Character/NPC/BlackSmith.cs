using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSmith : NPC
{
    protected override void Awake()
    {
        dialogueUIData = new List<DialogueUIData>() {
            new DialogueUIData(
                0,
                "성공을 위해 실패를 두려워하지 말라! 내가 가장 좋아하는 명언이지.",
                new Dictionary<string, List<Action>>() {
                    {"대장간", new List<Action> { () => UIManager.Instance.OpenEnchantPopup(), () => UIManager.Instance.DialoguePopup.Close() } } }
                )
        };
    }

    public override void OnClickNPC()
    {
        NPCTableCell tbCell = DataManager.Instance.TableDataTable.NPCTable.FindTableCellWithName(name);

        if (tbCell == null)
            return;

        GameObject obj = ObjectPoolManager.Instance.GetObjectInSubPool(tbCell.Prefab.name);

        if (obj)
        {
            BlackSmith npc = obj.GetComponent<BlackSmith>();

            if (npc)
                UIManager.Instance.OpenDialoguePopup(npc, tbCell.NPCName, dialogueUIData, openDialogueSound);
        }
    }
}
