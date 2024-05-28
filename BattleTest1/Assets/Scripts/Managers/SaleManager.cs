using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaleManager : MonoBehaviour
{
    static GameObject thisObject;
    static TextMeshProUGUI goldTMP;
    void Start()
    {
        thisObject = gameObject;
        goldTMP = GetComponentsInChildren<TextMeshProUGUI>()[1];
        AlterValue(39);
    }
    void Update()
    {
        
    }
    public static void AlterVisible()
    {
        thisObject.SetActive(!thisObject.activeSelf);
    }
    public static void AlterValue(int value)
    {
        goldTMP.text = value.ToString();
    }
}
