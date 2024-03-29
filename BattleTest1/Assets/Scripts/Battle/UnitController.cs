using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public ATile currentTile;
    public ATile targetTile;
    public GameObject startPos;
    public GameObject endPos;
    public AMap Map;
    float speed = 20;
    int targetIndex;
    ATile[] path = null;
    public float lineSize = 16f;
    void Start()
    {
        startPos.transform.position = currentTile.transform.position + new Vector3(0f, 2f, 0f);
        endPos.transform.position = targetTile.transform.position + new Vector3(0f, 2f, 0f);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Transform tileTransform in Map.transform)
            {
                tileTransform.GetComponent<ATile>().gCost = 0;
                tileTransform.GetComponent<ATile>().hCost = 0;
                tileTransform.GetComponent<ATile>().parentTile = null;
            }
            path = null;
            PathRequestManager.RequestPath(currentTile, targetTile, OnPathFound);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];

                if (hit.collider.gameObject.name.Contains("Tile"))
                {
                    currentTile = targetTile;
                    transform.position = currentTile.transform.position + new Vector3(0f, 2f, 0f);
                    targetTile = hit.collider.transform.GetComponent<ATile>();
                    startPos.transform.position = currentTile.transform.position + new Vector3(0f, 2f, 0f);
                    endPos.transform.position = targetTile.transform.position + new Vector3(0f, 2f, 0f);
                    break;
                }
            }
        }
    }

    private void OnPathFound(ATile[] newPath, bool pathSuccessful)
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
        ATile currentWaypoint = path[0];
        targetIndex = 0;
        string str = "UnitPath: ";
        for (int i = 0; i < path.Length; i++)
        {
            str += path[i] + "\n";
        }
        Debug.Log(str);
        while (true)
        {
            if (transform.position == currentWaypoint.transform.position + new Vector3(0f, 2f, 0f))
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.transform.position + new Vector3(0f,2f,0f), speed * Time.deltaTime);
            yield return null;
        }
    }
}
/*
 * 
        string str = "UnitPath: ";
        for (int i = 0; i < path.Length; i++)
        {
            str += path[i] + "\n";
        }
        Debug.Log(str);
 * 
            // Debug.Log(transform.position + " " + (currentWaypoint.transform.position + new Vector3(0f, 2f, 0f)));
            // Debug.Log((transform.position == currentWaypoint.transform.position + new Vector3(0f, 2f, 0f)));
            // Debug.Log((transform.position - currentWaypoint.transform.position + new Vector3(0f, 2f, 0f)).magnitude);
 * 
 * 
 * 
 * 
 * 
 * 
 */