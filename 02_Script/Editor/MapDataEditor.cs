using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 에디터용 맵 제작 클래스
/// </summary>
[CustomEditor(typeof(MapEditorClass))]
public class MapDataEditor : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapEditorClass mapEditorClass = (MapEditorClass)target;

        if (GUILayout.Button("Make Map Data"))
        {
            mapEditorClass.GenerateMap();
        }
    }
#endif
}