using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
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

        }
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