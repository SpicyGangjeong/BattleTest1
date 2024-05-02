using Assets.Scripts.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    Coordinate coordinate = new Coordinate();
    GameObject Halo;
    GameObject HaloCenter;
    public Types.TileState tileState;
    public Types.TileType tileType;
    public int coordi_X;
    public int coordi_Y;
    private void Start() 
    { 
        HaloCenter = transform.GetChild(0).gameObject;
        Halo = HaloCenter.transform.GetChild(0).gameObject;
        if (tileState == Types.TileState.Block)
        {
            transform.position = transform.position + new Vector3(0f, 1f, 0f);
            MaterialTools.SetTransparency(Halo.transform, false);
            MaterialTools.SetTransparency(HaloCenter.transform, false);
        }
        else
        {

        }

    }
    private void Update()
    {

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

    public static implicit operator Vector3(Tile v)
    {
        throw new NotImplementedException();
    }
}
