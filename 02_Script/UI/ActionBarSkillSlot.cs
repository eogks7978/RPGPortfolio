using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// 스킬 퀵슬롯 버튼
/// </summary>
public class ActionBarSkillSlot : MonoBehaviour
{
    [SerializeField] private Image skillImage;
    [SerializeField] private Image coolDownMask;
    [SerializeField] private Skill containedSkill;
    [SerializeField] private int containedSkillCode;
    [SerializeField] private Button thisButton;
    private Coroutine cooldownCoroutine;

    public int ContainedSkillCode => containedSkillCode;

    public Image SkillImage => skillImage;

    public Skill ContainedSkill => containedSkill;

    private void Awake()
    {
        thisButton.onClick.AddListener(OnSkillUsed);
    }

    /// <summary>
    /// 스킬 장착
    /// </summary>
    /// <param name="skillCode">스킬 테이블 코드</param>
    public void SetSkill(int skillCode)
    {
        Skill skill = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithCode(skillCode)
            .Skill;

        if (skill != null)
        {
            UnsetSkill();

            skillImage.enabled = true;
            containedSkill = skill;
            containedSkillCode = skillCode;
            skillImage.sprite = skill.Icon;
        }
    }

    /// <summary>
    /// 스킬 해제
    /// </summary>
    public void UnsetSkill()
    {
        SkillManager.Instance.RemoveSkill(containedSkillCode);

        skillImage.enabled = false;
        containedSkill = null;
        containedSkillCode = 0;
    }

    /// <summary>
    /// 스킬 퀵슬롯 클릭
    /// </summary>
    public void OnSkillUsed()
    {
        // 슬롯에 장착된 스킬을 사용할 스킬 리스트의 맨 앞으로 이동

        if (!containedSkill)
            return;

        if (!SkillManager.Instance.IsSkillAvailable(containedSkillCode))
            return;

        SkillManager.Instance.SetSkillAtFront(containedSkillCode);

        // 플레이어가 이미 타겟을 공격중이 아닐 때
        if (PlaySceneMain.Instance.Player.StateMachine.CurrentPlayerState != PlaySceneMain.Instance.Player.AttackState)
        {
            // 죽지 않은 몬스터 식별
            List<Monster> targetMonsters = MapManager.Instance.CurrentMap.ManagedMonsters.FindAll(
                n => !n.IsDead);

            if (targetMonsters == null || targetMonsters.Count == 0)
                return;

            // 가장 가까운 몬스터 탐색
            Monster nearMonster = targetMonsters.OrderBy(
                    n => Vector2.Distance(
                        n.transform.position, PlaySceneMain.Instance.Player.transform.position)).First();

            // 탐색된 가장 가까운 몬스터의 거리가 설정된 거리보다 가까울 경우 타겟으로 지정, 이동
            if (nearMonster && Vector2.Distance(nearMonster.transform.position, PlaySceneMain.Instance.Player.transform.position) < 5f)
            {
                InputManager.Instance.SetSelectedMonster(nearMonster);
                if (!JoyStick.Instance.IsMoving)
                    PlaySceneMain.Instance.Player.StateMachine.ChangeState(PlaySceneMain.Instance.Player.MoveState);
            }
        }
    }

    public void ShowCooldown()
    {
        if (cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);

        cooldownCoroutine = StartCoroutine(ShowCooldownCor());
    }

    private IEnumerator ShowCooldownCor()
    {
        SkillAndLevelData skillLevelInfo = SkillManager.Instance.ManagedSkills.Find(n => n.skillCode == ContainedSkillCode);

        if (skillLevelInfo == null)
            yield break;

        var skillStatInfo = containedSkill.SkillLevelData.SkillLevelAndStatus.Find(n => n.key == skillLevelInfo.level);

        if (skillStatInfo == null)
            yield break;

        float coolTimeMax = skillStatInfo.value.Cooldown;
        float currentTime = coolTimeMax;

        while (currentTime > 0.0f)
        {
            currentTime -= Time.deltaTime;

            coolDownMask.fillAmount = currentTime / coolTimeMax;

            yield return null;
        }
    }
}
