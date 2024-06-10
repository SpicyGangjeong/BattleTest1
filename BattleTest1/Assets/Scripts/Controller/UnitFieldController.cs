using Assets.Scripts.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFieldController : MonoBehaviour
{
    public static GameObject[] unitList;
    public static GameObject unitFieldController;
    void Start()
    {
        unitFieldController = gameObject;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                UnitController unitController = transform.GetChild(i).GetComponent<UnitController>();
                unitController.isOnBattlePhase = !unitController.isOnBattlePhase;
            }
        }
    }
    public static void PlaceUnit(GameObject itemObject, TileController tileController)
    {
        itemObject.transform.position = tileController.getLocation();
        itemObject.transform.parent = unitFieldController.transform;
        tileController.placable = false;
    }
    public static void PopUnit(TileController tileController)
    {
        tileController.placable = true;
        return;
    }
    public void UpdateUnitList()
    {
        unitList = null;
        unitList = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            unitList[i] = transform.GetChild(i).gameObject;
        }
    }
}
