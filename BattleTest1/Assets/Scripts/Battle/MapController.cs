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
                TileController tileController = Tiles[x, y].GetComponent<TileController>();
                tileController.setCoordinate(x, y);
                if ( 1 <= x && x <=6 && y !=0 && y != 7 )
                    tileController.setTileEnv(Types.TileState.Open, Types.TileType.Dry);
                else
                    tileController.setTileEnv(Types.TileState.Block, Types.TileType.Dry);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
