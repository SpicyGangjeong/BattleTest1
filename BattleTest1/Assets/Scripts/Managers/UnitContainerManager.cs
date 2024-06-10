using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitContainerManager : MonoBehaviour
{
    public GameObject unitBench;
    public GameObject unitField;
    public GameObject enemyField;
    void Start()
    {
        unitBench = transform.Find("UnitBench").gameObject;
        unitField = transform.Find("UnitField").gameObject;
        enemyField = transform.Find("EnemyField").gameObject;
    }

    void Update()
    {
        
    }
}
