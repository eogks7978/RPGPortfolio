using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TableDataTable", menuName = "Scriptable Object/TableDataTable", order = int.MaxValue)]
public class TableDataTable : ScriptableObject
{
    public MapObjectTable MapObjectTables;
    public SoundTable SoundTables;
    public MapTable MapTables;
    public TilemapTable TilemapTable;
    public BackgroundTable BackgroundTable;
    public MonsterTable MonsterTable;
    public SkillTable SkillTable;
    public PlayerLevelUpDataTable PlayerLevelUpTable;
    public ItemTable ItemTable;
    public NPCTable NPCTable;
    public CharacterAppearanceTable CharacterAppearanceTable;
    public CraftingTable CraftingTable;
}
