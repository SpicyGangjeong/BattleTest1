using Assets.Scripts.Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFieldController : MonoBehaviour
{
    public static GameObject unitFieldController;
    void Start()
    {
        unitFieldController = gameObject;
    }
    void Update()
    {
        
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
}
