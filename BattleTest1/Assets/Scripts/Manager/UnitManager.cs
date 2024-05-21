using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    StatManager statManager;
    Types.UnitCode unitCode;
    UnitItem unitItem;
    public UnitManager(Types.UnitCode unitCode)
    {
        this.unitCode = unitCode;
    }
    void Start()
    {
        statManager = transform.GetComponent<StatManager>();
        // statManager.setStats(Types.UnitCode.Lorien);
    }

    void Update()
    {
        
    }
}
