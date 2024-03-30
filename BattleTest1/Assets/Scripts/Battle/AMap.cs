using Assets.Scripts.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMap : MonoBehaviour
{
    public LayerMask tileStateMask;
    public Vector3 mapWorldSize;
    public float tileRadius;
    public GameObject[,] Tiles;
    public List<ATile> path;

    void Start()
    {
        Tiles = new GameObject[9, 10];
        for (int y = 0; y < transform.childCount / 9; y++) // 타일 배열 담기
        {
           for (int x = 0; x < transform.childCount / 10; x++)
            {
                Tiles[x,y] = transform.GetChild(y * 9 + x).gameObject;
                ATile tileController = Tiles[x, y].GetComponent<ATile>();
                tileController.setCoordinate(x, y);
                if ( 1 <= x && x <=7 && y !=0 && y != 9 )
                    tileController.setTileEnv(Types.TileState.Open, Types.TileType.Dry);
                else
                    tileController.setTileEnv(Types.TileState.Block, Types.TileType.Dry);
            }
        }
    }

    public List<ATile> GetNeighbours(ATile tile)
    {
        List<ATile> neighbours = new List<ATile>();
        for (int x = -1; x <= 1; x++) // 주변타일 탐색
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // 자기 위치 제외

                tile.getCoordinate(out int checkX, out int checkY);
                if (checkY % 2 == 0 && (x == -1 && (y == -1 || y == 1))) // 육각타일은 건너갈 수 없는 위치 제외
                {
                    continue;
                }
                else if (checkY % 2 == 1 && (x == 1 && (y == -1 || y == 1)))
                {
                    continue;
                }
                checkX += x;
                checkY += y;

                if (Tiles[checkX, checkY].GetComponent<ATile>().tileState == Types.TileState.Open) // 탐색한 타일이 열려있으면 
                {
                    neighbours.Add(Tiles[checkX, checkY].GetComponent<ATile>()); // 추가
                }
            }
        }
        string str = "";
        for (int i= 0; i < neighbours.Count; i++)
        {
            str += neighbours[i];
        }
        Debug.Log(tile + " " + str);
        return neighbours;
    }
}
