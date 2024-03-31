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

    //PathRequestManager������ ���� ��ã�� ��û�� �����ϴ� �Լ�
    public void StartFindPath(Tile startTile, Tile targetTile)
    {
        StartCoroutine(FindPath(startTile, targetTile));
    }

    IEnumerator FindPath(Tile sTile, Tile tTile)
    {
        Tile[] waypoints = new Tile[0];
        List<ATile> openList = new List<ATile>();
        HashSet<ATile> closedList = new HashSet<ATile>();
        ATile startTile = new ATile(ref sTile);
        ATile endTile = new ATile(ref tTile);
        bool pathSuccess = false;
        openList.Add(startTile);
        if (sTile.tileState == Types.TileState.Open && tTile.tileState == Types.TileState.Open)
        {
            while (openList.Count > 0) // ã�� ���� �����ִٸ� �ݺ�
            {
                ATile currentTile = openList[0];
                // fCost = gCost + hCost
                // gCost = �� ��ġ���� ���� ���� ���
                // hCost = �� Ÿ�Ͽ��� �������������� �Ÿ�
                // ������Ͽ� F cost�� ���� ���� ��带 ã�´�. ���࿡ F cost�� ���ٸ� H cost�� ���� ��带 ã�´�.
                for (int i = 0; i < openList.Count; i++)
                {
                    if (currentTile.fCost > openList[i].fCost ||
                        (currentTile.fCost == openList[i].fCost && currentTile.hCost > openList[i].hCost))
                    {
                        currentTile = openList[i];
                    }
                }
                // Ž���� ���� ���� ����� ���Ե�� ���� ������Ͽ��� �����ϰ� ������Ͽ� �߰��Ѵ�.
                openList.Remove(currentTile);
                closedList.Add(currentTile);
                // Ž���� ��尡 ��ǥ ����� Ž�� ����
                if (currentTile.refTile == endTile.refTile)
                {
                    pathSuccess = true;
                    endTile = currentTile;
                    break;
                }
                openList.Sort((ATile a, ATile b) => a.fCost.CompareTo(b.fCost));

                // Ž���� ��尡 ��ǥ ��尡 �ƴ϶�� ���Ž��(�̿� ���)
                foreach (ATile n in map.GetNeighbours(currentTile))
                {
                    // Ž���� ������Ͽ� �ִ� ���� ��ŵ
                    if (closedList.Contains(n)) continue;
                    n.gCost = currentTile.gCost + GetDistanceCost(currentTile, n);
                    n.hCost = GetDistanceCost(n, endTile);
                    n.fCost = n.hCost + n.gCost;
                    n.parentTile = currentTile;
                    openList.Add(n);
                }
            }
        }
        yield return null;
        // ��ã�� ����
        if (pathSuccess) // �� ã������
        {
            waypoints = RetracePath(startTile, endTile); // ��θ� ����
        }
        //������ ��ǥ�� ���� waypoints�� �������θ� �Ŵ����Լ����� �˷��ش�
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    // Ž������ �� ���� ����� ParentTile�� �����ϸ� ����Ʈ�� ��´�.
    // ���� ��ο� �ִ� ������ ��ǥ�� ���������� ��� ����
    Tile[] RetracePath(ATile startTile, ATile endTile)
    {
        List<Tile> path = new List<Tile>();
        ATile currentTile = endTile;

        while (currentTile.refTile != startTile.refTile)
        {
            path.Add(currentTile.refTile);
            currentTile = currentTile.parentTile;
        }
        Tile[] waypoints = path.ToArray();
        Array.Reverse(waypoints);
        return waypoints;
    }

    // �� ��尣�� �Ÿ��� Cost�� ��������� �밢���̵��� �����̵��� ���̰� ���� ���� ������ ���Ÿ� ��� x
    // ���� ��ǥ�������� �Ÿ��� �߽����� ��
    int GetDistanceCost(ATile TileA, ATile TileB)
    {
        int Dist = (int)Mathf.Abs(Vector3.Distance(TileA.refTile.transform.position, TileB.refTile.transform.position));
        return Dist;
    }

}
