using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    AMap map;

    public GameObject StartObject;
    public GameObject TargetObject;

    void Awake()
    {
        map = transform.parent.Find("Map").GetComponent<AMap>();
    }
    private void Update()
    {
        FindPath(StartObject.GetComponent<ATile>(), TargetObject.GetComponent<ATile>());
    }
    void FindPath(ATile startTile, ATile targetTile)
    {
        List<ATile> openList = new List<ATile>();
        HashSet<ATile> closedList = new HashSet<ATile>();
        openList.Add(startTile);

        for (;openList.Count > 0;)
        {
            ATile currentTile = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {

            }
            openList.Remove(currentTile); // 큐.pop 써도 되지 않나, 리무브는 너무 오래걸리는데
            closedList.Add(currentTile);
            
            foreach(ATile n in map.GetNeighbours(currentTile))
            {
                if (n.tileState == Types.TileState.Block || closedList.Contains(n)) continue;
                int newCurrentToNeighbourCost = currentTile.gCost + GetDistanceCost(currentTile, n);
                if(newCurrentToNeighbourCost < n.gCost || !openList.Contains(n))
                {
                    n.gCost = newCurrentToNeighbourCost;
                    n.hCost = GetDistanceCost(n, targetTile);
                    n.parentTile = currentTile;

                    if (!openList.Contains(n))
                        openList.Add(n);
                }
            }
        }
    }
    void RetracePath(ATile startTile, ATile endTile)
    {
        List<ATile> path = new List<ATile>();
        ATile currentTile = endTile;
        for (; currentTile != startTile;)
        {
            path.Add(currentTile);
            currentTile = currentTile.parentTile;
        }
        path.Reverse();
        map.path = path;
    }

    private int GetDistanceCost(ATile currentTile, ATile n)
    {
        return 10;
    }
}
