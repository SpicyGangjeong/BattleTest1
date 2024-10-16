using Assets.Scripts.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AMapController : MonoBehaviour
{
    public LayerMask tileStateMask;
    public Vector3 mapWorldSize;
    public float tileRadius;
    public static GameObject[,] tiles;
    public List<TileController> path;
    [SerializeField]
    public static GameObject unitContainer;

    void Start()
    {
        unitContainer = Instantiate(Resources.Load<GameObject>("Prefabs/BattleScene/UnitsContainer"));
        unitContainer.name = "UnitContainer";
        unitContainer.transform.SetParent(transform.parent, false);

        tiles = new GameObject[9, 10];
        for (int y = 0; y < transform.childCount / 9; y++) // 타일 배열 담기
        {
           for (int x = 0; x < transform.childCount / 10; x++)
            {
                tiles[x,y] = transform.GetChild(y * 9 + x).gameObject;
                TileController tileController = tiles[x, y].GetComponent<TileController>();
                tileController.setCoordinate(x, y);
                tileController.AMap = this;
                if (1 <= x && x <= 7 && 1 <= y  && y <= 4)
                {
                    tileController.setTileEnv(Types.TileState.Open, Types.TileType.Dry);
                    tileController.tileContainer = Types.TileContainer.UnitField;
                }
                else if (1 <= x && x <= 7 && 5 <= y && y <= 8)
                {
                    tileController.setTileEnv(Types.TileState.Open, Types.TileType.Dry);
                    tileController.tileContainer = Types.TileContainer.EnemyField;
                }
                else
                {
                    tileController.setTileEnv(Types.TileState.Block, Types.TileType.Dry);
                    tileController.tileContainer = Types.TileContainer.Wall;
                }
                
            }
        }
    }

    public List<ATile> getNeighbours(ATile tile)
    {
        List<ATile> neighbours = new List<ATile>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                tile.refTile.getCoordinate(out int checkX, out int checkY);
                if (checkY % 2 == 0 && (x == -1 && (y == -1 || y == 1)))
                {
                    continue;
                }
                else if (checkY % 2 == 1 && (x == 1 && (y == -1 || y == 1)))
                {
                    continue;
                }
                checkX += x;
                checkY += y;

                if (checkX >= 0 && checkX < tiles.GetLength(0) && checkY >= 0 && checkY < tiles.GetLength(1))
                {
                    TileController objTile = tiles[checkX, checkY].GetComponent<TileController>();
                    if (objTile.tileState == Types.TileState.Open || objTile.tileState == Types.TileState.Object)
                    {
                        neighbours.Add(new ATile(ref objTile));
                    }
                }
            }
        }
        return neighbours;
    }


}
