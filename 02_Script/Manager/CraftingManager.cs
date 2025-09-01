using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : Singleton<CraftingManager>
{
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        UIManager.Instance.CraftingPopup.Initialize();
    }
}
