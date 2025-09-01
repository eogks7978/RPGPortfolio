using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craftsman : NPC
{
    protected override void Awake()
    {
        dialogueUIData = new List<DialogueUIData>() {
            new DialogueUIData(
                0,
                "간단하게 만들려고 했는데 만들다보니 오랜 시간동안 이것 저것 다 만들어버렸어..",
                new Dictionary<string, List<Action>>() {
                    {"제작", new List<Action> { () => UIManager.Instance.OpenCraftingPopup(), () => UIManager.Instance.DialoguePopup.Close() } } }
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
            Craftsman npc = obj.GetComponent<Craftsman>();

            if (npc)
                UIManager.Instance.OpenDialoguePopup(npc, tbCell.NPCName, dialogueUIData, openDialogueSound);
        }
    }
}
