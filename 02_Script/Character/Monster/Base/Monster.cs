using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.Rendering;

public class Monster : Character, ITriggerCheckable
{
    [SerializeField, Tooltip("공격 타겟 Tag")] protected string targetTag;
    [SerializeField, Tooltip("Idle 상태일 때 랜덤 배회 주기")] protected float wanderMoveDelay;
    [SerializeField, Tooltip("사망시 경험치 지급 수치")] protected int containedExp;
    [SerializeField, Tooltip("스폰으로부터 랜덤 배회 거리")] protected float RandomMovementRange = 3f;
    [SerializeField, Tooltip("패스파인더 길 표시 유무(에디터)")] protected bool drawDebugLines;
    [SerializeField, Tooltip("공격 효과음")] protected AudioClip attackSound;
    [SerializeField] protected Animator animator;

    [SerializeField, Tooltip("플레이어를 감지하는 Collider")] private GameObject playerDetectCollider;
    [SerializeField, Tooltip("체력바")] private UIHealthBar healthBar;
    [SerializeField, Tooltip("미니맵에 표시될 오브젝트")] private Transform minimapIconTr;
    [SerializeField] private Transform healthSliderTr;
    [SerializeField, Tooltip("몬스터 클릭을 위한 콜라이더")] private BoxCollider2D BoxCollider;

    protected bool isAggroed;
    protected bool isWithinStrikingDistance;

    // 공격빈도
    protected float attackDelayTime;

    // --------공격 이동 관련 데이터들--------
    protected bool isAttackMostionOn;
    protected Transform targetTr;
    protected Vector2 targetPos;
    protected Vector2 originSpawnPos;

    protected Coroutine attackCoroutine;
    protected IEnumerator attackDelayCoroutine;
    protected Coroutine transformMoveCoroutine;
    protected Coroutine vectorMoveCoroutine;
    // -------------------------------------

    #region Property
    public Animator Anim => animator;
    public GameObject PlayerDetectCollider => playerDetectCollider;
    public UIHealthBar HealthBar => healthBar;
    public Transform MinimapIconTr => minimapIconTr;
    public Transform HealthSliderTr => healthSliderTr;

    public bool IsAggroed => isAggroed;
    public bool IsWithinStrikingDistance => isWithinStrikingDistance;
    public bool IsAttacking => isAttackMostionOn;

    public int ContainedExp => containedExp;

    public string TargetTag => targetTag;
    public Vector2 OriginSpawnPos => originSpawnPos;

    public override bool IsDead => StateMachine.CurrentMonsterState == DeadState;
    #endregion

    #region StateMachine Variables
    public MonsterStateMachine StateMachine { get; set; }
    public MonsterIdleState IdleState { get; set; }
    public MonsterChaseState ChaseState { get; set; }
    public MonsterAttackState AttackState { get; set; }
    public MonsterBackToSpawnState BackToSpawnState { get; set; }
    public MonsterDeadState DeadState { get; set; }
    #endregion

    protected virtual void Awake()
    {
        StateMachine = new MonsterStateMachine();

        IdleState = new MonsterIdleState(this, StateMachine);
        ChaseState = new MonsterChaseState(this, StateMachine);
        AttackState = new MonsterAttackState(this, StateMachine);
        BackToSpawnState = new MonsterBackToSpawnState(this, StateMachine);
        DeadState = new MonsterDeadState(this, StateMachine);

        if (animator == null)
            animator = GetComponent<Animator>();
        if (BoxCollider == null)
            BoxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        StateMachine.CurrentMonsterState.FrameUpdate();
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.CurrentMonsterState.PhysicsUpdate();
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
        playerDetectCollider.SetActive(false);
        healthSliderTr.gameObject.SetActive(false);
        minimapIconTr.gameObject.SetActive(false);
        attackedDamageUI.gameObject.SetActive(false);
    }

    public virtual void Spawn(Vector2 spawnPos)
    {
        originSpawnPos = spawnPos;
        transform.position = spawnPos;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        animationRoot.rotation = Quaternion.identity;
        playerDetectCollider.SetActive(true);
        healthSliderTr.gameObject.SetActive(true);
        minimapIconTr.gameObject.SetActive(true);
        attackedDamageUI.gameObject.SetActive(true);

        isFacingLeft = true;
        isAggroed = false;
        isWithinStrikingDistance = false;
        isAttackMostionOn = false;
        attackDelayTime = 0f;
        currentHealth = maxHealth;

        healthBar.Initailize(GetComponent<Character>());
        StateMachine.Initialize(this, IdleState);

        SetCollider(true);
        SetPlayerDetectCollider(true);
    }

    #region ManageStatus
    public override void SetStatus(Dictionary<string, float> stats)
    {
        if (IsDead)
            return;

        foreach (KeyValuePair<string, float> stat in stats)
        {
            switch (stat.Key)
            {
                case nameof(MaxHealth):
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

                    healthBar.UpdateSlider();
                    attackedDamageUI.SetDamage(-(int)valueByIntention, Color.gray);

                    if (currentHealth <= 0)
                    {
                        Die();
                    }
                    break;
                case nameof(DefensePoint):
                    break;
                case nameof(AttackPoint):
                    break;
                case nameof(AttackSpeed):
                    break;
                case nameof(MoveSpeed):
                    animator.SetFloat("MoveSpeed", MoveSpeed);
                    break;
                default:
                    return;
            }
        }
    }

    /// <summary>
    /// string으로 필드값 찾기
    /// </summary>
    /// <param name="fieldName">찾을 필드 이름</param>
    /// <returns></returns>
    public override object GetFieldValueByName(string fieldName)
    {
        PropertyInfo property = GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
        return property?.GetValue(this);
    }

    public virtual void Attack()
    {
        if (attackDelayTime > 0f)
            animator.SetBool("Idle", true);

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(AttackCor());
    }

    public virtual void StopAttack()
    {
        StopCoroutine(attackCoroutine);
    }

    protected virtual IEnumerator AttackCor()
    {
        while (true)
        {
            if (attackDelayCoroutine != null)
                StopCoroutine(attackDelayCoroutine);
            attackDelayCoroutine = AttackDelayCor();
            yield return StartCoroutine(attackDelayCoroutine);

            isAttackMostionOn = true;
            CheckForLeftOrRightFacing(targetTr.position - transform.position);
            animator.SetFloat("AttackSpeed", AttackSpeed);
            animator.SetFloat("Attack_Sword", 1);
            animator.SetTrigger("Attack");
            attackDelayTime = 1f / AttackSpeed;
        }
    }

    /// <summary>
    /// 공격 빈도마다 공격
    /// </summary>
    /// <returns></returns>
    protected IEnumerator AttackDelayCor()
    {
        while (attackDelayTime > 0f)
        {
            attackDelayTime -= Time.deltaTime;
            yield return null;
        }
    }

    public override void Die()
    {
        StateMachine.ChangeState(DeadState);
        EventHandler.OnMonsterDie(this);
    }
    #endregion

    #region MovementFunction
    public void SetTarget(Transform targetTr)
    {
        this.targetTr = targetTr;
    }

    public void StartTransformMove()
    {
        if (transformMoveCoroutine != null)
            StopCoroutine(transformMoveCoroutine);
        transformMoveCoroutine = StartCoroutine(MoveToTransformWithPathFinder());
    }

    public void StartPositionMove(IEnumerator coroutine)
    {
        if (vectorMoveCoroutine != null)
            StopCoroutine(vectorMoveCoroutine);
        vectorMoveCoroutine = StartCoroutine(coroutine);
    }

    public void StopTransformMove()
    {
        if (transformMoveCoroutine != null)
            StopCoroutine(transformMoveCoroutine);
    }

    public void StopPositionMove()
    {
        if (vectorMoveCoroutine != null)
            StopCoroutine(vectorMoveCoroutine);
    }

    /// <summary>
    /// Transform으로 이동
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveToTransformWithPathFinder()
    {
        Coroutine currentMoveCor = null;

        while (StateMachine.CurrentMonsterState == ChaseState)
        {
            if (PathFinder.Instance.GenerateAstarPath(
                transform.position,
                PathFinder.Instance.GetClosestNode(targetTr.position),
                out List<Vector2> path))
            {
                if (currentMoveCor != null)
                    StopCoroutine(currentMoveCor);
                currentMoveCor = StartCoroutine(MoveToPosition(path, ChaseState));
            }
            else
                StateMachine.ChangeState(BackToSpawnState);
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// 배회 이동
    /// </summary>
    /// <returns></returns>
    public IEnumerator WanderingMove()
    {
        while (StateMachine.CurrentMonsterState == IdleState)
        {
            yield return new WaitForSeconds(wanderMoveDelay);

            if (PathFinder.Instance.GenerateAstarPath(
                transform.position,
                PathFinder.Instance.GetClosestNode(GetRandomPointInCircle()),
                out List<Vector2> path))
            {
                if (path.Count <= 30)
                    yield return StartCoroutine(MoveToPosition(path, IdleState));
            }
        }
    }

    /// <summary>
    /// 스폰 위치로 복귀 이동
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveToSpawnPos()
    {
        if (PathFinder.Instance.GenerateAstarPath(
            transform.position,
            PathFinder.Instance.GetClosestNode(OriginSpawnPos),
            out List<Vector2> path))
        {
            yield return StartCoroutine(MoveToPosition(path, BackToSpawnState));
            if (StateMachine.CurrentMonsterState == BackToSpawnState)
                StateMachine.ChangeState(IdleState);
        }
    }

    /// <summary>
    /// 목표 Vector2로 이동
    /// </summary>
    /// <param name="path">패스파인더로 찾은 길</param>
    /// <param name="validState">타겟 상태. 이 상태가 아니면 코루틴이 종료됨</param>
    /// <returns></returns>
    IEnumerator MoveToPosition(List<Vector2> path, MonsterState validState)
    {
        animator.SetFloat("MoveSpeed", MoveSpeed);
        animator.SetBool("Run", true);

        while (StateMachine.CurrentMonsterState == validState)
        {
            if (path.Count > 0) //if the target is not yet reached
            {
                transform.position = Vector2.MoveTowards(transform.position, path[0], MoveSpeed * Time.fixedDeltaTime);
                CheckForLeftOrRightFacing(path[0] - (Vector2)transform.position);

                if ((Vector2)transform.position == path[0])
                {
                    transform.position = path[0];
                    path.RemoveAt(0);
                }
            }
            else
            {
                animator.SetBool("Run", false);
                yield break;
            }
#if UNITY_EDITOR
            if (drawDebugLines)
            {
                for (int i = 0; i < path.Count - 1; i++) //visualize your path in the sceneview
                {
                    Debug.DrawLine(path[i], path[i + 1]);
                }
            }
#endif
            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// 범위 안의 임의의 지점 반환
    /// </summary>
    /// <returns>범위 안의 임의의 지점</returns>
    protected Vector2 GetRandomPointInCircle()
    {
        return originSpawnPos + UnityEngine.Random.insideUnitCircle * RandomMovementRange;
    }

    /// <summary>
    /// 플레이어 감지 Collider 세팅
    /// </summary>
    /// <param name="b">SetActive(b)</param>
    public void SetPlayerDetectCollider(bool b)
    {
        playerDetectCollider.SetActive(b);
    }

    /// <summary>
    /// 몬스터 클릭을 가능하게 할지 세팅
    /// </summary>
    /// <param name="b">col.enable(b)</param>
    public void SetCollider(bool b)
    {
        BoxCollider.enabled = b;
    }
    #endregion

    /// <summary>
    /// 2d 오브젝트를 UI로서 사용하게 하는 세팅. 도감, 튜토리얼, 다이얼로그 등에 사용 가능. 
    /// </summary>
    /// <param name="isOn"></param>
    public override void SetAsUI(bool isOn)
    {
        GetComponent<Monster>().enabled = !isOn;
        base.SetAsUI(isOn);
    }

    #region Distance Checks
    public void SetAggroStatus(bool isAggroed)
    {
        this.isAggroed = isAggroed;
    }

    public void SetStrikingDistanceBool(bool isWithinStrikingDistance)
    {
        this.isWithinStrikingDistance = isWithinStrikingDistance;
    }
    #endregion

    #region AnimationEvent
    private void PlayAttackSound()
    {
        if (attackSound)
            SoundController.Instance.PlaySfx(attackSound);
    }

    protected void StrikeTarget()
    {
        IDamagable target = targetTr.GetComponent<IDamagable>();
        if (target != null)
        {
            target.SetStatus(new Dictionary<string, float> { { nameof(target.CurrentHealth), -AttackPoint } });
        }
    }

    protected void AttackAnimDone()
    {
        isAttackMostionOn = false;
    }

    protected void MonsterDeadAnimDone()
    {

    }
    #endregion
}

