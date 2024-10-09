using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    PathFinding pathFinding;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    // 길찾기 요청
    public static void RequestPath(TileController pathStart, TileController pathEnd, UnityAction<TileController, bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback); // 새로운 요청을 만듦
        instance.pathRequestQueue.Enqueue(newRequest); // 큐에 넣음
        instance.TryProcessNext(); // 다음 단계를 시작함
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0) // 큐에 남은 작업이 있고, 길찾기 프로세스가 실행중이지 않을 때 
        {
            currentPathRequest = pathRequestQueue.Dequeue(); // 큐에 작업을 빼고 
            isProcessingPath = true; // 길찾기 플레그를 세우고
            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd); // 길찾기를 시작.
        }
    }
   
    public void FinishedProcessingPath(TileController path, bool success)
    {
        currentPathRequest.callback(path, success); // 콜백에 담고
        isProcessingPath = false; // 플래그 고치고
        TryProcessNext(); // 다음꺼 진행
    }
}

struct PathRequest
{
    public TileController pathStart;
    public TileController pathEnd;
    public UnityAction<TileController, bool> callback;

    public PathRequest(TileController nStart, TileController nEnd, UnityAction<TileController, bool> nCallback)
    {
        pathStart = nStart;
        pathEnd = nEnd;
        callback = nCallback;
    }
}