using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sprite Animation을 사용하는 이펙트
/// </summary>
public class SpriteEffect : MonoBehaviour
{
    [SerializeField] protected Animator anim;

    private void Awake()
    {
        if (!anim)
            anim = GetComponent<Animator>();
    }

    protected void OnEnable()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        anim.Rebind();
    }

    public void EffectDone()
    {
        ObjectPoolManager.Instance.ReturnObjectToMainPool(gameObject);
    }
}
