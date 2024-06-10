using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    PathRequestManager requestManager;
    AMapController map;
    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        map = transform.parent.Find("Map").GetComponent<AMapController>();
    }

    //PathRequestManager에서의 현재 길찾기 요청을 시작하는 함수
    public void StartFindPath(TileController startTile, TileController targetTile)
    {
        StartCoroutine(FindPath(startTile, targetTile));
    }

    IEnumerator FindPath(TileController sTile, TileController tTile)
    {
        TileController[] waypoints = new TileController[0];
        List<ATile> openList = new List<ATile>();
        HashSet<ATile> closedList = new HashSet<ATile>();
        ATile startTile = new ATile(ref sTile);
        ATile endTile = new ATile(ref tTile);
        bool pathSuccess = false;
        openList.Add(startTile);

        while (openList.Count > 0) // 찾을 방이 남아있다면 반복
        {
            ATile currentTile = openList[0];
            // fCost = gCost + hCost
            // gCost = 그 위치까지 가는 실제 비용
            // hCost = 그 타일에서 도착지점까지의 거리
            // 열린목록에 F cost가 가장 작은 노드를 찾는다. 만약에 F cost가 같다면 H cost가 작은 노드를 찾는다.
            for (int i = 0; i < openList.Count; i++)
            {
                if (currentTile.fCost > openList[i].fCost ||
                    (currentTile.fCost == openList[i].fCost && currentTile.hCost > openList[i].hCost))
                {
                    currentTile = openList[i];
                }
            }
            // 탐색된 현재 가장 비용이 적게드는 노드는 열린목록에서 제거하고 끝난목록에 추가한다.
            openList.Remove(currentTile);
            closedList.Add(currentTile);
            // 탐색된 노드가 목표 노드라면 탐색 종료
            if (currentTile.refTile == endTile.refTile)
            {
                pathSuccess = true;
                endTile = currentTile;
                break;
            }
            // 탐색된 노드가 목표 노드가 아니라면 계속탐색(이웃 노드)
            foreach (ATile neighbour in map.getNeighbours(currentTile))
            {
                // 탐색이 끝난목록에 있는 경우는 스킵
                if (closedList.Contains(neighbour))
                    continue;

                float newMovementCostToNeighbour = currentTile.gCost + getDistanceCost(currentTile, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = getDistanceCost(neighbour, endTile);
                    neighbour.fCost = neighbour.gCost + neighbour.hCost;
                    neighbour.parentTile = currentTile;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
        yield return null;
        // 길찾기 종료
        if (pathSuccess) // 길 찾았으면
        {
            waypoints = RetracePath(startTile, endTile); // 경로를 추적
        }
        //노드들의 좌표를 담은 waypoints와 성공여부를 매니저함수에게 알려준다
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    // 탐색종료 후 최종 노드의 ParentTile를 추적하며 리스트에 담는다.
    // 최종 경로에 있는 노드들의 좌표를 순차적으로 담아 리턴
    TileController[] RetracePath(ATile startTile, ATile endTile)
    {
        List<TileController> path = new List<TileController>();
        ATile currentTile = endTile;
        currentTile = currentTile.parentTile;
        while (currentTile.refTile != startTile.refTile)
        {
            path.Add(currentTile.refTile);
            currentTile = currentTile.parentTile;
        }
        TileController[] waypoints = path.ToArray();
        Array.Reverse(waypoints);
        return waypoints;
    }

    // 두 노드간의 거리로 Cost를 계산하지만 대각선이동과 직선이동의 길이가 서로 같기 때문에 노드거리 고려 x
    // 오직 목표지점과의 거리를 중심으로 봄
    int getDistanceCost(ATile TileA, ATile TileB)
    {
        int Dist = (int)Mathf.Abs(Vector3.Distance(TileA.refTile.transform.position, TileB.refTile.transform.position));
        return Dist;
    }

}
