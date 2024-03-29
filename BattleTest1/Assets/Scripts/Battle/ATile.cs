using Assets.Scripts.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATile : MonoBehaviour
{
    Coordinate coordinate = new Coordinate();
    GameObject Halo;
    GameObject HaloCenter;
    public Types.TileState tileState;
    public Types.TileType tileType;
    public int gCost;
    public int hCost;
    public ATile parentTile;
    public int coordi_X;
    public int coordi_Y;
    public int fCost
    {
        get { return gCost + hCost; }
    }

    private void Start() 
    { 
        HaloCenter = transform.GetChild(0).gameObject;
        Halo = HaloCenter.transform.GetChild(0).gameObject;
        if (tileState == Types.TileState.Block)
        {
            transform.position = new Vector3(0f, 1f, 0f) + transform.position;
            MaterialTools.SetTransparency(Halo.transform, false);
            MaterialTools.SetTransparency(HaloCenter.transform, false);
        }
        else
        {

        }
    }
    private void Update()
    {
        coordi_X = coordinate.X;
        coordi_Y = coordinate.Y;
    }
    public void setTileEnv(Types.TileState TileState, Types.TileType TileType)
    {
        tileState = TileState;
        tileType = TileType;
    }

    public void setCoordinate(int x, int y)
    {
        coordinate.X = x;
        coordinate.Y = y;
    }
    public void getCoordinate(out int x, out int y)
    {
        x = coordinate.X;
        y = coordinate.Y;
    }
}
