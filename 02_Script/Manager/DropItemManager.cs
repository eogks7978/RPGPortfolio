using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DropItem 관리자
/// </summary>
public class DropItemManager : MonoBehaviour
{
    #region singleton
    private static DropItemManager instance;
    public static DropItemManager Instance => instance;
    #endregion

    [SerializeField] private float randomDropRange;

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

    private void OnEnable()
    {
        EventHandler.OnMonsterDead += DropItem;
    }

    private void OnDisable()
    {
        EventHandler.OnMonsterDead -= DropItem;
    }

    /// <summary>
    /// Item 드랍
    /// </summary>
    /// <param name="monster"></param>
    private void DropItem(Monster monster)
    {
        MonsterTableCell tbCell = DataManager.Instance.TableDataTable.MonsterTable.FindTableCellWithName(monster.name);

        if (tbCell != null)
        {
            // 드랍 아이템 리스트 검사
            foreach(var data in tbCell.DropItemData.data)
            {
                InventoryItemTableCell itemTbCell = data.ItemTbCell;

                // 데이터 안의 아이템의 테이블을 찾았다면
                if (itemTbCell != null)
                {
                    float randomRate = Random.Range(0f, 1f);

                    // 드랍 성공
                    if (randomRate <= data.DropRate)
                    {
                        int quantity = Random.Range(1, data.MaxDropCount + 1);

                        DroppedItem droppedItem = ObjectPoolManager.Instance.GetObjectInMainPool(nameof(DroppedItem)).GetComponent<DroppedItem>();
                        droppedItem.transform.position = (Vector2)monster.transform.position + Random.insideUnitCircle * randomDropRange;

                        Item newItem = null;

                        switch(itemTbCell._ItemType)
                        {
                            case InventoryItemTableCell.ItemType.Equipment:
                                newItem = new EquipmentItem(itemTbCell.Code, quantity, 0, null, false);
                                break;
                            case InventoryItemTableCell.ItemType.Consumable:
                                newItem = new ConsumableItem(itemTbCell.Code, quantity);
                                break;
                            case InventoryItemTableCell.ItemType.Etc:
                                newItem = new EtcItem(itemTbCell.Code, quantity);
                                break;
                        }

                        if (newItem == null)
                            return;

                        droppedItem.SetItem(newItem);
                    }
                }
            }
        }
    }
}
