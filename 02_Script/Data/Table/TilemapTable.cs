using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "TilemapTable", menuName = "Scriptable Object/TilemapTable", order = int.MaxValue)]
public class TilemapTable : ScriptableObject
{
    public List<TileBase> TileLists;
    public List<string> TileListsString;

    public void Initialize()
    {
        if (TileLists.Count == 0) return;

        TileListsString.Clear();
        TileLists.RemoveAll(t => t == null);

        TileLists = TileLists.OrderBy(item => item.name).ToList();

        for (int i = 0; i < TileLists.Count; ++i)
        {
            TileListsString.Add(TileLists[i].name);
        }
    }

    public TileBase FindWithName(string tileName)
    {
        return TileLists[TileListsString.BinarySearch(tileName)];
    }
}