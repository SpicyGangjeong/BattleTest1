using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item
{
    void setItem(Types.UnitFaction faction, Types.UnitJob job, int cost);
    int getCost();
    Types.UnitFaction getFaction();
    Types.UnitJob getJob();
    string getName();
    Stat getStat();
}
