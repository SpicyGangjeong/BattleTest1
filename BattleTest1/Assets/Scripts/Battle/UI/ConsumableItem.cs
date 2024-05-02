using UnityEditor;
using UnityEngine;

public class ConsumableItem : Item
{
    int cost;
    Types.UnitFaction faction;
    Types.UnitJob job;

    public void setItem(Types.UnitFaction faction, Types.UnitJob job, int cost)
    {
        throw new System.NotImplementedException();
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