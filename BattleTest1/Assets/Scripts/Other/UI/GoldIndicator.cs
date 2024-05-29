using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GoldIndicator : MonoBehaviour
{
    static Inventory inventory;
    private readonly string eventId = "I_Gold";
    static TextMeshProUGUI goldTMP;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.getInstance();
        goldTMP = transform.GetComponent<TextMeshProUGUI>();

        inventory.IndicateGold -= goldChangeEvent;
        inventory.IndicateGold += goldChangeEvent;
        goldTMP.SetText(inventory.Gold.ToString());
    }
    void goldChangeEvent(object sender, EventArgs e)
    {
        goldTMP.SetText(inventory.Gold.ToString());
    }
}
