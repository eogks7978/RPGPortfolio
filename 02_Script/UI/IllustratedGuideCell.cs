using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Illustrated Popup 안에 몬스터의 정보를 담을 UI
/// </summary>
public class IllustratedGuideCell : MonoBehaviour
{
    [SerializeField] private Transform monsterSpriteTr;
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private TMPro.TextMeshProUGUI hpText;
    [SerializeField] private TMPro.TextMeshProUGUI attackPointText;
    [SerializeField] private TMPro.TextMeshProUGUI defenseText;
    [SerializeField] private TMPro.TextMeshProUGUI moveSpeedText;
    [SerializeField] private TMPro.TextMeshProUGUI attackSpeedText;
    [SerializeField] private TMPro.TextMeshProUGUI expText;
    [SerializeField] private string activeLayerName;
    [SerializeField] private string inActiveLayerName;
    [SerializeField] private string unlockText;
    private Monster monster;
    private MonsterTableCell monsterTbCell;
    [SerializeField] private Vector3 monsterImageSacle;

    public void SetCell(MonsterTableCell tbCell)
    {
        GameObject obj = ObjectPoolManager.Instance.GetObjectInSubPool(tbCell.Prefab.name);

        if (obj != null)
        {
            monster = obj.GetComponent<Monster>();
            monsterTbCell = tbCell;

            if (monster != null)
            {
                monster.transform.SetParent(monsterSpriteTr);
                monster.transform.localPosition = Vector3.zero;
                monster.transform.localScale = monsterImageSacle;
                monster.SetAsUI(true);

                bool isUnLock = IllustratedGuideManager.Instance.IsUnLock(tbCell.Code);
                SetStatUI(monster, isUnLock);

                gameObject.SetActive(true);
            }
        }
    }

    public void ResetCell()
    {
        ObjectPoolManager.Instance.ReturnObjectToSubPool(monster.gameObject);
        monster.SetAsUI(false);
    }

    private void SetStatUI(Monster mosnter, bool isUnLock)
    {
        nameText.text = isUnLock ? monsterTbCell.MonsterName.ToString() : unlockText;
        hpText.text = isUnLock ? TranslateHandler.Translate(nameof(monster.MaxHealth)) + " " + monster.MaxHealth.ToString("F0") : unlockText;
        attackPointText.text = isUnLock ? TranslateHandler.Translate(nameof(monster.AttackPoint)) + " " + monster.AttackPoint.ToString("F0") : unlockText;
        defenseText.text = isUnLock ? TranslateHandler.Translate(nameof(monster.DefensePoint)) + " " + monster.DefensePoint.ToString("F0") : unlockText;
        moveSpeedText.text = isUnLock ? TranslateHandler.Translate(nameof(monster.MoveSpeed)) + " " + monster.MoveSpeed.ToString("F1") : unlockText;
        attackSpeedText.text = isUnLock ? TranslateHandler.Translate(nameof(monster.AttackSpeed)) + " " + monster.AttackSpeed.ToString("F1") : unlockText;
        expText.text = isUnLock ? TranslateHandler.Translate(nameof(monster.ContainedExp)) + " " + monster.ContainedExp.ToString("F0") : unlockText;
    }
}
