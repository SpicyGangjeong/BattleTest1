
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    PathRequestManager requestManager;
    AMap map;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        map = transform.parent.Find("Map").GetComponent<AMap>();
    }

    //PathRequestManager에서의 현재 길찾기 요청을 시작하는 함수
    public void StartFindPath(ATile startTile, ATile targetTile)
    {
        StartCoroutine(FindPath(startTile, targetTile));
    }

    IEnumerator FindPath(ATile startTile, ATile targetTile)
    {
        ATile[] waypoints = new ATile[0]; // ?
        bool pathSuccess = false; // 길찾기 성공 했는지 담을 변수
        if (startTile.tileState == Types.TileState.Open && targetTile.tileState == Types.TileState.Open) // 시작타일이 열려있고 끝타일이 열려있다면 길찾기
        {
            List<ATile> openList = new List<ATile>();
            HashSet<ATile> closedList = new HashSet<ATile>();
            openList.Add(startTile);
            // TODO 정렬해서 넣은 순서대로 찾는게 아니라 h코스트대로 정렬하고 찾아야함
            while (openList.Count > 0) // 찾을 방이 남아있다면 반복
            {
                ATile currentTile = openList[0]; 
                // 현재 타일에서 갈 수 있는 노드와
                // 열린목록에 F cost가 가장 작은 노드를 찾는다. 만약에 F cost가 같다면 H cost가 작은 노드를 찾는다.
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].fCost < currentTile.fCost || openList[i].fCost == currentTile.fCost && openList[i].hCost < currentTile.hCost)
                    {
                        currentTile = openList[i]; // 즉 현재상태에서 가장 비용이 적은 노드부터 탐색한다는 말
                    }
                }
                // 탐색된 현재 가장 비용이 적게드는 노드는 열린목록에서 제거하고 끝난목록에 추가한다.
                openList.Remove(currentTile);
                closedList.Add(currentTile);

                // 탐색된 노드가 목표 노드라면 탐색 종료
                if (currentTile == targetTile)
                {
                    pathSuccess = true;
                    break;
                }

                // 탐색된 노드가 목표 노드가 아니라면 계속탐색(이웃 노드)
                foreach (ATile n in map.GetNeighbours(currentTile))
                {
                    // 이동불가 노드이거나 탐색이 끝난목록에 있는 경우는 스킵
                    if (n.tileState == Types.TileState.Block|| closedList.Contains(n)) continue;

                    // 이웃 노드들의 G cost와 H cost를 계산하여 열린목록에 추가한다.
                    int newCurrentToNeighbourCost = currentTile.gCost + GetDistanceCost(currentTile, n);
                    // 이웃노드들이 현재 노드보다 비용이 낮거나 오픈리스트에 든적이 없다면 
                    if (newCurrentToNeighbourCost < n.gCost || !openList.Contains(n)) 
                    {
                        n.gCost = newCurrentToNeighbourCost; // 이웃노드들의 G코스트 계싼 ( g코스트 - 그 노드까지 오는데의 비용 )
                        n.hCost = GetDistanceCost(n, targetTile); // 이웃노드들의 h코스트 계싼 ( h코스트 - 이동비용 )
                        n.parentTile = currentTile; // 이전 노드 기록

                        if (!openList.Contains(n)) // 다시한번 오픈리스트에 든적이 없는지 확인
                            openList.Add(n);
                    }
                }
            }
            // 길찾기 종료
        }
        yield return null;
        if (pathSuccess) // 길 찾았으면
        {
            waypoints = RetracePath(startTile, targetTile); // 경로를 추적
        }
        //노드들의 좌표를 담은 waypoints와 성공여부를 매니저함수에게 알려준다
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    // 탐색종료 후 최종 노드의 ParentTile를 추적하며 리스트에 담는다.
    // 최종 경로에 있는 노드들의 좌표를 순차적으로 담아 리턴
    ATile[] RetracePath(ATile startTile, ATile endTile)
    {
        List<ATile> path = new List<ATile>();
        ATile currentTile = endTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parentTile;
        }
        ATile[] waypoints = path.ToArray();
        Array.Reverse(waypoints);
        return waypoints;
    }

    // 두 노드간의 거리로 Cost를 계산하지만 대각선이동과 직선이동의 길이가 서로 같기 때문에 노드거리 고려 x
    // 오직 목표지점과의 거리를 중심으로 봄
    int GetDistanceCost(ATile TileA, ATile TileB)
    {
        int distX = (int)MathF.Abs(TileA.transform.position.x - TileB.transform.position.x);
        int distY = (int)MathF.Abs(TileA.transform.position.y - TileB.transform.position.y);

        if (distX > distY)
            return 10 * (distX - distY);
        return 10 * (distY - distX);
    }

}
