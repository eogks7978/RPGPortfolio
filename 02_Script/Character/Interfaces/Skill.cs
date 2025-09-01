using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// 최상위(부모) 스킬 클래스
/// </summary>
public class Skill : MonoBehaviour
{
    [SerializeField, Header("스킬 이름")] protected string skillName;
    [SerializeField, Header("배울 수 있는 레벨")] protected int availableLevel;
    [SerializeField, Header("스킬 등급(스킬 시전 우선순위 정렬에 사용)")] protected int grade;
    [SerializeField, Header("스킬 UI 아이콘")] protected Sprite icon;
    [SerializeField, Header("스킬레벨 달성시 데미지 등의 수치 변화 데이터")] protected SkillLevelData skillLevelData;
    [SerializeField, Header("시전자의 공격 애니메이션 이름")] protected string attackAnimationName;
    [SerializeField] protected Animator thisAnim;
    [SerializeField, Header("스킬 효과음")] protected AudioClip soundEffect;
    [SerializeField, Header("UI에 표시될 스킬 설명")] protected string description;

    public string SkillName => skillName;
    public int AvailableLevel => availableLevel;
    public int Grade => grade;
    public Sprite Icon => icon;
    public SkillLevelData SkillLevelData => skillLevelData;
    public string AttackAnimationName => attackAnimationName;
    public Animator Anim => thisAnim;
    public string Description => description;

    /// <summary>
    /// 스킬 시전 함수
    /// </summary>
    /// <param name="attacker">시전자</param>
    /// <param name="targetTr">타겟 Transform</param>
    /// <param name="skillLevel">스킬 레벨</param>
    /// <param name="attackSpeed">시전자 공격속도</param>
    /// <param name="onSkillEffectFinish">스킬 이펙트 끝날 때 콜백</param>
    public virtual void Active(Transform attacker, Transform targetTr, int skillLevel, float attackSpeed, Action onSkillEffectFinish)
    {

    }

    /// <summary>
    /// 스킬 시전을 시도했을 때 시전이 가능한 상태인지 체크(ex) 돌진기인데 타겟과 시전자 사이에 충돌체가 있다. => false)
    /// </summary>
    /// <param name="attackerTr">시전자</param>
    /// <param name="targetTr">타겟 Transform</param>
    /// <param name="halfRadiusOfCollider">시전자의 충돌체 Collider(Trigger 충돌 아님)</param>
    /// <returns></returns>
    public virtual bool CanActive(Transform attackerTr, Transform targetTr, float halfRadiusOfCollider)
    {
        ISkillable skillable = attackerTr.GetComponent<ISkillable>();

        SkillTableCell tbCell = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithSkillName(skillName);
        SkillAndLevelData skillData = SkillManager.Instance.ManagedSkills.Find(n => n.skillCode == tbCell.Code);
        var statBonusData = skillLevelData.SkillLevelAndStatus.Find(n => n.key == skillData.level);
        if (statBonusData.value.requiredMana > skillable.CurrentMana)
            return false;

        return true;
    }
}
