using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour
{
    public bool draggable;
    public bool isOnBattle;
    public TileController currentTile;
    public TileController targetTile;
    public Transform target;
    public GameObject startObject;
    public GameObject destObject;
    float lineSize = 30.0f;
    float speed = 20;
    TileController[] path;
    int targetIndex;
    public StatManager statManager;


    void Start()
    {
        isOnBattle = false;
        statManager = transform.GetComponent<StatManager>();
        setCurrentTile();
    }

    private void OnPathFound(TileController[] newPath, bool pathSuccessful)
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
        TileController currentWaypoint = path[0];
        targetIndex = 0;
        while (true)
        {
            if (transform.position == currentWaypoint.getLocation())
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.getLocation(), speed * Time.deltaTime);
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
            RaycastHit hit = CastingTool.CastRay();
            if (hit.transform == transform)
            {
                //TODO StatManager의 value를 쓰도록 바꿔야함
                int value = statManager.getCost();
                SaleManager.AlterVisible();
                SaleManager.AlterValue(value);

                draggable = true;
            }
        }
        if (draggable && Input.GetMouseButtonUp(0))
        {
            RaycastHit[] hits = CastingTool.CastRayArray();
            bool isMovingSuccessful = false;
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.name.Contains("Tile") && hitObject.transform.GetComponent<TileController>().placable)
                {
                    TileController newTile = hitObject.transform.GetComponent<TileController>();
                    draggable = false;
                    switch (newTile.tileContainer)
                    {
                        case Types.TileContainer.UnitBench:
                            UnitBenchController.PlaceUnit(gameObject, newTile.tileIndex);
                            isMovingSuccessful = true;
                            break;
                        case Types.TileContainer.UnitField:
                            UnitFieldController.PlaceUnit(gameObject, newTile);
                            isMovingSuccessful = true;
                            break;
                        case Types.TileContainer.EnemyField:
                        case Types.TileContainer.Wall:
                        default:
                            MovingFailure(isMovingSuccessful);
                            break;
                    }
                    if (isMovingSuccessful)
                    {
                        switch (currentTile.tileContainer)
                        {
                            case Types.TileContainer.UnitBench:
                                UnitBenchController.PopUnit(gameObject, currentTile.tileIndex);
                                currentTile = newTile;
                                break;
                            case Types.TileContainer.UnitField:
                                UnitFieldController.PopUnit(currentTile);
                                currentTile = newTile;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                }
            }
            RaycastResult[] uiHits = CastingTool.CastUIRayArray();
            for (int i = 0; i < uiHits.Length; i++)
            {
                if (uiHits[i].gameObject.name.Contains("SellPanel"))
                {
                    switch (currentTile.tileContainer)
                    {
                        case Types.TileContainer.UnitBench:
                            UnitBenchController.PopUnit(gameObject, currentTile.tileIndex);
                            break;
                        case Types.TileContainer.UnitField:
                            UnitFieldController.PopUnit(currentTile);
                            break;
                        default:
                            break;
                    }
                    SaleManager.sellUnit(gameObject, statManager.getCost());
                }
            }
            SaleManager.AlterVisible();
            MovingFailure(isMovingSuccessful);
        }
        if (draggable && !isOnBattle) // TODO 전투중일 땐 중지해야 함.
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            transform.position = new Vector3(worldPosition.x, 5f, worldPosition.z);

        }

    }

    private void MovingFailure(bool isMovingSuccessful)
    {
        if (!isMovingSuccessful)
        {
            draggable = false;
            transform.position = currentTile.getLocation();
        }
    }

    public void setCurrentTile()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position + Vector3.up * 0.5f, -transform.up, lineSize);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.collider.gameObject.name.Contains("Tile"))
            {
                currentTile = hit.collider.transform.GetComponent<TileController>();
            }
        }
    }
}