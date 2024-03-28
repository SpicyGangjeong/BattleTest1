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
        Tiles = new GameObject[transform.childCount / 8, transform.childCount / 8];
        for (int y = 0; y < transform.childCount / 8; y++) // Ÿ�� �迭 ���
        {
           for (int x = 0; x < transform.childCount / 8; x++)
            {
                Tiles[x,y] = transform.GetChild(y * 8 + x).gameObject;
                ATile tileController = Tiles[x, y].GetComponent<ATile>();
                tileController.setCoordinate(x, y);
                if ( 1 <= x && x <=6 && y !=0 && y != 7 )
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
                if (x == -1 && (y == -1 || y == 1)) continue; // ����Ÿ���� �ǳʰ� �� ���� ��ġ ����
                if (x == 0 && y == 0) continue; // �ڱ� ��ġ ����

                tile.getCoordinate(out int checkX, out int checkY);
                checkX += x;
                checkY += y;

                if (Tiles[checkX, checkY].GetComponent<ATile>().tileState == Types.TileState.Open) // Ž���� Ÿ���� ���������� 
                {
                    neighbours.Add(Tiles[checkX, checkY].GetComponent<ATile>()); // �߰�
                }
            }
        }
        return neighbours;
    }
}
