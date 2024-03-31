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
    public List<Tile> path;

    void Start()
    {
        Tiles = new GameObject[9, 10];
        for (int y = 0; y < transform.childCount / 9; y++) // Ÿ�� �迭 ���
        {
           for (int x = 0; x < transform.childCount / 10; x++)
            {
                Tiles[x,y] = transform.GetChild(y * 9 + x).gameObject;
                Tile tileController = Tiles[x, y].GetComponent<Tile>();
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
        for (int x = -1; x <= 1; x++) // �ֺ�Ÿ�� Ž��
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // �ڱ� ��ġ ����

                tile.refTile.getCoordinate(out int checkX, out int checkY);
                if (checkY % 2 == 0 && (x == -1 && (y == -1 || y == 1))) // ����Ÿ���� �ǳʰ� �� ���� ��ġ ����
                {
                    continue;
                }
                else if (checkY % 2 == 1 && (x == 1 && (y == -1 || y == 1)))
                {
                    continue;
                }
                checkX += x;
                checkY += y;

                if (Tiles[checkX, checkY].GetComponent<Tile>().tileState == Types.TileState.Open) // Ž���� Ÿ���� ���������� 
                {
                    Tile objTile = Tiles[checkX, checkY].GetComponent<Tile>();
                    neighbours.Add(new ATile(ref objTile)); // �߰�
                }
            }
        }
        return neighbours;
    }

}
