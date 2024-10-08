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
        TileController objectiveTile = null;

        PriorityQueue<ATile, ATile> pq = new PriorityQueue<ATile, ATile> ();
        HashSet<ATile> closedList = new HashSet<ATile>();

        ATile startTile = new ATile(ref sTile, 0, 0);
        ATile endTile = new ATile(ref tTile);
        float smallest_fCost = float.MaxValue;
        bool pathSuccess = false;

        pq.Enqueue(startTile, startTile);
        closedList.Add(startTile);

        if (!startTile.Equals(endTile))
        {
            int LoopCount = 0;
            while (pq.Count > 0) // 찾을 방이 남아있다면 반복
            {
                ATile currentTile = pq.Dequeue();
                if (LoopCount > 100000)
                {
                    Debug.Log("Loop was broken by [Too Much Loop] in PathFinding");
                    break;
                }

                // 탐색된 노드가 목표 노드라면 탐색 종료
                if (currentTile.Equals(endTile))
                {

                    if (smallest_fCost > currentTile.fCost)
                    {
                        smallest_fCost = currentTile.fCost;
                        endTile = currentTile;
                    }
                    pathSuccess = true;
                }
                closedList.Add(currentTile);
                // 탐색된 노드가 목표 노드가 아니라면 계속탐색(이웃 노드)
                foreach (ATile neighbour in map.getNeighbours(currentTile))
                {
                    // 탐색이 끝난목록에 있는 경우는 스킵
                    if (closedList.Contains(neighbour))
                        continue;

                    float newMovementCost_ToNeighbour = currentTile.gCost + getDistanceCost(currentTile, neighbour);
                    if (newMovementCost_ToNeighbour < neighbour.gCost) // 새로운 코스트가 기존 코스트보다 낮으면? 넣고 아니면 스킵
                    {
                        neighbour.gCost = newMovementCost_ToNeighbour;
                        if (neighbour.hCost == 0)
                        {
                            neighbour.hCost = getDistanceCost(neighbour, endTile);
                        }
                        neighbour.fCost = neighbour.gCost + neighbour.hCost;
                        neighbour.parentTile = currentTile;

                        pq.Enqueue(neighbour, neighbour);
                    }
                }
            }
        }
        // 길찾기 종료
        if (pathSuccess) // 길 찾았으면
        {
            objectiveTile = RetracePath(startTile, endTile); // 경로를 추적
        }
        //노드들의 좌표를 담은 waypoints와 성공여부를 매니저함수에게 알려준다
        requestManager.FinishedProcessingPath(objectiveTile, pathSuccess);
        yield return null;
    }


    // 탐색종료 후 최종 노드의 ParentTile를 추적하며 리스트에 담는다.
    // 최종 경로에 있는 노드들의 좌표를 순차적으로 담아 리턴
    TileController RetracePath(ATile startTile, ATile endTile)
    {
        ATile currentTile = endTile.parentTile;
        ATile beforeTile = endTile;
        while (!currentTile.Equals(startTile))
        {
            beforeTile = currentTile;
            currentTile = currentTile.parentTile;
        }
        return beforeTile.refTile;
    }

    // 두 노드간의 거리로 Cost를 계산하지만 대각선이동과 직선이동의 길이가 서로 같기 때문에 노드거리 고려 x
    // 오직 목표지점과의 거리를 중심으로 봄
    int getDistanceCost(ATile TileA, ATile TileB)
    {
        int Dist = (int)Mathf.Abs(Vector3.Distance(TileA.refTile.transform.position, TileB.refTile.transform.position));
        return Dist;
    }

}
