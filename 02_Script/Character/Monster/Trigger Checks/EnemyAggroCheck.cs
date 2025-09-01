using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggroCheck : MonoBehaviour
{
    private Monster monster;
    private Coroutine waitAttackDoneCoroutine;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == monster.TargetTag )
        {
            monster.SetTarget(collision.transform);
            monster.SetAggroStatus(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameObject.activeInHierarchy 
            && collision.gameObject.tag == monster.TargetTag)
        {
            if (waitAttackDoneCoroutine != null)
                StopCoroutine(waitAttackDoneCoroutine);
            waitAttackDoneCoroutine =
                StartCoroutine(WaitAttackDone());
        }
    }

    private IEnumerator WaitAttackDone()
    {
        yield return new WaitWhile(() => monster.IsAttacking);
        ChangeState();
    }

    private void ChangeState()
    {
        monster.SetTarget(null);
        monster.SetAggroStatus(false);
        monster.StateMachine.ChangeState(monster.BackToSpawnState);
    }
}


