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

    //PathRequestManager������ ���� ��ã�� ��û�� �����ϴ� �Լ�
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
            while (pq.Count > 0) // ã�� ���� �����ִٸ� �ݺ�
            {
                ATile currentTile = pq.Dequeue();
                if (LoopCount > 100000)
                {
                    Debug.Log("Loop was broken by [Too Much Loop] in PathFinding");
                    break;
                }

                // Ž���� ��尡 ��ǥ ����� Ž�� ����
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
                // Ž���� ��尡 ��ǥ ��尡 �ƴ϶�� ���Ž��(�̿� ���)
                foreach (ATile neighbour in map.getNeighbours(currentTile))
                {
                    // Ž���� ������Ͽ� �ִ� ���� ��ŵ
                    if (closedList.Contains(neighbour))
                        continue;

                    float newMovementCost_ToNeighbour = currentTile.gCost + getDistanceCost(currentTile, neighbour);
                    if (newMovementCost_ToNeighbour < neighbour.gCost) // ���ο� �ڽ�Ʈ�� ���� �ڽ�Ʈ���� ������? �ְ� �ƴϸ� ��ŵ
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
        // ��ã�� ����
        if (pathSuccess) // �� ã������
        {
            objectiveTile = RetracePath(startTile, endTile); // ��θ� ����
        }
        //������ ��ǥ�� ���� waypoints�� �������θ� �Ŵ����Լ����� �˷��ش�
        requestManager.FinishedProcessingPath(objectiveTile, pathSuccess);
        yield return null;
    }


    // Ž������ �� ���� ����� ParentTile�� �����ϸ� ����Ʈ�� ��´�.
    // ���� ��ο� �ִ� ������ ��ǥ�� ���������� ��� ����
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

    // �� ��尣�� �Ÿ��� Cost�� ��������� �밢���̵��� �����̵��� ���̰� ���� ���� ������ ���Ÿ� ��� x
    // ���� ��ǥ�������� �Ÿ��� �߽����� ��
    int getDistanceCost(ATile TileA, ATile TileB)
    {
        int Dist = (int)Mathf.Abs(Vector3.Distance(TileA.refTile.transform.position, TileB.refTile.transform.position));
        return Dist;
    }

}
