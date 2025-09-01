using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaknessSlash : Skill
{
    private IDamagable attacker;
    private IDamagable target;
    private int usedSkillLevel;
    private Action onSkillFinish;

    public override void Active(Transform attackerTr, Transform targetTr, int skillLevel, float attackSpeed, Action onSkillEffectFinish)
    {
        attacker = attackerTr.GetComponent<IDamagable>();
        target = targetTr.GetComponent<IDamagable>();
        onSkillFinish = onSkillEffectFinish;
        usedSkillLevel = skillLevel;

        Vector2 direction = (targetTr.position - attackerTr.position).normalized / 3f;

        transform.position = attackerTr.position + new Vector3(direction.x, direction.y + 0.5f, 0f);
        transform.rotation = Quaternion.Euler(
            0f
            , 0f
            , Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

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
        usedSkillLevel = 0;
    }
}
