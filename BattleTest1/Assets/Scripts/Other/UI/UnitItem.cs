﻿using UnityEditor;
using UnityEngine;
public class UnitItem : Item
{
    TextAsset jsonData = Resources.Load<TextAsset>("Prefabs/BattleScene/unitStat");
    Stat unitStat;
    int cost;
    Types.UnitFaction faction;
    Types.UnitJob job;
    Types.UnitCode unitCode;
    public UnitItem(int unitCode)
    {
        unitStat = JsonHandler.getStat(jsonData, unitCode);
        switch (unitStat.rarity)
        {
            case Types.UnitRarity.Common:
                cost = 1;
                break;
            case Types.UnitRarity.Uncommon:
                cost = 2;
                break;
            case Types.UnitRarity.Rare:
                cost = 3;
                break;
            case Types.UnitRarity.Epic:
                cost = 4;
                break;
            case Types.UnitRarity.Legend:
                cost = 5;
                break;
            default:
                cost = -1;
                break;
        }
        setItem(unitStat.faction, unitStat.job, cost);
    }
    public void setItem(Types.UnitFaction faction, Types.UnitJob job, int cost)
    {
        this.faction = faction;
        this.job = job;
        this.cost = cost;
    }
    int Item.getCost()
    {
        return cost;
    }

    Types.UnitFaction Item.getFaction()
    {
        return faction;
    }

    Types.UnitJob Item.getJob()
    {
        return job;
    }
    public string getName()
    {
        return unitStat.unitName;
    }
    public Stat getStat()
    {
        return unitStat;
    }
    public GameObject loadItem()
    {
        // TODO 유닛 이름이든 코드든 써서 prefab에 해당하는 객체로 인스탄시에이트 해야함
        GameObject itemObject = Resources.Load<GameObject>("Prefabs/BattleScene/DummyUnit");
        return itemObject;
    }
}