using UnityEditor;
using UnityEngine;


public class RelicItem : Item
{

    int cost;
    Types.UnitFaction faction;
    Types.UnitJob job;
    public RelicItem(int itemId)
    {

    }
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