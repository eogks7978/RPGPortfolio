using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShadowSlash : Skill
{
    [SerializeField] private LayerMask mask;
    private IDamagable attacker;
    private IDamagable target;
    private int usedSkillLevel;
    private Transform attackerTr;
    private Transform targetTr;

    private Action onSkillFinish;

    public override void Active(Transform attackerTr, Transform targetTr, int skillLevel, float attackSpeed, Action onSkillEffectFinish)
    {
        attacker = attackerTr.GetComponent<IDamagable>();
        target = targetTr.GetComponent<IDamagable>();
        onSkillFinish = onSkillEffectFinish;
        usedSkillLevel = skillLevel;
        this.attackerTr = attackerTr;
        this.targetTr = targetTr;

        transform.position = targetTr.position;

        var data = skillLevelData.SkillLevelAndStatus.Find(n => n.key == usedSkillLevel);

        if (data == null)
            return;

        ISkillable iSkillable = attackerTr.GetComponent<ISkillable>();
        iSkillable.SetStatus(new Dictionary<string, float> { { nameof(ISkillable.CurrentMana), -data.value.requiredMana } });

        thisAnim.SetFloat(nameof(attacker.AttackSpeed), attacker.AttackSpeed);
        gameObject.SetActive(true);
    }

    private void SoundEffect()
    {
        SoundController.Instance.PlaySfx(soundEffect);
    }

    public void SpeedMoveAndSlashToTarget()
    {
        SoundEffect();

        Vector2 direction = targetTr.position - attackerTr.position;
        Vector2 targetVector = (Vector2)targetTr.position + direction.normalized *.5f;

        Character character = attackerTr.GetComponent<Character>();

        if (character == null || character.Col == null)
            return;

        RaycastHit2D hit = Physics2D.Raycast(
            attackerTr.position,
            direction,
            Vector2.Distance((Vector2)targetTr.position + direction.normalized, attackerTr.position) + character.Col.radius * .5f,
            mask);

        attackerTr.position = 
            hit == default(RaycastHit2D) 
            ? targetVector 
            : hit.point - direction.normalized * character.Col.radius * .5f;
    }

    public override bool CanActive(Transform attackerTr, Transform targetTr, float halfRadiusOfCollider)
    {
        ISkillable skillable = attackerTr.GetComponent<ISkillable>();

        SkillTableCell tbCell = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithSkillName(skillName);
        SkillAndLevelData skillData = SkillManager.Instance.ManagedSkills.Find(n => n.skillCode == tbCell.Code);
        var statBonusData = skillLevelData.SkillLevelAndStatus.Find(n => n.key == skillData.level);
        if (statBonusData.value.requiredMana > skillable.CurrentMana)
            return false;

        Vector2 direction = targetTr.position - attackerTr.position;
        Vector2 targetVector = (Vector2)targetTr.position + direction.normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            attackerTr.position, 
            direction,
            Vector2.Distance((Vector2)targetTr.position + direction.normalized, attackerTr.position) + halfRadiusOfCollider, 
            mask);

        if (hit)
        {
            if (Vector2.Distance(hit.point, attackerTr.position)
                < Vector2.Distance(targetTr.position, attackerTr.position))
            {
                return false;
            }
            else
                return true;
        }

        return hit == default(RaycastHit2D);
    }

    public void StrkingEvent()
    {
        if (target == null)
            return;

        var levelDataOfSkill = skillLevelData.SkillLevelAndStatus.Find(n => n.key == usedSkillLevel);

        float damage = 0;

        foreach (StatMagnificationData data in levelDataOfSkill.value.StatMagnification)
        {
            object value = attacker.GetFieldValueByName(data.statName);

            if (value != null)
            {
                damage += Convert.ToSingle(value) * data.value;
            }
        }

        target.SetStatus(
            new Dictionary<string, float> { { nameof(target.CurrentHealth),
            -damage } });
    }

    public void DisableEvent()
    {
        Anim.Rebind();
        ResetField();
        onSkillFinish?.Invoke();
    }

    public void ResetField()
    {
        attacker = null;
        target = null;
        usedSkillLevel = 0;
    }
}
