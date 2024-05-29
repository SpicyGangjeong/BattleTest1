using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    StatManager statManager;
    Types.UnitCode unitCode;
    UnitItem unitItem;
    void Start()
    {
        
        
    }

    void Update()
    {

    }
    public void setInitialStatus(UnitItem unitItem)
    {
        statManager = transform.GetComponent<StatManager>();
        this.unitItem = unitItem;
        statManager.setStats(unitItem.getStat());
    }
}
