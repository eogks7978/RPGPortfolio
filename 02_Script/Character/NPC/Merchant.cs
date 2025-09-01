using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : NPC
{
    [SerializeField] private List<ConsumableItem> consumableItemList;
    [SerializeField] private List<EquipmentItem> equipmentItemList;
    [SerializeField] private List<EtcItem> etcItemList;

    protected override void Awake()
    {
        List<Item> items = new List<Item>();
        items.AddRange(equipmentItemList);
        items.AddRange(consumableItemList);
        items.AddRange(etcItemList);

        dialogueUIData = new List<DialogueUIData>() {
            new DialogueUIData(
                0,
                "나를 겨우 포트폴리오 게임 세상에 집어넣다니.. 두고보자 개발자..",
                new Dictionary<string, List<Action>>() { 
                    {"상점", new List<Action> { () => UIManager.Instance.OpenStorePopup(items), () => UIManager.Instance.DialoguePopup.Close() } } }
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
            Merchant npc = obj.GetComponent<Merchant>();

            if (npc)
                UIManager.Instance.OpenDialoguePopup(npc, tbCell.NPCName, dialogueUIData, openDialogueSound);
        }
    }
}
