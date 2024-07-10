using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour
{
    GameManager gameManager = GameManager.getInstance();
    public bool draggable;
    public bool enemyFound = false;
    public bool isOnRange = false;
    public TileController currentTile;
    public TileController targetTile;
    GameObject enemy;
    float lineSize = 30.0f;
    float speed = 20;
    int targetIndex;
    public StatManager statManager;
    [SerializeField]
    public bool isAlly = true;
    public TileController[] path;


    void Start()
    {
        statManager = transform.GetComponent<StatManager>();
        setCurrentTile();
    }

    private void OnPathFound(TileController[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopAllCoroutines();
            StartCoroutine(FollowPath());
        } else if (!pathSuccessful)
        {
            Debug.Log("PathNotFound");
            //TODO 다시 적 찾도록 유도해야함
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
                yield return new WaitForSeconds(0.2f);
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    isOnRange = true; // TODO 근접하면 싸우는중인거 손봐야함
                    yield break;
                }
                currentWaypoint = path[targetIndex];
                if (currentWaypoint.tileState == Types.TileState.Block || currentWaypoint.tileState == Types.TileState.Object)
                {
                    path = null;
                    enemyFound = false;
                    yield return new WaitForSeconds(0.2f);
                    yield break;
                }
            }
            currentTile.tileState = Types.TileState.Open;
            currentWaypoint.tileState = Types.TileState.Block;
            currentTile = currentWaypoint;

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.getLocation(), speed * Time.deltaTime);
            // TODO 다음 이동 때 이동 가능 한 타일인지 확인하고 움직여야 함.
            yield return null;
        }
    }
    private void Update()
    {
        // Moving Inputs
        if (isAlly)
        {
            if (gameManager.isOnBattle) // TODO
            {
                if (Input.GetMouseButton(0))
                {
                    if (!enemyFound)
                    {
                        enemy = null;
                        targetTile = null;
                        enemy = FindNearestEnemy();
                        if (enemy != null)
                        {
                            enemyFound = true;
                            targetTile = enemy.GetComponent<UnitController>().currentTile;
                        }
                    }
                    if (enemyFound && !isOnRange)
                    {
                        path = null;
                        PathRequestManager.RequestPath(currentTile, targetTile, OnPathFound);
                    }
                    else if (enemyFound && isOnRange)
                    {
                        Debug.Log("OnBattle");
                    }
                }
            }
            else
            {
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
                if (draggable) // TODO 전투중일 땐 중지해야 함.
                {
                    Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                    transform.position = new Vector3(worldPosition.x, 5f, worldPosition.z);
                }
            }
            /*
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
            }*/
            // end
            

        } 
        else
        {
            if (gameManager.isOnBattle) // TODO
            {
                if (!enemyFound)
                {
                    enemy = null;
                    targetTile = null;
                    enemy = FindNearestEnemy();
                    if (enemy != null)
                    {
                        enemyFound = true;
                        targetTile = enemy.GetComponent<UnitController>().currentTile;
                    }
                }
                if (enemyFound && !isOnRange)
                {
                    path = null;
                    PathRequestManager.RequestPath(currentTile, targetTile, OnPathFound);
                }
            }
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
    public GameObject FindNearestEnemy() // 적이 아군 찾는것도 해야함
    {
        float distance = float.MaxValue;
        GameObject nearistEnemy = null;
        if (isAlly)
        {
            foreach (GameObject enemy in EnemyFieldController.enemyList)
            {
                float dummyDistance = (transform.position - enemy.transform.position).magnitude;
                if (dummyDistance < distance)
                {
                    distance = dummyDistance;
                    nearistEnemy = enemy;
                }
            }
        } else
        {
            foreach (GameObject unit in UnitFieldController.unitList)
            {
                float dummyDistance = (transform.position - unit.transform.position).magnitude;
                if (dummyDistance < distance)
                {
                    distance = dummyDistance;
                    nearistEnemy = unit;
                }
            }
        }
        return nearistEnemy;
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