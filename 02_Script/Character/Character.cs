using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

public class Character : MonoBehaviour, IDamagable, IMovable
{
    [SerializeField, Range(0, float.MaxValue)] protected float maxHealth;
    [SerializeField, Range(0, float.MaxValue)] protected float currentHealth;
    [SerializeField, Range(0, float.MaxValue)] protected float defensePoint;
    [SerializeField, Range(0, float.MaxValue)] protected float attackPoint;
    [SerializeField, Range(1f, 10f)] protected float attackSpeed;
    [SerializeField, Range(0f, 5f)] protected float moveSpeed;

    [SerializeField] protected Transform animationRoot;
    [SerializeField] protected CharacterAttackedUI attackedDamageUI;
    [SerializeField, Header("물리 이동 캐릭터에만 적용")] protected CircleCollider2D thisCollider;
    [SerializeField] protected SortingGroup sortingGroup;

    [SerializeField] protected bool isFacingLeft;
    protected bool isDead;

    #region Property
    public CharacterAttackedUI AttackedDamageUI => attackedDamageUI;
    public SortingGroup SortingGroup => sortingGroup;

    public virtual float MaxHealth => maxHealth;
    public virtual float CurrentHealth => currentHealth;
    public virtual float DefensePoint => defensePoint;
    public virtual float AttackPoint => attackPoint;
    public virtual float AttackSpeed => attackSpeed;
    public virtual float MoveSpeed => moveSpeed;

    public CircleCollider2D Col => thisCollider;

    public bool IsFacingLeft => isFacingLeft;
    public virtual bool IsDead => isDead;
    #endregion

    public virtual void SetStatus(Dictionary<string, float> stats)
    {
        foreach (KeyValuePair<string, float> stat in stats)
        {
            switch (stat.Key)
            {
                case nameof(MaxHealth):
                    break;
                case nameof(currentHealth):
                    currentHealth -= stat.Value;

                    attackedDamageUI.SetDamage(stat.Value, Color.red);

                    if (CurrentHealth < 0)
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
                default:
                    return;
            }
        }
    }

    public virtual object GetFieldValueByName(string fieldName)
    {
        PropertyInfo property = GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
        return property?.GetValue(this);
    }

    public virtual void Die()
    {

    }

    public virtual void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (isFacingLeft && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(animationRoot.transform.rotation.x, 180f, animationRoot.transform.rotation.z);
            animationRoot.transform.rotation = Quaternion.Euler(rotator);
            isFacingLeft = !isFacingLeft;
        }
        else if (!isFacingLeft && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(animationRoot.transform.rotation.x, 0f, animationRoot.transform.rotation.z);
            animationRoot.transform.rotation = Quaternion.Euler(rotator);
            isFacingLeft = !isFacingLeft;
        }
    }

    public virtual void SetAsUI(bool isOn)
    {
        if (sortingGroup)
            sortingGroup.sortingLayerName = isOn ? "UI" : "Default";

        if (thisCollider)
            thisCollider.enabled = isOn ? false : true;

        GetComponent<Character>().enabled = !isOn;
    }
}
