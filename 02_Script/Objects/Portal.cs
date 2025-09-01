using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¸Ê ÀÌµ¿ Æ÷Å»
/// </summary>
public class Portal : MapObjectBase
{
    [SerializeField] private int codeForMapToMove;
    [SerializeField] private float spawnPosXForMapToChange = 0f;
    [SerializeField] private float spawnPosYForMapToChange = 0f;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private float mapChangeDelay = 3f;
    [SerializeField] private IEnumerator MapChangeCoroutine;

    private void Awake()
    {
        if (boxCollider2D == null)
            boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        MapChangeDelay();
    }

    private void OnDisable()
    {
        DisableCol();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            MapManager.Instance.ChangeMap(
                codeForMapToMove, new Vector3(
                    spawnPosXForMapToChange, spawnPosYForMapToChange, 0f));
        }
    }

    private void MapChangeDelay()
    {
        if (MapChangeCoroutine != null)
            StopCoroutine(MapChangeCoroutine);
        MapChangeCoroutine = MapChangeDelayCor();
        StartCoroutine(MapChangeCoroutine);
    }

    private IEnumerator MapChangeDelayCor()
    {
        yield return new WaitForSeconds(mapChangeDelay);
        boxCollider2D.enabled = true;
    }

    private void DisableCol()
    {
        boxCollider2D.enabled = false;
    }

    public override MapObjectData GetMapObjectData()
    {
        Dictionary<string, object> properties = new Dictionary<string, object>() {
            {nameof(codeForMapToMove), codeForMapToMove },
            {nameof(spawnPosXForMapToChange), spawnPosXForMapToChange},
            {nameof(spawnPosYForMapToChange), spawnPosYForMapToChange}
        };

        MapObjectData data = new(
            TbCell.Code,
            transform.position.x,
            transform.position.y,
            properties);

        return data;
    }

    public override void SetMapObjectData(MapObjectData data)
    {
        base.SetMapObjectData(data);

        codeForMapToMove = Convert.ToInt32(data.Properties[nameof(codeForMapToMove)]);
        spawnPosXForMapToChange = Convert.ToSingle(data.Properties[nameof(spawnPosXForMapToChange)]);
        spawnPosYForMapToChange = Convert.ToSingle(data.Properties[nameof(spawnPosYForMapToChange)]);
    }
}
