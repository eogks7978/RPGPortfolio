using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevButton : MonoBehaviour
{
    [SerializeField] private Button thisButton;

    private void Start()
    {
        thisButton.onClick.AddListener(UIManager.Instance.OpenDevPannel);
    }
}
