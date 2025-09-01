using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 아이템슬롯에 아이템 등록 시 나타나는 중간팝업
/// </summary>
public class UIItemSetPopup : UIPanel
{
    [SerializeField] private List<Button> itemSlots;
    [SerializeField] private List<Image> itemSlotImages;
    private Item item;

    protected override void Awake()
    {
        Initialize();
        base.Awake();
    }

    public void Open(Item itemSlot, UnityAction<object> cbClose = null)
    {
        this.item = itemSlot;
        UpdateSlot();
        base.Open(cbClose);
    }

    private void Initialize()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            // 클로저 때문에 생기는 문제 때문에 index변수를 선언. 지우면 안됨.
            int index = i;
            itemSlots[i].onClick.AddListener(() => SetItem(ref item, index));
        }
    }

    private void SetItem(ref Item item, int index)
    {
        UIManager.Instance.ItemHotbar.Add(item, index);
    }

    private void UpdateSlot()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (i >= UIManager.Instance.ItemHotbar.HotbarSlots.Length)
                return;

            HotbarSlot slot = UIManager.Instance.ItemHotbar.HotbarSlots[i];

            if (slot == null) 
                continue;

            itemSlotImages[i].gameObject.SetActive(slot.SlotItem != null);

            itemSlotImages[i].sprite = slot.ItemIconImage.sprite;
        }
    }
}
