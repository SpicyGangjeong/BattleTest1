using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public bool draggable;
    public bool OnBattle;
    public Tile currentTile;
    public Tile targetTile;
    public Transform target;
    public GameObject StartObject;
    public GameObject DestObject;
    float lineSize = 30.0f;
    float speed = 20;
    Tile[] path;
    int targetIndex;

    void Start()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, -transform.up, lineSize);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.collider.gameObject.name.Contains("Tile"))
            {
                currentTile = hit.collider.transform.GetComponent<Tile>();
            }
        }
    }

    private void OnPathFound(Tile[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine(FollowPath());
            StartCoroutine(FollowPath());
        }
    }
    IEnumerator FollowPath()
    {
        Tile currentWaypoint = path[0];
        targetIndex = 0;
        while (true)
        {
            if (transform.position == currentWaypoint.transform.position + Vector3.up * 2)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.transform.position + Vector3.up * 2, speed * Time.deltaTime);
            yield return null;
        }

    }

    private void Update()
    {
        // Moving Inputs
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            path = null;
            PathRequestManager.RequestPath(currentTile, targetTile, OnPathFound);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, lineSize);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.name.Contains("Tile"))
                {
                    targetTile = hits[i].collider.transform.GetComponent<Tile>();
                    DestObject.transform.position = targetTile.transform.position + Vector3.up * 2;
                    setCurrentTile();
                    break;
                }
            }
        } */
        // end
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastRay();

            if (hit.transform == transform)
            {
                // 드래그가 가능한 상태로 변경
                draggable = true;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            draggable = false;
        }
        if (draggable) // TODO 전투중일 땐 중지해야 함.
        {
            // 현재 화면에 있는 마우스 커서의 x,y 좌표와
            // 카메라를 통해 보는 이 스크립트가 실행되는 오브젝트의 z좌표를 사용해
            // ScreenPoint Vector3 position 값 생성
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            // 오브젝트를 이동할 때 움직일 x,z 좌표를 가진 WorldPoint Vector3 position 생성
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            // 위 worldPosition의 x,z 좌표를 사용하고 직접 y좌표를 설정해 오브젝트 이동
            transform.position = new Vector3(worldPosition.x, 0.5f, worldPosition.z);
        }

    }
    private RaycastHit CastRay()
    {
        // 마우스 커서가 가리키는 카메라가 랜더링하는 가장 먼곳의 위치 ScreenPoint Vector3 position
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        // 마우스 커서가 가리키는 카메라가 랜더링하는 가장 가까운곳의 위치 ScreenPoint Vector3 position
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        // 각 위치를 WorldPosition으로 변경
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        // RaycastHit 정보를 담을 변수 생성
        RaycastHit hit;
        // 현재 worldMousePosNear에서 시작하고 worldMousePosFar로 향하는 Raycast를 생성한다
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        // 정보를 가진 hit 반환
        return hit;
    }
    private void setCurrentTile()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, -transform.up, lineSize);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.collider.gameObject.name.Contains("Tile"))
            {
                currentTile = hit.collider.transform.GetComponent<Tile>();
                StartObject.transform.position = currentTile.transform.position;
            }
        }
    }
}