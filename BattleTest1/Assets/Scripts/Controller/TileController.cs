using Assets.Scripts.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    Coordinate coordinate = new Coordinate();
    GameObject halo;
    GameObject haloCenter;
    public AMapController AMap;
    public static bool isTileStarted;
    public Types.TileState tileState;
    public Types.TileType tileType;
    public int coordi_X;
    public int coordi_Y;
    public bool placable;
    public Types.TileContainer tileContainer;
    public int tileIndex;
    private void Start() 
    {
        placable = true;
        haloCenter = transform.GetChild(0).gameObject;
        halo = haloCenter.transform.GetChild(0).gameObject;
        if (tileState == Types.TileState.Block)
        {
            transform.position = transform.position + new Vector3(0f, 1f, 0f);
            AlterHaloAndCenterVisible(false);
            placable = false;
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
    public Vector3 getLocation()
    {
        Vector3 vector3 = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        return vector3;
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
    public void AlterHaloAndCenterVisible(bool value)
    {
        MaterialTools.SetTransparency(halo.transform, value);
        MaterialTools.SetTransparency(haloCenter.transform, value);
    }
    //public override bool Equals(object obj)
    //{
    //    if (obj == this) return true;
    //    if (obj is null || obj.GetType() != this.GetType()) return false;
    //    TileController other = (TileController)obj;
    //    getCoordinate(out int this_X, out int this_Y);
    //    other.getCoordinate(out int other_X, out int other_Y);
    //    return this_X == other_X && this_Y == other_Y;
    //}
}
