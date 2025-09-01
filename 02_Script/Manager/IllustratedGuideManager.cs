using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 도감 관리 클래스
/// </summary>
public class IllustratedGuideManager : MonoBehaviour
{
    #region singleton
    private static IllustratedGuideManager instance;
    public static IllustratedGuideManager Instance => instance;
    #endregion

    private IllustratedGuideData illustratedGuideData;
    public IllustratedGuideData IllustratedGuideData => illustratedGuideData;

    private void OnEnable()
    {
        EventHandler.OnMonsterDead += OnMonsterDie;
    }

    private void OnDisable()
    {
        EventHandler.OnMonsterDead -= OnMonsterDie;
    }

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

    public void Initialize(IllustratedGuideData illustratedGuideData)
    {
        this.illustratedGuideData = illustratedGuideData;
    }

    public bool IsUnLock(int code)
    {
        var data = illustratedGuideData.MonsterCodeAndIsUnlock[code];

        if (data == null)
            return false;

        return data.value;
    }

    /// <summary>
    /// 몬스터가 죽으면 이벤트로 받아서 도감 해제
    /// </summary>
    /// <param name="monster"></param>
    private void OnMonsterDie(Monster monster)
    {
        MonsterTableCell tbCell = DataManager.Instance.TableDataTable.MonsterTable.FindTableCellWithName(monster.name);

        if (tbCell == null) 
            return;

        var data = illustratedGuideData.MonsterCodeAndIsUnlock[tbCell.Code];
        if (!data.value)
            data.value = true;
    }
}
