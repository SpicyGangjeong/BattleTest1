using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public TextAsset jsonData;
    public Stat unitStat;
    public void Start()
    {
        
    }
    public void setStats(Stat stat)
    {
        this.unitStat = stat;
    }
    public void starUp(int destStar)
    {
        while(destStar != unitStat.star)
        {
            unitStat.star++;
            unitStat.maxHp = (int)Math.Round(unitStat.maxHp * 1.8);
            unitStat.curHp = (int)Math.Round(unitStat.curHp * 1.8);
            unitStat.melee = (int)Math.Round(unitStat.melee * 1.5);
        }
    }
    public int getCost()
    {
        int cost = 0;
        switch (unitStat.rarity)
        {
            case Types.UnitRarity.Common:
                cost = 1 * (int)Math.Pow(3.0f, unitStat.star - 1);
                break;
            case Types.UnitRarity.Uncommon:
                cost = 2 * (int)Math.Pow(3.0f, unitStat.star - 1);
                break;
            case Types.UnitRarity.Rare:
                cost = 3 * (int)Math.Pow(3.0f, unitStat.star - 1);
                break;
            case Types.UnitRarity.Epic:
                cost = 4 * (int)Math.Pow(3.0f, unitStat.star - 1);
                break;
            case Types.UnitRarity.Legend:
                cost = 5 * (int)Math.Pow(3.0f, unitStat.star - 1);
                break;
            default:
                cost = -1;
                break;
        }
        return cost;
    }
    public void getDamaged()
    {

    }
    public void putGear()
    {

    }
}