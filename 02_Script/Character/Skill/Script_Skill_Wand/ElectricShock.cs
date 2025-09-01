using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://www.youtube.com/watch?v=VvjIUIlso9M
/// </summary>
public class ElectricShock : Skill
{
    private IDamagable attacker;
    private IDamagable target;
    private int usedSkillLevel;
    private Character attackerCharacter;
    private Character targetCharacter;
    private Action onSkillFinish;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Texture[] textures;

    [SerializeField] private float animationFps = 15f;
    [SerializeField] private float hitTick;
    [SerializeField] private int maxHitCount;

    private int animationStep;
    private float hitCounter;
    private float fpsCounter;
    private int hitCount;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        fpsCounter += Time.deltaTime;
        hitCounter += Time.deltaTime;

        // LineRenderer 설정
        SetLine();

        // 설정한 Tick당 타겟에 데미지 부여
        if (hitCounter > hitTick / attacker.AttackSpeed)
        {
            StrkingEvent();
            hitCount++;
            hitCounter -= hitTick / attacker.AttackSpeed;
        }

        // 애니메이션 프레임 설정
        if (fpsCounter * attacker.AttackSpeed >= 1f / animationFps)
        {
            animationStep++;
            if (animationStep == textures.Length)
                animationStep = 0;

            lineRenderer.material.SetTexture("_MainTex", textures[animationStep]);

            fpsCounter = 0f;
        }

        if (hitCount >= maxHitCount || targetCharacter.IsDead || attackerCharacter.IsDead)
        {
            DisableEvent();
        }
    }

    public override void Active(Transform attackerTr, Transform targetTr, int skillLevel, float attackSpeed, Action onSkillEffectFinish)
    {
        attacker = attackerTr.GetComponent<IDamagable>();
        target = targetTr.GetComponent<IDamagable>();
        usedSkillLevel = skillLevel;

        attackerCharacter = attackerTr.GetComponent<Character>();
        targetCharacter = targetTr.GetComponent<Character>();
        onSkillFinish = onSkillEffectFinish;

        SetLine();

        gameObject.SetActive(true);

        var data = skillLevelData.SkillLevelAndStatus.Find(n => n.key == usedSkillLevel);

        if (data == null)
            return;

        ISkillable iSkillable = attackerTr.GetComponent<ISkillable>();
        iSkillable.SetStatus(new Dictionary<string, float> { { nameof(ISkillable.CurrentMana), -data.value.requiredMana } });

        SoundEffect();
    }

    private void SoundEffect()
    {
        SoundController.Instance.PlaySfx(soundEffect);
    }
    private void SetLine()
    {
        Vector3 direction = (targetCharacter.transform.position - attackerCharacter.transform.position).normalized;
        transform.position = attackerCharacter.transform.position + new Vector3(direction.x / 5f, direction.y / 5f + 0.25f, 0f);

        lineRenderer.SetPosition(0, targetCharacter.transform.position - attackerCharacter.transform.position - direction / 4f);
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
        ResetField();
        onSkillFinish?.Invoke();
        gameObject.SetActive(false);
    }

    public void ResetField()
    {
        hitCount = 0;
        attacker = null;
        target = null;
    }
}
