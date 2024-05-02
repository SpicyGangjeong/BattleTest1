using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListener : MonoBehaviour
{
    [SerializeField]
    public GameObject MenuPanel;
    public void OnMenuButtonClicked()
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
    }

    public void OnRerollButtonClicked()
    {

    }
    public void OnItemButtonClicked()
    {

        this.OnRerollButtonClicked();
    }
}
