using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

/// <summary>
/// 게임의 메인 플레이어 클래스
/// </summary>
public class Player : Character, ISkillable
{
    [SerializeField, Tooltip("플레이어 이름")] private string playerName;
    [SerializeField, Tooltip("velocity 물리 이동을 위한 rigidBody")] private Rigidbody2D rigidBody;
    [SerializeField, Tooltip("메인 애니메이터")] private Animator animator;

    [SerializeField, Tooltip("플레이어 레벨")] private int level;
    [SerializeField, Tooltip("현재 보유 스킬 포인트")] private int skillPoint;
    [SerializeField, Tooltip("현재 남은 스킬 포인트")] private int statPoint;
    [SerializeField, Tooltip("레벨업 타겟 경험치")] private float maxExp;
    [SerializeField, Tooltip("현재 경험치")] private float currentExp;
    [SerializeField, Tooltip("마나 최대치")] private float maxMana;
    [SerializeField, Tooltip("현재 마나")] private float currentMana;

    [SerializeField, Tooltip("힘 스탯")] private int strength;
    [SerializeField, Tooltip("덱 스탯")] private int dexterity;
    [SerializeField, Tooltip("건강 스탯")] private int constitution;

    [SerializeField, Tooltip("무기 외형 포지션")] private Transform weaponTr;
    [SerializeField, Tooltip("무기 SpriteRenderer")] private SpriteRenderer weaponSpriteRenderer;
    [SerializeField, Tooltip("플레이어 이름 Tmp")] private TMPro.TextMeshProUGUI playerNameTmp;

    [SerializeField, Tooltip("플레이어의 보너스 스탯(장비, 버프)")] private PlayerStatData bonusStatData;
    [SerializeField, Tooltip("외형 데이터")] private PlayerAppearanceData appearanceData;

    [SerializeField] private SpriteRenderer hair;
    [SerializeField] private SpriteRenderer faceHair;
    [SerializeField] private SpriteRenderer leftEyeBack;
    [SerializeField] private SpriteRenderer rightEyeBack;
    [SerializeField] private SpriteRenderer leftEyeFront;
    [SerializeField] private SpriteRenderer rightEyeFront;
    [SerializeField] private List<SpriteRenderer> skins;

    [Tooltip("마지막으로 사용한 스킬의 테이블 코드")] private int recentlyUsedSkillCode;
    [Tooltip("공격모션 진행중인지 반환")] private bool isAttacking;

    #region Property
    public Rigidbody2D RB => rigidBody;
    public Animator Anim => animator;
    public PlayerAppearanceData AppearanceData => appearanceData;
    #region BaseStat
    public string PlayerName => playerName;
    public int Level => level;
    public float CurrentExp => currentExp;
    public float MaxExp => maxExp;
    public int SkillPoint => skillPoint;
    public int StatPoint => statPoint;
    public float BaseMaxHealth => maxHealth;
    public override float CurrentHealth => currentHealth;
    public float BaseMaxMana => maxMana;
    public float CurrentMana => currentMana;
    public float BaseDefensePoint => defensePoint;
    public float BaseAttackPoint => attackPoint;
    public float BaseAttackSpeed => attackSpeed;
    public float BaseMoveSpeed => moveSpeed;

    public int BaseStrength => strength;
    public int BaseDexterity => dexterity;
    public int BaseConstitution => constitution;
    #endregion

    #region TotalStat
    public override float MaxHealth => maxHealth + bonusStatData.MaxHealth;
    public float MaxMana => maxMana + bonusStatData.MaxMana;
    public override float DefensePoint => defensePoint + bonusStatData.DefensePoint;
    public override float AttackPoint => attackPoint + bonusStatData.AttackPoint;
    public override float AttackSpeed => attackSpeed + bonusStatData.AttackSpeed;
    public override float MoveSpeed => moveSpeed + bonusStatData.MoveSpeed;

    public float Strength => strength + bonusStatData.Strength;
    public float Dexterity => dexterity + bonusStatData.Dexterity;
    public float Constitution => constitution + bonusStatData.Constitution;

    public bool IsAttacking => isAttacking;
    public override bool IsDead => StateMachine.CurrentPlayerState == DeadState;

    public PlayerStatData BonusStatData => bonusStatData;
    #endregion
    #endregion

    #region StateMachine Variables
    public PlayerStateMachine StateMachine { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerMoveState MoveState { get; set; }
    public PlayerAttackState AttackState { get; set; }
    public PlayerDeadState DeadState { get; set; }
    #endregion

    private Coroutine moveCoroutine;
    private Coroutine attackCoroutine;

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        AttackState = new PlayerAttackState(this, StateMachine);
        DeadState = new PlayerDeadState(this, StateMachine);

        if (!animator)
            animator = GetComponent<Animator>();
        if (!rigidBody)
            rigidBody = GetComponent<Rigidbody2D>();
        if (!thisCollider)
            thisCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        StateMachine.CurrentPlayerState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentPlayerState.PhysicsUpdate();
    }

    private void OnEnable()
    {
        EventHandler.OnMonsterDead += GetExp;
        EventHandler.OnMonsterSelected += ChaseTarget;
        EventHandler.OnJoyStickDragEnd += StopMove;
    }

    private void OnDisable()
    {
        EventHandler.OnMonsterDead -= GetExp;
        EventHandler.OnMonsterSelected -= ChaseTarget;
        EventHandler.OnJoyStickDragEnd -= StopMove;
    }

    public void Initialize(PlayerData data)
    {
        InitializeStat(data);
        InitializePlayerUI();
        InitializeAppearance(data);

        isAttacking = false;
        StateMachine.Initialize(this, IdleState);
        Col.enabled = true;
    }

    private void InitializeStat(PlayerData data)
    {
        playerName = data.PlayerName;
        level = data.Level;
        skillPoint = data.SkillPoint;
        statPoint = data.StatPoint;
        maxExp = DataManager.Instance.TableDataTable.PlayerLevelUpTable.FindTableCellWithLevel(Level).MaxExperience;
        currentExp = data.CurrentExp;
        maxHealth = data.PlayerStatData.MaxHealth;
        currentHealth = data.PlayerStatData.CurrentHealth;
        maxMana = data.PlayerStatData.MaxMana;
        currentMana = data.PlayerStatData.CurrentMana;
        attackPoint = data.PlayerStatData.AttackPoint;
        defensePoint = data.PlayerStatData.DefensePoint;
        attackSpeed = data.PlayerStatData.AttackSpeed;
        moveSpeed = data.PlayerStatData.MoveSpeed;

        strength = data.PlayerStatData.Strength;
        dexterity = data.PlayerStatData.Dexterity;
        constitution = data.PlayerStatData.Constitution;
        appearanceData = data.PlayerAppearanceData;
    }

    private void InitializePlayerUI()
    {
        playerNameTmp.text = playerName;
        UIManager.Instance.PlayerHealthBar.Initailize(GetComponent<IDamagable>());
        UIManager.Instance.PlayerManaBar.Initailize(GetComponent<ISkillable>());
        UIManager.Instance.PlayerExpBar.Initailize(this);
        UIManager.Instance.PlayerLevelBar.Initailize(this);
    }

    private void InitializeAppearance(PlayerData data)
    {
        hair.sprite = DataManager.Instance.TableDataTable.CharacterAppearanceTable.
            FindHairTableCellWithCode(data.PlayerAppearanceData.HairCode).Sprite[0];
        hair.color = new Color(
            data.PlayerAppearanceData.HairColorR,
            data.PlayerAppearanceData.HairColorG,
            data.PlayerAppearanceData.HairColorB);

        faceHair.sprite = DataManager.Instance.TableDataTable.CharacterAppearanceTable.
            FindFacehairTableCellWithCode(data.PlayerAppearanceData.FacehairCode).Sprite[0];
        faceHair.color = new Color(
            data.PlayerAppearanceData.FacehairColorR,
            data.PlayerAppearanceData.FacehairColorG,
            data.PlayerAppearanceData.FacehairColorB);

        CharacterAppearanceTableCell eyesTbCell = DataManager.Instance.TableDataTable.CharacterAppearanceTable.
            FindEyesTableCellWithCode(data.PlayerAppearanceData.EyesCode);
        leftEyeBack.sprite = eyesTbCell.Sprite[0];
        rightEyeBack.sprite = eyesTbCell.Sprite[0];
        leftEyeFront.sprite = eyesTbCell.Sprite[1];
        rightEyeFront.sprite = eyesTbCell.Sprite[1];
        Color eyesColor = new Color(
            data.PlayerAppearanceData.EyesColorR,
            data.PlayerAppearanceData.EyesColorG,
            data.PlayerAppearanceData.EyesColorB);
        leftEyeFront.color = eyesColor;
        rightEyeFront.color = eyesColor;

        CharacterAppearanceTableCell skinTbCell = DataManager.Instance.TableDataTable.CharacterAppearanceTable.
            FindSkinTableCellWithCode(data.PlayerAppearanceData.SkinCode);
        for (int i = 0; i < skins.Count; ++i)
        {
            if (i < skinTbCell.Sprite.Count)
                skins[i].sprite = skinTbCell.Sprite[i];
            else
                break;
        }
    }

    #region ManageStatus
    public void GetExp(Monster monster)
    {
        SetStatus(new Dictionary<string, float> { { nameof(CurrentExp), monster.ContainedExp } });
    }

    private void LevelUp()
    {
        if (DataManager.Instance.TableDataTable.PlayerLevelUpTable.FindTableCellWithLevel(Level) == null)
        {
            UIManager.Instance.ShowSimpleNotification("최대 레벨입니다.", Color.yellow);
            return;
        }

        GameObject levelUpEffect = ObjectPoolManager.Instance.GetObjectInMainPool(
            ObjectManager.Instance.FindPoolInEtcMainObjectToPool(typeof(LevelUpEffect)).key.name);

        levelUpEffect.transform.SetParent(transform);
        levelUpEffect.transform.localPosition = Vector3.zero;

        level += 1;

        PlayerLevelUpData playerLevelUpData = DataManager.Instance.TableDataTable.PlayerLevelUpTable.FindTableCellWithLevel(Level);

        currentExp = 0f;
        maxExp = playerLevelUpData.MaxExperience;
        SetStatus(new Dictionary<string, float> { { nameof(SkillPoint), playerLevelUpData.SkillPoint } });
        SetStatus(new Dictionary<string, float> { { nameof(StatPoint), playerLevelUpData.StatPoint } });
        SetStatus(new Dictionary<string, float> { { nameof(MaxHealth), playerLevelUpData.MaxHealth } });
        SetStatus(new Dictionary<string, float> { { nameof(MaxMana), playerLevelUpData.MaxMana } });
        SetStatus(new Dictionary<string, float> { { nameof(AttackPoint), playerLevelUpData.AttackPoint } });
        SetStatus(new Dictionary<string, float> { { nameof(DefensePoint), playerLevelUpData.DefensePoint } });
        SetStatus(new Dictionary<string, float> { { nameof(AttackSpeed), playerLevelUpData.AttackSpeed } });
        SetStatus(new Dictionary<string, float> { { nameof(MoveSpeed), playerLevelUpData.MoveSpeed } });

        SetStatus(new Dictionary<string, float> { { nameof(CurrentHealth), MaxHealth } });
        SetStatus(new Dictionary<string, float> { { nameof(CurrentMana), MaxMana } });

        EventHandler.OnPlayerLevelUp?.Invoke();
    }

    /// <summary>
    /// 플레이어의 Original 스탯을 변경
    /// </summary>
    /// <param name="stats"></param>
    public override void SetStatus(Dictionary<string, float> stats)
    {
        if (IsDead)
            return;

        foreach (KeyValuePair<string, float> stat in stats)
        {
            switch (stat.Key)
            {
                case nameof(Strength):
                    strength += (int)stat.Value;
                    var valueByStrength = GetStatMagnificationByOriginStat(nameof(Strength));
                    foreach(var value in valueByStrength)
                    {
                        SetStatus(new Dictionary<string, float> { { value.Key, stat.Value * value.Value } });
                    }
                    break;
                case nameof(Dexterity):
                    dexterity += (int)stat.Value;
                    var valueByDexterity = GetStatMagnificationByOriginStat(nameof(Dexterity));
                    foreach (var value in valueByDexterity)
                    {
                        SetStatus(new Dictionary<string, float> { { value.Key, stat.Value * value.Value } });
                    }
                    break;
                case nameof(Constitution):
                    constitution += (int)stat.Value;

                    var valueByConstitution = GetStatMagnificationByOriginStat(nameof(Constitution));
                    foreach (var value in valueByConstitution)
                    {
                        SetStatus(new Dictionary<string, float> { { value.Key, stat.Value * value.Value } });
                    }
                    UIManager.Instance.PlayerHealthBar.UpdateSlider();
                    UIManager.Instance.PlayerManaBar.UpdateSlider();
                    break;
                case nameof(CurrentExp):
                    float resultExpValue = currentExp + stat.Value;
                    if (resultExpValue >= maxExp)
                    {
                        float beforeMaxExp = maxExp;
                        LevelUp();
                        SetStatus(new Dictionary<string, float>() { { nameof(CurrentExp), resultExpValue - beforeMaxExp } });
                    }
                    else if (resultExpValue < 0)
                    {
                        currentExp = 0f;
                    }
                    else
                    {
                        currentExp = resultExpValue;
                    }
                    UIManager.Instance.PlayerExpBar.UpdateSlider();
                    break;
                case nameof(SkillPoint):
                    skillPoint += (int)stat.Value;
                    break;
                case nameof(StatPoint):
                    statPoint += (int)stat.Value;
                    break;
                case nameof(MaxHealth):
                    maxHealth += stat.Value;
                    currentHealth += stat.Value;
                    break;
                case nameof(CurrentHealth):
                    float defensedValue = stat.Value + DefensePoint;

                    if (defensedValue > 0)
                        defensedValue = 0;

                    float valueByIntention = stat.Value < 0
                        ? defensedValue
                        : stat.Value;

                    float resultValue = currentHealth + valueByIntention;

                    resultValue = resultValue < 0 ? 0 : resultValue;
                    resultValue = resultValue > MaxHealth ? MaxHealth : resultValue;

                    currentHealth = (int)resultValue;
                    UIManager.Instance.PlayerHealthBar.UpdateSlider();

                    attackedDamageUI.SetDamage(
                    Mathf.Abs((int)valueByIntention), (int)valueByIntention <= 0
                    ? Color.red
                    : Color.green);

                    if (currentHealth <= 0)
                    {
                        Die();
                    }
                    break;
                case nameof(DefensePoint):
                    defensePoint += stat.Value;
                    break;
                case nameof(AttackPoint):
                    attackPoint += stat.Value;
                    break;
                case nameof(AttackSpeed):
                    attackSpeed += stat.Value;
                    break;
                case nameof(MaxMana):
                    maxMana += stat.Value;
                    currentMana += stat.Value;
                    break;
                case nameof(CurrentMana):
                    float resultValueMP = currentMana + stat.Value;

                    resultValueMP = resultValueMP < 0 ? 0 : resultValueMP;
                    resultValueMP = resultValueMP > MaxMana ? MaxMana : resultValueMP;

                    currentMana = (int)resultValueMP;
                    UIManager.Instance.PlayerManaBar.UpdateSlider();

                    break;
                case nameof(MoveSpeed):
                    moveSpeed += stat.Value;
                    animator.SetFloat("MoveSpeed", MoveSpeed);
                    break;
                default:
                    return;
            }
        }
        EventHandler.OnPlayerStatChanged?.Invoke();
    }

    /// <summary>
    /// 플레이어의 Bonus 스탯을 변경
    /// </summary>
    /// <param name="stats"></param>
    public void SetBonusStatus(Dictionary<string, float> stats)
    {
        if (IsDead)
            return;

        foreach (KeyValuePair<string, float> stat in stats)
        {
            switch (stat.Key)
            {
                case nameof(bonusStatData.Strength):
                    bonusStatData.Strength += (int)stat.Value;
                    var valueByStrength = GetStatMagnificationByOriginStat(nameof(bonusStatData.Strength));
                    foreach (var value in valueByStrength)
                    {
                        SetBonusStatus(new Dictionary<string, float> { { value.Key, stat.Value * value.Value } });
                    }
                    break;
                case nameof(bonusStatData.Dexterity):
                    bonusStatData.Dexterity += (int)stat.Value;
                    var valueByDexterity = GetStatMagnificationByOriginStat(nameof(bonusStatData.Dexterity));
                    foreach (var value in valueByDexterity)
                    {
                        SetBonusStatus(new Dictionary<string, float> { { value.Key, stat.Value * value.Value } });
                    }
                    break;
                case nameof(bonusStatData.Constitution):
                    bonusStatData.Constitution += (int)stat.Value;

                    var valueByConstitution = GetStatMagnificationByOriginStat(nameof(bonusStatData.Constitution));
                    foreach (var value in valueByConstitution)
                    {
                        SetBonusStatus(new Dictionary<string, float> { { value.Key, stat.Value * value.Value } });
                    }
                    UIManager.Instance.PlayerHealthBar.UpdateSlider();
                    UIManager.Instance.PlayerManaBar.UpdateSlider();
                    break;
                case nameof(bonusStatData.MaxHealth):
                    bonusStatData.MaxHealth += stat.Value;
                    currentHealth = currentHealth > MaxHealth
                        ? MaxHealth
                        : currentHealth;
                    UIManager.Instance.PlayerHealthBar.UpdateSlider();
                    break;
                case nameof(bonusStatData.DefensePoint):
                    bonusStatData.DefensePoint += stat.Value;
                    break;
                case nameof(bonusStatData.AttackPoint):
                    bonusStatData.AttackPoint += stat.Value;
                    break;
                case nameof(bonusStatData.AttackSpeed):
                    bonusStatData.AttackSpeed += stat.Value;
                    break;
                case nameof(bonusStatData.MaxMana):
                    bonusStatData.MaxMana += stat.Value;
                    currentMana = currentMana > MaxMana
                        ? MaxMana
                        : currentMana;
                    UIManager.Instance.PlayerManaBar.UpdateSlider();
                    break;
                case nameof(bonusStatData.MoveSpeed):
                    bonusStatData.MoveSpeed += stat.Value;
                    animator.SetFloat("MoveSpeed", MoveSpeed);
                    break;
                default:
                    return;
            }
        }
        EventHandler.OnPlayerStatChanged?.Invoke();
    }

    /// <summary>
    /// 필드를 string값으로 찾아서 반환
    /// </summary>
    /// <param name="fieldName">찾을 필드명</param>
    /// <returns></returns>
    public override object GetFieldValueByName(string fieldName)
    {
        PropertyInfo property = GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
        return property?.GetValue(this);
    }

    /// <summary>
    /// 스탯들이 1 오를때 마다 적용되는 능력치 반환
    /// </summary>
    /// <param name="statName"></param>
    /// <returns></returns>
    public static Dictionary<string, float> GetStatMagnificationByOriginStat(string statName)
    {
        switch(statName)
        {
            case nameof(Strength):
                return new Dictionary<string, float> { { nameof(AttackPoint), 1f } };
            case nameof(Dexterity):
                return new Dictionary<string, float> { { nameof(AttackSpeed), 0.01f } };
            case nameof(Constitution):
                return new Dictionary<string, float> {
                    { nameof(MaxHealth), 10f }, 
                    { nameof(DefensePoint), 0.4f} };
        }

        return null;
    }
    #endregion

    public override void Die()
    {
        float expDecrease = -currentExp * 0.1f;
        SetStatus(new Dictionary<string, float> { { nameof(CurrentExp), expDecrease } });
        StateMachine.ChangeState(DeadState);

        StartCoroutine(ShowPlayerDeadPopup(expDecrease));
    }

    public IEnumerator ShowPlayerDeadPopup(float expDecrease)
    {
        yield return new WaitForSeconds(2f);

        UIManager.Instance.OpenPlayerDeadPopup(expDecrease);
    }

    /// <summary>
    /// 플레이어 부활
    /// </summary>
    public void Resurrection()
    {
        MapData mapData = DataManager.LoadData<MapData>(
            DataManager.Instance.TableDataTable.MapTables.FindTableCellWithCode(1).Data.name);
        MapManager.Instance.ChangeMap(1, new Vector2(mapData.PlayerDefaultPositionX, mapData.PlayerDefaultPositionY));

        StateMachine.Initialize(this, IdleState);
        isAttacking = false;
        SetStatus(new Dictionary<string, float> { {nameof(CurrentHealth), MaxHealth} });
        SetStatus(new Dictionary<string, float> { { nameof(CurrentMana), MaxMana } });
        Col.enabled = true;
    }

    public void StartAttack()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        attackCoroutine = StartCoroutine(AttackCor());
    }

    public void StopAttack()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
    }

    /// <summary>
    /// 공격 체크 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCor()
    {
        while (true)
        {
            yield return new WaitUntil(() => SkillManager.Instance.AvailableSkills.Count != 0);

            if (StateMachine.CurrentPlayerState != AttackState)
                yield break;

            if (!InputManager.Instance.SelectedMonster)
            {
                StateMachine.ChangeState(IdleState);
                yield break;
            }

            if (InputManager.Instance.SelectedMonster.IsDead)
            {
                StateMachine.ChangeState(IdleState);
                yield break;
            }

            int skillCodeToUse = SkillManager.Instance.AvailableSkills[0].skillCode;

            Skill skillToUse = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithCode(skillCodeToUse)
                .Skill;

            if (skillToUse == null)
                yield break;

            if (!skillToUse.CanActive(transform, InputManager.Instance.SelectedMonster.transform, thisCollider.radius * .5f))
            {
                SkillManager.Instance.ConditionalSorting();
                continue;
            }

            SkillAndLevelData skillLevelData = SkillManager.Instance.ManagedSkills.Find(n => n.skillCode == skillCodeToUse);
            var statBonusData = skillToUse.SkillLevelData.SkillLevelAndStatus.Find(n => n.key == skillLevelData.level);

            if (statBonusData == null)
                yield break;

            if (Vector2.Distance(
                transform.position,
                InputManager.Instance.SelectedMonster.transform.position)
                <= statBonusData.value.ActiveRange)
            {
                CheckForLeftOrRightFacing(InputManager.Instance.SelectedMonster.transform.position - transform.position);

                if (statBonusData.value.requiredMana <= currentMana)
                    StartCoroutine(UseSkill(skillCodeToUse, skillToUse.AttackAnimationName, statBonusData));
            }
            else
            {
                StateMachine.ChangeState(MoveState);
                yield break;
            }

            SkillManager.Instance.ConditionalSorting();

            yield return new WaitUntil(() => !isAttacking);

            if (JoyStick.Instance.IsMoving)
                StateMachine.ChangeState(MoveState);

            yield return null;
        }
    }

    public IEnumerator UseSkill(int skillCode, string animationName, SerializableKeyValuePair<int, StatBonusData> statBonusData)
    {
        recentlyUsedSkillCode = skillCode;

        isAttacking = true;
        animator.SetFloat(nameof(AttackSpeed), AttackSpeed);
        animator.SetFloat(animationName, 1);
        animator.SetTrigger("Attack");

        yield return new WaitWhile(() => isAttacking);

        animator.SetFloat(animationName, 0);
    }

    #region MovementFunction
    /// <summary>
    /// InputManager에서 몬스터가 선택되면 실행.
    /// </summary>
    public void ChaseTarget()
    {
        if (SkillManager.Instance.SlotedSkills.Count == 0)
        {
            UIManager.Instance.ShowSimpleNotification("사용 할 스킬이 없습니다.", Color.red);
            return;
        }

        if (SkillManager.Instance.AvailableSkills.Count == 0)
            return;

        StartCoroutine(ChaseTargetCor());

        IEnumerator ChaseTargetCor()
        {
            yield return new WaitUntil(() => !isAttacking);

            if (!JoyStick.Instance.IsMoving)
            {
                StateMachine.ChangeState(MoveState);
            }
        }
    }

    public void Move(Vector2 velocity)
    {
        rigidBody.velocity = velocity * MoveSpeed;
        CheckForLeftOrRightFacing(velocity);
    }

    public void StartMove()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToTargetWithPathFinder());
    }

    public void StopMove()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        if (!isAttacking)
            StateMachine.ChangeState(IdleState);
    }

    /// <summary>
    /// InputManager의 선택된 몬스터를 PathFinder로 길을 찾아 이동
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveToTargetWithPathFinder()
    {
        Coroutine currentMoveCor = null;

        while (StateMachine.CurrentPlayerState == MoveState)
        {
            if (PathFinder.Instance.GenerateAstarPath(
                transform.position,
                PathFinder.Instance.GetClosestNode(InputManager.Instance.SelectedMonster.transform.position),
                out List<Vector2> path))
            {
                if (currentMoveCor != null)
                    StopCoroutine(currentMoveCor);
                currentMoveCor = StartCoroutine(MoveToTarget(path, MoveState));
            }

            yield return new WaitForSeconds(0.2f); // 지속호출 부하 방지
        }
    }

    /// <summary>
    /// PathFinder 기반 길찾기 이동.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="validState"></param>
    /// <returns></returns>
    IEnumerator MoveToTarget(List<Vector2> path, PlayerState validState)
    {
        animator.SetFloat("MoveSpeed", MoveSpeed);
        animator.SetBool("Run", true);

        while (StateMachine.CurrentPlayerState == validState && !JoyStick.Instance.IsMoving)
        {
            if (path.Count > 0)
            {
                if (SkillManager.Instance.AvailableSkills.Count != 0)
                {
                    if (InputManager.Instance.SelectedMonster.IsDead)
                    {
                        StateMachine.ChangeState(IdleState);
                        yield break;
                    }

                    CheckForLeftOrRightFacing(path[0] - (Vector2)transform.position);
                    Move((path[0] - (Vector2)transform.position).normalized);

                    SkillAndLevelData skillLevelInfo = SkillManager.Instance.AvailableSkills[0];

                    if (skillLevelInfo == null)
                        yield break;

                    Skill skillToUse = DataManager.Instance.TableDataTable.SkillTable.FindTableCellWithCode(
                        skillLevelInfo.skillCode).Skill;

                    if (skillToUse == null)
                        yield break;

                    SkillAndLevelData skillLevelData = SkillManager.Instance.ManagedSkills.Find(n => n.skillCode == skillLevelInfo.skillCode);
                    var statBonusData = skillToUse.SkillLevelData.SkillLevelAndStatus.Find(n => n.key == skillLevelData.level);

                    if (statBonusData == null)
                        yield break;

                    if (Vector2.Distance(transform.position,
                        InputManager.Instance.SelectedMonster.transform.position)
                        <= statBonusData.value.ActiveRange)
                        StateMachine.ChangeState(AttackState);

                    if (Vector2.Distance((Vector2)transform.position, path[0]) <= 0.1f)
                        path.RemoveAt(0);
                }
            }
            else
            {
                animator.SetBool("Run", false);
                yield break;
            }

            yield return null;
        }
    }
    #endregion

    /// <summary>
    /// 플레이어 장비 변경시 외형, 스탯 적용
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isEquip"></param>
    public void ChangePlayerEquipment(Item item, bool isEquip)
    {
        if (item == null)
            return;

        InventoryItemTableCell itemTbCell = DataManager.Instance.TableDataTable.ItemTable.FindTableCellWithCode(item.Code);
        EquipmentTableCell tbCell = itemTbCell as EquipmentTableCell;

        if (tbCell == null)
            return;

        EquipmentItem equipmentItem = item as EquipmentItem;
        Quaternion ratationValue = Quaternion.Euler(tbCell.OriginalRotation);
        var itemStatData = equipmentItem.GetEquipmentStat();

        int equipValue = isEquip ? 1 : -1;

        SetBonusStatus(new Dictionary<string, float> { 
            { nameof(MaxHealth), equipValue * Convert.ToSingle(itemStatData.GetType().GetField(nameof(MaxHealth)).GetValue(itemStatData)) },
            { nameof(MaxMana), equipValue * Convert.ToSingle(itemStatData.GetType().GetField(nameof(MaxMana)).GetValue(itemStatData)) },
            { nameof(AttackPoint), equipValue * Convert.ToSingle(itemStatData.GetType().GetField(nameof(AttackPoint)).GetValue(itemStatData)) },
            { nameof(DefensePoint), equipValue * Convert.ToSingle(itemStatData.GetType().GetField(nameof(DefensePoint)).GetValue(itemStatData)) },
            { nameof(AttackSpeed), equipValue * Convert.ToSingle(itemStatData.GetType().GetField(nameof(AttackSpeed)).GetValue(itemStatData)) },
            { nameof(MoveSpeed), equipValue * Convert.ToSingle(itemStatData.GetType().GetField(nameof(MoveSpeed)).GetValue(itemStatData)) },
            { nameof(Strength), equipValue * Convert.ToSingle(itemStatData.GetType().GetField(nameof(Strength)).GetValue(itemStatData)) },
            { nameof(Dexterity), equipValue * Convert.ToSingle(itemStatData.GetType().GetField(nameof(Dexterity)).GetValue(itemStatData)) },
            { nameof(Constitution), equipValue * Convert.ToSingle(itemStatData.GetType().GetField(nameof(Constitution)).GetValue(itemStatData)) }
        });     

        switch (tbCell.equipmentTag)
        {
            case EquipmentTableCell.EquipmentTag.Sword:
                if (isEquip)
                {
                    weaponSpriteRenderer.sprite = tbCell.ItemSprite;
                    weaponTr.transform.localRotation = ratationValue;
                }
                else
                {
                    weaponSpriteRenderer.sprite = null;
                }
                break;
            case EquipmentTableCell.EquipmentTag.Bow:
                if (isEquip)
                {
                    weaponSpriteRenderer.sprite = tbCell.ItemSprite;
                    weaponTr.transform.localRotation = ratationValue;
                }
                else
                {
                    weaponSpriteRenderer.sprite = null;
                }
                break;
            case EquipmentTableCell.EquipmentTag.Wand:
                if (isEquip)
                {
                    weaponSpriteRenderer.sprite = tbCell.ItemSprite;
                    weaponTr.transform.localRotation = ratationValue;
                }
                else
                {
                    weaponSpriteRenderer.sprite = null;
                }
                break;
            case EquipmentTableCell.EquipmentTag.Helmet:
                break;
            case EquipmentTableCell.EquipmentTag.Armor:
                break;
            case EquipmentTableCell.EquipmentTag.Pants:
                break;
            case EquipmentTableCell.EquipmentTag.Boots:
                break;
        }
    }

    #region AnimationTriggers
    protected void AttackPointOfMotion()
    {
        SkillAndLevelData skillLevelInfo = SkillManager.Instance.AvailableSkills.Find(n => n.skillCode == recentlyUsedSkillCode);

        if (skillLevelInfo == null)
            return;

        SkillManager.Instance.UseSkill(transform,
            recentlyUsedSkillCode,
            skillLevelInfo.level,
            InputManager.Instance.SelectedMonster.transform,
            AttackSpeed);
    }

    protected void AttackAnimDone()
    {
        isAttacking = false;
    }
    #endregion
}
