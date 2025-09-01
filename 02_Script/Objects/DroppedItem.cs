using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 바닥에 떨어진 아이템 클래스
/// </summary>
public class DroppedItem : MonoBehaviour
{
    public enum State
    {
        Create, Idle, Drop
    }
    public State state = State.Idle;

    [SerializeField, Tooltip("포함된 Item")] private Item item;
    [SerializeField, Tooltip("플레이어 감지 collider")] private CircleCollider2D circleCollider2D;
    [SerializeField, Tooltip("자동 풀 반환 시간")] private float disableTime;
    [SerializeField, Tooltip("주울 시 플레이어한테 이동하는 애니메이션 속도")] private float dropSpeed;
    [SerializeField, Tooltip("아이템 이름 표시")] private TextMeshProUGUI itemNameText;
    [SerializeField] private SpriteRenderer itemSprite;
    [SerializeField] private AudioClip dropSound;
    private float passedTime = 0f;

    public Item Item => item;

    private void Awake()
    {
        if (!circleCollider2D)
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
        }
    }

    private void Update()
    {
        StateProcess();
    }

    private void OnEnable()
    {
        EventHandler.OnMapChanged += OnMapChanged;
        circleCollider2D.enabled = false;
        ChangeState(State.Create);
    }

    private void OnDisable()
    {
        EventHandler.OnMapChanged -= OnMapChanged;
    }

    public void SetItem(Item item)
    {
        this.item = item;
        InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);
        itemSprite.sprite = itemTbCell.Icon;
        itemNameText.text = itemTbCell.Name;
        itemNameText.color = itemTbCell.Rarity.TextColour;
    }

    private void StateProcess()
    {
        switch (state)
        {
            case State.Create:
                passedTime += Time.deltaTime;

                if (passedTime >= 1f)
                    ChangeState(State.Idle);
                break;
            case State.Idle:
                passedTime += Time.deltaTime;

                if (passedTime >= disableTime)
                    ObjectPoolManager.Instance.ReturnObjectToMainPool(gameObject);
                break;
            case State.Drop:
                transform.position = Vector2.MoveTowards(transform.position, PlaySceneMain.Instance.Player.transform.position, dropSpeed * Time.smoothDeltaTime);

                if (Vector2.Distance(transform.position, PlaySceneMain.Instance.Player.transform.position) <= 0.1f)
                {
                    SoundController.Instance.PlaySfx(dropSound);
                    ObjectPoolManager.Instance.ReturnObjectToMainPool(gameObject);
                }
                break;
        }
    }

    private void ChangeState(State s)
    {
        if (state == s) 
            return;

        state = s;

        switch (state)
        {
            case State.Create:
                passedTime = 0f;
                break;
            case State.Idle:
                circleCollider2D.enabled = true;
                passedTime = 0f;
                break;
            case State.Drop:
                circleCollider2D.enabled = false;
                InventoryItemTableCell tbCell = DataManager.Instance.TableDataTable.ItemTable.
                    FindTableCellWithCode(item.Code);
                UIManager.Instance.DropItemUI.SetText(tbCell.ColouredName);
                break;
        }
    }

    private void GetItem()
    {
        InventoryManager.Instance.TryGetItem(item,
            (isSuccess) =>
            {
                if (isSuccess)
                    ChangeState(State.Drop);
            });
    }

    private void OnMapChanged()
    {
        ObjectPoolManager.Instance.ReturnObjectToMainPool(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetItem();
        }
    }
}
