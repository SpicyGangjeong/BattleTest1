using UnityEditor;
using UnityEngine;
public class UnitItem : Item
{
    int cost;
    Types.UnitFaction faction;
    Types.UnitJob job;

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
}