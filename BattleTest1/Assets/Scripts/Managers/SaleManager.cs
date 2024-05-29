using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaleManager : MonoBehaviour
{
    static GameObject thisObject;
    static TextMeshProUGUI goldTMP;
    static Inventory inventory;
    void Start()
    {
        inventory = Inventory.getInstance();
        thisObject = gameObject;
        UITools.AlterVisible(gameObject);
        goldTMP = GetComponentsInChildren<TextMeshProUGUI>()[1];
    }
    void Update()
    {
        
    }
    public static void AlterVisible()
    {
        UITools.AlterVisible(thisObject);
    }
    public static void AlterValue(int value)
    {
        goldTMP.SetText(value.ToString());
    }
    public static void sellUnit(GameObject unitItem, int value)
    {
        inventory.Gold += value;
        Destroy(unitItem);
    }
}
