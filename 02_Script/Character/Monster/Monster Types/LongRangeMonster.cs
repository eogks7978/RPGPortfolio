using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 투사체 공격을 하는 몬스터 클래스
/// </summary>
public class LongRangeMonster : Monster
{
    [SerializeField, Tooltip("공격 애니메이션 string")] private string attackAnimationName;
    [SerializeField, Tooltip("투사체 속도")] private float projectileSpeed;
    [SerializeField, Tooltip("투사체 애니메이션 컨트롤러")] private RuntimeAnimatorController controller;
    [SerializeField, Tooltip("공격 텀")] private float shotTerm;
    [SerializeField, Tooltip("명중 이펙트")] private GameObject hitEffect;
    [SerializeField, Tooltip("명중 사운드")] private AudioClip projectileHitSound;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void StopAttack()
    {
        base.StopAttack();
    }

    protected override IEnumerator AttackCor()
    {
        while (true)
        {
            if (attackDelayCoroutine != null)
                StopCoroutine(attackDelayCoroutine);
            attackDelayCoroutine = AttackDelayCor();
            yield return StartCoroutine(attackDelayCoroutine);

            isAttackMostionOn = true;
            CheckForLeftOrRightFacing(targetTr.position - transform.position);
            animator.SetFloat("AttackSpeed", AttackSpeed);
            animator.SetFloat(attackAnimationName, 1);
            animator.SetTrigger("Attack");
            attackDelayTime = 1f / AttackSpeed;
        }
    }

    public void Shot()
    {
        Projectile projectile = ObjectPoolManager.Instance.GetObjectInMainPool("Projectile").GetComponent<Projectile>();
        projectile.SetProjectile(
            controller,
            targetTr,
            transform,
            transform.position + new Vector3(0f, 0.3f, 0f),
            AttackPoint,
            projectileSpeed,
            true,
            new Vector2(0f, 0.3f),
            hitEffect,
            projectileHitSound);

        SoundController.Instance.PlaySfx(attackSound);
    }
}
