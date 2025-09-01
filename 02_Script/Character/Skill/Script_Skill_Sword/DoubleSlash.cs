using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSlash : Skill
{
    private IDamagable attacker;
    private IDamagable target;
    private Action onSkillFinish;
    private int usedSkillLevel;
    public override void Active(Transform attackerTr, Transform targetTr, int skillLevel, float attackSpeed, Action onSkillEffectFinish)
    {
        attacker = attackerTr.GetComponent<IDamagable>();
        target = targetTr.GetComponent<IDamagable>();
        onSkillFinish = onSkillEffectFinish;
        usedSkillLevel = skillLevel;

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
    }
}
