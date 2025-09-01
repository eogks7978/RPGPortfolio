using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 투사체 클래스. 몬스터 공격, 스킬 등에 쓰일 수 있음.
/// </summary>
public class Projectile : MonoBehaviour
{
    [Tooltip("타겟 Transform")] private Transform targetTr;
    [Tooltip("공격자 Transform")] private Transform attackerTr;

    [Tooltip("스킬 TableCell")] private SkillTableCell skillTableCell;
    [Tooltip("적중시 데미지")] private float damage;

    [Tooltip("투사체 속도")] private float speed;
    [Tooltip("날아가면서 이미지가 타겟 방향으로 회전할건지")] private bool isRotate;
    [Tooltip("타겟 Position의 Offset")] private Vector2 targetOffset;
    [Tooltip("공격 이펙트 Table상의 name")] private string hitEffectName;
    [Tooltip("적중 사운드")] private AudioClip hitSoundEffect;
    [Tooltip("적중시 콜백")] private Action<bool> cbDone;
    [SerializeField, Tooltip("투사체 SpriteRenderer")] private SpriteRenderer spriteRenderer;
    [SerializeField, Tooltip("투사체 애니메이터")] private Animator animator;

    Vector3 targetPos;

    private void FixedUpdate()
    {
        GoToTarget();
    }

    private void OnEnable()
    {
        EventHandler.OnMapChanged += DisableProjectile;
    }

    private void OnDisable()
    {
        EventHandler.OnMapChanged -= DisableProjectile;
    }

    /// <summary>
    /// 투사체 발사(스킬)
    /// </summary>
    /// <param name="projectileSprite"><투사체 이미지>
    /// <param name="controller"></ 투사체 애니메이션>
    /// <param name="targetTr"></param>
    /// <param name="attackerTr"></param>
    /// <param name="StartPosition"></param>
    /// <param name="damageMagnification"><데미지 배율>
    /// <param name="speed"></param>
    /// <param name="isRotate"><비행도중 목표 위치에 따른 회전>
    /// <param name="originProjectileRotationZ"><투사체 이미지가 보는 방향. 위=0 오른쪽=90 아래=180 왼쪽=270>
    /// <param name="cbDone"></param>
    public void SetProjectile(
        RuntimeAnimatorController controller,
        Transform targetTr,
        Transform attackerTr,
        Vector2 StartPosition,
        SkillTableCell skillTableCell,
        float speed,
        bool isRotate,
        Vector2 targetOffset,
        GameObject hitEffect = null,
        AudioClip hitSoundEffect = null,
        Action<bool> cbDone = null)
    {
        animator.runtimeAnimatorController = controller;
        this.targetTr = targetTr;
        this.attackerTr = attackerTr;
        this.speed = speed;
        this.isRotate = isRotate;
        this.targetOffset = targetOffset;
        hitEffectName = hitEffect ? hitEffect.name : null;
        this.cbDone = cbDone;
        this.hitSoundEffect = hitSoundEffect;
        this.skillTableCell = skillTableCell;

        transform.position = StartPosition;

        if (isRotate)
        {
            Vector3 direction = targetTr.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 스킬이 아닌 투사체 발사
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="targetTr"></param>
    /// <param name="attackerTr"></param>
    /// <param name="StartPosition"></param>
    /// <param name="damage"></param>
    /// <param name="speed"></param>
    /// <param name="isRotate"></param>
    /// <param name="targetOffset"></param>
    /// <param name="hitEffect"></param>
    /// <param name="hitSoundEffect"></param>
    /// <param name="cbDone"></param>
    public void SetProjectile(
    RuntimeAnimatorController controller,
    Transform targetTr,
    Transform attackerTr,
    Vector2 StartPosition,
    float damage,
    float speed,
    bool isRotate,
    Vector2 targetOffset,
    GameObject hitEffect = null,
    AudioClip hitSoundEffect = null,
    Action<bool> cbDone = null)
    {
        animator.runtimeAnimatorController = controller;
        this.targetTr = targetTr;
        this.attackerTr = attackerTr;
        this.speed = speed;
        this.isRotate = isRotate;
        this.targetOffset = targetOffset;
        hitEffectName = hitEffect ? hitEffect.name : null;
        this.cbDone = cbDone;
        this.hitSoundEffect = hitSoundEffect;
        this.damage = damage;

        transform.position = StartPosition;

        if (isRotate)
        {
            Vector3 direction = targetTr.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        gameObject.SetActive(true);
    }

    private void GoToTarget()
    {
        targetPos = (Vector2)targetTr.position + targetOffset;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPos,
            speed * Time.fixedDeltaTime);

        if (isRotate)
        {
            Vector3 direction = targetPos - transform.position;

            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        }

        if (transform.position == targetPos)
        {
            if (skillTableCell)
            {
                GiveSkillDamageToTarget();
            }
            else
            {
                GiveDamageToTarget();
            }

            cbDone?.Invoke(true);
            ShowEffect();
            SoundEffect();
            DisableProjectile();
        }
    }

    private void GiveSkillDamageToTarget()
    {
        IDamagable damagable = attackerTr.GetComponent<IDamagable>();

        if (damagable == null)
            return;

        IDamagable targetDamagable = targetTr.GetComponent<IDamagable>();

        if (targetDamagable == null)
            return;

        var skillLevelInfo = SkillManager.Instance.ManagedSkills.Find(n => n.skillCode == skillTableCell.Code);
        var levelDataOfSkill = skillTableCell.Skill.SkillLevelData.SkillLevelAndStatus.Find(n => n.key == skillLevelInfo.level);

        float damage = 0;

        foreach (StatMagnificationData data in levelDataOfSkill.value.StatMagnification)
        {
            object value = damagable.GetFieldValueByName(data.statName);

            if (value != null)
            {
                damage += Convert.ToSingle(value) * data.value;
            }
        }

        targetDamagable.SetStatus(new Dictionary<string, float> { { nameof(targetDamagable.CurrentHealth), -damage } });
    }

    private void GiveDamageToTarget()
    {
        IDamagable damagable = targetTr.GetComponent<IDamagable>();

        if (damagable == null)
            return;

        damagable.SetStatus(new Dictionary<string, float> { { nameof(damagable.CurrentHealth), -damage } });
    }

    private void ShowEffect()
    {
        if (hitEffectName != null && !hitEffectName.Equals(string.Empty))
        {
            GameObject effect = ObjectPoolManager.Instance.GetObjectInMainPool(hitEffectName);
            if (effect != null)
                effect.transform.position = targetPos;
        }
    }

    private void SoundEffect()
    {
        SoundController.Instance.PlaySfx(hitSoundEffect);
    }

    private void DisableProjectile()
    {
        ResetProjectile();
        ObjectPoolManager.Instance.ReturnObjectToMainPool(gameObject);
    }

    private void ResetProjectile()
    {
        skillTableCell = null;
        damage = 0f;
        targetTr = null;
        attackerTr = null;
        speed = 0;
    }
}
