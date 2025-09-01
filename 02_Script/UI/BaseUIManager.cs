using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseUIManager : MonoBehaviour
{
    // 열려있는 패널 리스트
    [SerializeField] protected List<UIPanel> _opened = new List<UIPanel>();

    public int OpenedPannelCount { get { return _opened.Count; } }

    /// <summary>
    /// 열려있는 모든 패널을 닫는다.
    /// </summary>
    public void CloseAll()
    {
        for (int i = _opened.Count - 1; i >= 0; i--)
        {
            _opened[i].Close();
        }
    }

    /// <summary>
    /// 열려있는 모든 패널을 닫고 콜백 실행한다.
    /// </summary>
    public void CloseAll(Action callback)
    {
        for (int i = _opened.Count - 1; i >= 0; i--)
        {
            _opened[i].Close();
        }
        callback?.Invoke();
    }

    /// <summary>
    /// 열려있는 마지막 패널을 닫는다.
    /// </summary>
    /// <returns></returns>
    public bool CloseLastOpenedPanel()
    {
        if (_opened.Count > 0)
        {
            _opened[_opened.Count - 1].Close();
            return true;
        }
        return false;
    }

    protected void AddPanel(UIPanel panel)
    {
        _opened.Add(panel);
    }

    protected void RemovePanel(UIPanel panel)
    {
        _opened.Remove(panel);
    }

}
