using Assets.Scripts.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFieldController : MonoBehaviour
{
    public static GameObject[] unitList;
    public static GameObject unitFieldController;
    GameManager gameManager = GameManager.getInstance();
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
                gameManager.isOnBattle = !gameManager.isOnBattle;
            }
        }
    }
    public static void PlaceUnit(GameObject itemObject, TileController tileController)
    {
        itemObject.transform.position = tileController.getLocation();
        itemObject.transform.parent = unitFieldController.transform;
        UpdateUnitList();
        tileController.placable = false;
    }
    public static void PopUnit(TileController tileController)
    {
        tileController.placable = true;
        UpdateUnitList();
        return;
    }
    public static void UpdateUnitList()
    {
        unitList = null;
        unitList = new GameObject[unitFieldController.transform.childCount];
        for (int i = 0; i < unitFieldController.transform.childCount; i++)
        {
            unitList[i] = unitFieldController.transform.GetChild(i).gameObject;
        }
    }
}
