using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UITools
{
    public static void AlterVisible(GameObject go)
    {
        if (go == null) return;
        go.SetActive(!go.activeSelf);
    }
}