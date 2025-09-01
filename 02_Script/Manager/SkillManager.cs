using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// 스킬 총괄 클래스
/// </summary>
public class SkillManager : MonoBehaviour
{
    #region singleton
    private static SkillManager instance;
    public static SkillManager Instance => instance;
    #endregion

    [SerializeField, Tooltip("관리하고 있는 모든 스킬")] private List<SkillAndLevelData> managedSkills;
    [SerializeField, Tooltip("슬롯에 장착된 스킬")] private List<SlotedSkillData> slotedSkills;
    [SerializeField, Tooltip("현재 사용 가능한 스킬")] private List<SkillAndLevelData> availableSkills;
    [SerializeField, Tooltip("쿨타임 중인 스킬")] private List<SkillAndLevelData> coolingDownSkill;
    [SerializeField, Header("최대 레벨이라고 알려주는 String")] private string defaultStringMaxLevel;
    [SerializeField, Header("스킬포인트가 부족하다고 알려주는 String")] private string defaultStringNotEnoughSkillPoint;

    public List<SkillAndLevelData> ManagedSkills => managedSkills;
    public List<SlotedSkillData> SlotedSkills => slotedSkills;
    public List<SkillAndLevelData> AvailableSkills => availableSkills;

    private void Awake()
    {
        //if (instance == null)
        //{
        instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    return;
        //}
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Initialize(SkillData data)
    {
        managedSkills = data.ManagedSkillLevelInfos;

        availableSkills = new();
        foreach (SlotedSkillData slotedData in data.SlotedSkillLevelInfos)
        {
            SkillAndLevelData skillLevelInfo = managedSkills.Find(n => n.skillCode == slotedData.SlotNumberAndSkill.value.skillCode);
            if (skillLevelInfo != null)
            {
                slotedSkills.Add(new(new(slotedData.SlotNumberAndSkill.key, skillLevelInfo)));
                availableSkills.Add(skillLevelInfo);
            }

            CreateSkillPool(slotedData.SlotNumberAndSkill.value.skillCode);
        }

        coolingDownSkill = new();

        ConditionalSorting();
    }

    public void UseSkill(Transform attacker, int skillCode, int skillLevel, Transform target, float attackSpeed)
    {
        GameObject skill = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithCode(skillCode)
            .Skill.gameObject;

        if (skill == null) return;

        Skill newSkill = ObjectPoolManager.Instance.GetObjectInMainPool(skill.name).GetComponent<Skill>();
        newSkill.transform.localScale = Vector3.one;
        newSkill.transform.position = target.position;
        newSkill.Active(attacker, target, skillLevel, attackSpeed, () =>
        {
            ObjectPoolManager.Instance.ReturnObjectToMainPool(newSkill.gameObject);
        });

        StartCoroutine(CoolDown(skillCode));
        UIManager.Instance.SkillActionBar.ShowCooldown(skillCode);
    }

    /// <summary>
    /// 사용 가능한 스킬 중 원하는 스킬을 먼저 사용하게 리스트 앞으로 옮김
    /// </summary>
    /// <param name="skillCode"></param>
    public void SetSkillAtFront(int skillCode)
    {
        SkillAndLevelData skillLevelInfo = availableSkills.Find(n => n.skillCode == skillCode);

        if (skillLevelInfo == null)
            return;

        availableSkills.RemoveAt(availableSkills.IndexOf(skillLevelInfo));
        availableSkills.Insert(0, skillLevelInfo);
    }

    /// <summary>
    /// 슬롯에 스킬 장착
    /// </summary>
    /// <param name="skillCode"></param>
    /// <param name="index"></param>
    public void AddSkillToSlot(int skillCode, int index)
    {
        SkillAndLevelData skillLevelInfo = managedSkills.Find(n => n.skillCode == skillCode);

        if (skillLevelInfo == null)
            return;

        slotedSkills.Add(new SlotedSkillData(new SerializableKeyValuePair<int, SkillAndLevelData>(index, skillLevelInfo)));

        if (!coolingDownSkill.Contains(skillLevelInfo))
        {
            availableSkills.Add(skillLevelInfo);

            CreateSkillPool(skillCode);
        }

        ConditionalSorting();
    }

    public void ChangeSkillIndexInAvailableSkill(int skillCode, int index)
    {
        SkillAndLevelData skillLevelInfo = availableSkills.Find(n => n.skillCode == skillCode);

        if (skillLevelInfo == null)
            return;

        int skillIndexToChange = availableSkills.IndexOf(skillLevelInfo);

        SkillAndLevelData skillLevelInfoToChange = availableSkills[index];
        availableSkills[index] = skillLevelInfo;
        availableSkills[skillIndexToChange] = skillLevelInfoToChange;
    }

    /// <summary>
    /// 스킬 슬롯 해제
    /// </summary>
    /// <param name="skillCode"></param>
    public void RemoveSkill(int skillCode)
    {
        slotedSkills.RemoveAll(n => n.SlotNumberAndSkill.value.skillCode == skillCode);
        availableSkills.RemoveAll(n => n.skillCode == skillCode);
        coolingDownSkill.RemoveAll(n => n.skillCode == skillCode);
    }

    public bool IsSkillAvailable(int skillCode)
    {
        return availableSkills.Find(n => n.skillCode == skillCode) != null;
    }

    public bool IsSkillSloted(int skillCode)
    {
        return slotedSkills.Find(n => n.SlotNumberAndSkill.value.skillCode == skillCode) != null;
    }

    private IEnumerator CoolDown(int skillCode)
    {
        Skill skill = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithCode(skillCode)
            .Skill;

        SkillAndLevelData info = availableSkills.Find(n => n.skillCode == skillCode);
        if (info == null)
            yield break;

        availableSkills.RemoveAll(n => n.skillCode == skillCode);
        coolingDownSkill.Add(info);

        var statBonusData = skill.SkillLevelData.SkillLevelAndStatus.Find(n => n.key == info.level);
        if (statBonusData == null)
            yield break;

        yield return new WaitForSeconds(statBonusData.value.Cooldown);

        // 쿨타임중에 스킬 슬롯에서 뺄 경우 대비
        if (coolingDownSkill.Contains(info))
        {
            coolingDownSkill.RemoveAll(n => n.skillCode == skillCode);
            availableSkills.Add(info);
        }
    }

    /// <summary>
    /// 스킬을 오브젝트풀에 인스턴스화
    /// </summary>
    /// <param name="skillCode"></param>
    private void CreateSkillPool(int skillCode)
    {
        GameObject skill = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithCode(skillCode)
            .Skill.gameObject;

        if (skill != null)
        {
            if (!ObjectPoolManager.Instance.ContainsMainPool(skill.name))
            {
                ObjectPoolManager.Instance.AddMainPool(new List<ObjectPool>()
                { new ObjectPool(skill, 5, 10, true) });
            }
        }
    }

    /// <summary>
    /// 높은 등급의 스킬을 가장 먼저 사용하게 배치
    /// </summary>
    public void ConditionalSorting()
    {
        availableSkills = availableSkills.OrderByDescending(n =>
        {
            SkillTableCell tbCell = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithCode(n.skillCode);
            int grade = tbCell.Skill.Grade;

            bool isMonsterSelected = InputManager.Instance.SelectedMonster == null 
            ? false 
            : tbCell.Skill.CanActive(
                PlaySceneMain.Instance.Player.transform, 
                InputManager.Instance.SelectedMonster.transform,
                PlaySceneMain.Instance.Player.Col.radius * .5f);

            // 우선순위: CanActive가 true일 때 우선, 그 후에 Grade 값으로 내림차순 정렬
            return (isMonsterSelected ? 1 : 0) * 1000 + grade;
        }).ToList();
    }

    public void LevelUpSkill(int skillCode)
    {
        // 스킬포인트가 부족하면 Return
        if (PlaySceneMain.Instance.Player.SkillPoint <= 0)
        {
            UIManager.Instance.OpenNotificationPopup(defaultStringNotEnoughSkillPoint);
            return;
        }

        SkillAndLevelData skillInfo = managedSkills.Find(n => n.skillCode == skillCode);
        Skill skill = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithCode(skillCode)
            .Skill;

        if (skillInfo != null)
        {
            if (skillInfo.level < skill.SkillLevelData.maxSkillLevel)
            {
                PlaySceneMain.Instance.Player.SetStatus(new Dictionary<string, float> { { nameof(PlaySceneMain.Instance.Player.SkillPoint), -1 } });
                skillInfo.level++;
                EventHandler.OnSkillLevelUp?.Invoke();
            }
            else
            {
                UIManager.Instance.OpenNotificationPopup(defaultStringMaxLevel);
            }
        }
    }

    public int GetSkillLevel(int skillCode)
    {
        SkillAndLevelData info = managedSkills.Find(n => n.skillCode == skillCode);

        return info != null ? info.level : 0;
    }
}