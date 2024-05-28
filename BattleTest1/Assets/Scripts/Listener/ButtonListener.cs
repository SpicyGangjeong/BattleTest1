using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListener : MonoBehaviour
{
    [SerializeField]
    public GameObject MenuPanel;
    public void onMenuButtonClicked()
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
    }

    public void onRerollButtonClicked()
    {
        
    }
    public static void onItemButtonClicked()
    {

    }
}
