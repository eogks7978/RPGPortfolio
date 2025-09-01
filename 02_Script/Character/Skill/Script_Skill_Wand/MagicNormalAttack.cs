using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicNormalAttack : Skill
{
    private int usedSkillLevel;
    private Character targetCharacter;
    private Transform attackerTr;
    private Action onSkillFinish;
    [SerializeField] private int maxProjectileShotCount;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private RuntimeAnimatorController controller;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private AudioClip projectileHitSound;

    public override void Active(Transform attackerTr, Transform targetTr, int skillLevel, float attackSpeed, Action onSkillEffectFinish)
    {
        onSkillFinish = onSkillEffectFinish;
        usedSkillLevel = skillLevel;

        targetCharacter = targetTr.GetComponent<Character>();
        this.attackerTr = attackerTr;

        var data = skillLevelData.SkillLevelAndStatus.Find(n => n.key == usedSkillLevel);

        if (data == null)
            return;

        ISkillable iSkillable = attackerTr.GetComponent<ISkillable>();
        iSkillable.SetStatus(new Dictionary<string, float> { { nameof(ISkillable.CurrentMana), -data.value.requiredMana } });

        Shot();
    }

    public void Shot()
    {
        SkillTableCell tbCell = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithSkillName(skillName);

        if (!tbCell) return;

        Projectile projectile = ObjectPoolManager.Instance.GetObjectInMainPool("Projectile").GetComponent<Projectile>();
        projectile.SetProjectile(
            controller,
            targetCharacter.transform,
            attackerTr,
            attackerTr.position + new Vector3(0f, 0.3f, 0f),
            tbCell,
            projectileSpeed,
            true,
            new Vector2(0f, 0.3f),
            hitEffect,
            projectileHitSound);

        SoundEffect();
        DisableEvent();
    }


    private void SoundEffect()
    {
        SoundController.Instance.PlaySfx(soundEffect);
    }

    public void DisableEvent()
    {
        ResetField();
        onSkillFinish?.Invoke();
    }

    public void ResetField()
    {

    }
}
