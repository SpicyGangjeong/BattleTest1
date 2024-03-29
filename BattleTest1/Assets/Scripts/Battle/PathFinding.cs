
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
    public void StartFindPath(ATile startTile, ATile targetTile)
    {
        StartCoroutine(FindPath(startTile, targetTile));
    }

    IEnumerator FindPath(ATile startTile, ATile targetTile)
    {
        ATile[] waypoints = new ATile[0]; // ?
        bool pathSuccess = false; // ��ã�� ���� �ߴ��� ���� ����
        if (startTile.tileState == Types.TileState.Open && targetTile.tileState == Types.TileState.Open) // ����Ÿ���� �����ְ� ��Ÿ���� �����ִٸ� ��ã��
        {
            List<ATile> openList = new List<ATile>();
            HashSet<ATile> closedList = new HashSet<ATile>();
            openList.Add(startTile);
            // TODO �����ؼ� ���� ������� ã�°� �ƴ϶� h�ڽ�Ʈ��� �����ϰ� ã�ƾ���
            while (openList.Count > 0) // ã�� ���� �����ִٸ� �ݺ�
            {
                ATile currentTile = openList[0]; 
                // ���� Ÿ�Ͽ��� �� �� �ִ� ����
                // ������Ͽ� F cost�� ���� ���� ��带 ã�´�. ���࿡ F cost�� ���ٸ� H cost�� ���� ��带 ã�´�.
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].fCost < currentTile.fCost || openList[i].fCost == currentTile.fCost && openList[i].hCost < currentTile.hCost)
                    {
                        currentTile = openList[i]; // �� ������¿��� ���� ����� ���� ������ Ž���Ѵٴ� ��
                    }
                }
                // Ž���� ���� ���� ����� ���Ե�� ���� ������Ͽ��� �����ϰ� ������Ͽ� �߰��Ѵ�.
                openList.Remove(currentTile);
                closedList.Add(currentTile);

                // Ž���� ��尡 ��ǥ ����� Ž�� ����
                if (currentTile == targetTile)
                {
                    pathSuccess = true;
                    break;
                }

                // Ž���� ��尡 ��ǥ ��尡 �ƴ϶�� ���Ž��(�̿� ���)
                foreach (ATile n in map.GetNeighbours(currentTile))
                {
                    // �̵��Ұ� ����̰ų� Ž���� ������Ͽ� �ִ� ���� ��ŵ
                    if (n.tileState == Types.TileState.Block|| closedList.Contains(n)) continue;

                    // �̿� ������ G cost�� H cost�� ����Ͽ� ������Ͽ� �߰��Ѵ�.
                    int newCurrentToNeighbourCost = currentTile.gCost + GetDistanceCost(currentTile, n);
                    // �̿������� ���� ��庸�� ����� ���ų� ���¸���Ʈ�� ������ ���ٸ� 
                    if (newCurrentToNeighbourCost < n.gCost || !openList.Contains(n)) 
                    {
                        n.gCost = newCurrentToNeighbourCost; // �̿������� G�ڽ�Ʈ ��� ( g�ڽ�Ʈ - �� ������ ���µ��� ��� )
                        n.hCost = GetDistanceCost(n, targetTile); // �̿������� h�ڽ�Ʈ ��� ( h�ڽ�Ʈ - �̵���� )
                        n.parentTile = currentTile; // ���� ��� ���

                        if (!openList.Contains(n)) // �ٽ��ѹ� ���¸���Ʈ�� ������ ������ Ȯ��
                            openList.Add(n);
                    }
                }
            }
            // ��ã�� ����
        }
        yield return null;
        if (pathSuccess) // �� ã������
        {
            waypoints = RetracePath(startTile, targetTile); // ��θ� ����
        }
        //������ ��ǥ�� ���� waypoints�� �������θ� �Ŵ����Լ����� �˷��ش�
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    // Ž������ �� ���� ����� ParentTile�� �����ϸ� ����Ʈ�� ��´�.
    // ���� ��ο� �ִ� ������ ��ǥ�� ���������� ��� ����
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

    // �� ��尣�� �Ÿ��� Cost�� ��������� �밢���̵��� �����̵��� ���̰� ���� ���� ������ ���Ÿ� ��� x
    // ���� ��ǥ�������� �Ÿ��� �߽����� ��
    int GetDistanceCost(ATile TileA, ATile TileB)
    {
        int distX = (int)MathF.Abs(TileA.transform.position.x - TileB.transform.position.x);
        int distY = (int)MathF.Abs(TileA.transform.position.y - TileB.transform.position.y);

        if (distX > distY)
            return 10 * (distX - distY);
        return 10 * (distY - distX);
    }

}
