using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat: MonoBehaviour
{
    [SerializeField]
    #region Status
    public Types.UnitType unitType { get; }
    public Types.UnitJob job { get; }
    public Types.UnitFaction faction { get; }
    public int star { get; set; }
    public string name { get; set; }
    public int maxHp { get; set; }
    public int curHp { get; set; }
    public int maxMp { get; set; }
    public int curMp { get; set; }
    public int melee { get; set; }
    public int sorcery { get; set; }
    public int attackRange { get; set; }
    public int skillRange { get; set; }
    public int meleeGuard { get; set; }
    public int sorceryGuard { get; set; }
    public int rarity { get; set; }
    public int criticalChance { get; set; }
    public int criticalMagnification { get; set; }
    public float meleeSpeed { get; set; }
    public float moveSpeed { get; set; }

    #endregion Status

    public Stat(Types.UnitType type)
    {
        switch (type)
        {

            default:
                break;
        }
    }
}