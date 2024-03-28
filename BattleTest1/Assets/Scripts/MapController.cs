using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject[,] Tiles;
    void Start()
    {
        Tiles = new GameObject[transform.childCount / 8, transform.childCount / 8];
        for (int y = 0; y < transform.childCount / 8; y++) // 타일 배열 담기
        {
           for (int x = 0; x < transform.childCount / 8; x++)
            {
                Tiles[x,y] = transform.GetChild(y * 8 + x).gameObject;
                // Debug.Log("Tiles[" + x + "," + y + "]" + " = " + Tiles[x,y].name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
