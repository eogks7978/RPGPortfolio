using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 도감 데이터
/// </summary>
[Serializable]
public class IllustratedGuideData
{
    [Header("몬스터 테이블 코드와 도감 해제 유무")] public List<SerializableKeyValuePair<int, bool>> MonsterCodeAndIsUnlock;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="monsterCodeAndIsUnLock">몬스터 테이블 코드와 도감 해제 유무</param>
    public IllustratedGuideData(List<SerializableKeyValuePair<int, bool>> monsterCodeAndIsUnLock)
    {
        MonsterCodeAndIsUnlock = monsterCodeAndIsUnLock;
    }
}
