using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListener : MonoBehaviour
{
    [SerializeField]
    public GameObject MenuPanel;
    public void onMenuButtonClicked()
    {
        UITools.AlterVisible(MenuPanel);
    }

    public void onRerollButtonClicked()
    {
        
    }
    public static void onItemButtonClicked()
    {

    }
}
