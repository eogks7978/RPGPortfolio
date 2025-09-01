using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIPanel : MonoBehaviour
{
    [Header("[Close]")]
    [SerializeField] protected Button[] closeButtons;

    protected UnityAction<object> _cbClose = null;
    protected object _results = null;

    protected float _backupCameraHeightScale = 1f;

    protected virtual void Awake()
    {
        foreach (var button in closeButtons)
        {
            button.onClick.AddListener(OnClickClose);
        }
    }

    //// 미리 한번 열었다가 닫음으로 처음 오픈될 때 렉을 없앤다.
    public virtual void Preload()
    {
        Close();
    }

    public virtual void Open(UnityAction<object> cbClose = null)
    {
        gameObject.SetActive(true);

        _cbClose = cbClose;
        _results = null;
    }

    public virtual void Close()
    {
        if (!isActiveAndEnabled)
            return;

        if (_cbClose != null)
            _cbClose(_results);

        gameObject.SetActive(false);
    }

    protected void OnClickClose()
    {
        Close();
    }
}

