using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ItemSlotUI : MonoBehaviour
{
    [SerializeField] protected Image itemIconImage = null;

    public int SlotIndex;

    public abstract Item SlotItem { get; set; }
    public Image ItemIconImage => itemIconImage;

    private void OnEnable() => UpdateSlotUI();

    protected virtual void Start()
    {
        SlotIndex = transform.GetSiblingIndex();
        UpdateSlotUI();
    }

    public abstract void UpdateSlotUI();

    protected virtual void EnableSlotUI(bool enable) => itemIconImage.enabled = enable;
}
