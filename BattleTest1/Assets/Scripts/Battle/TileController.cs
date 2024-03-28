using Assets.Scripts.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    Coordinate coordinate = new Coordinate();
    GameObject Halo;
    GameObject HaloCenter;
    Types.TileState tileState;
    Types.TileType tileType;

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
    public Tuple<int,int> getCoordinate()
    {
        return coordinate.getBoth();
    }
}
