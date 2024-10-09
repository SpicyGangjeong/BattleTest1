using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour
{
    GameManager gameManager = GameManager.getInstance();
    public bool draggable;
    public bool enemyFound = false;
    public bool followingPath = false;
    public bool isOnRange = false;
    public TileController currentTile;
    public TileController targetTile;
    public GameObject enemy;
    float lineSize = 30.0f;
    float speed = 1;
    int targetIndex;
    public StatManager statManager;
    [SerializeField]
    public bool isAlly = true;
    public TileController nextTile;

    void Start()
    {
        statManager = transform.GetComponent<StatManager>();
        setCurrentTile();
    }

    private void OnPathFound(TileController nextPath, bool pathSuccessful)
    {
        if (nextPath == targetTile)
        {
            Debug.Log("isOnRange!!");
            isOnRange = true;
            followingPath = false;
        } 
        else if (pathSuccessful)
        {
            nextTile = nextPath;
            StopAllCoroutines();
            StartCoroutine(FollowPath());
        }
        else
        {
            Debug.Log("PathNotFound: " + gameObject.name);
            enemyFound = false;
            followingPath = false;
        }
    }

    IEnumerator FollowPath()
    {
        TileController currentWaypoint = nextTile;
        followingPath = true;


        if (currentWaypoint.tileState == Types.TileState.Open)
        {
            currentWaypoint.tileState = Types.TileState.Object;
            currentTile.tileState = Types.TileState.Open;
            currentTile = currentWaypoint;
            while (true)
            {
                if (transform.position == currentWaypoint.getLocation())
                {
                    yield return new WaitForSeconds(0.2f);
                    break;
                }


                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.getLocation(), speed * Time.deltaTime);
                yield return null;
            }
        }
        enemyFound = false;
        followingPath = false;
        targetTile = null;
    }

    private void Update()
    {
        // Moving Inputs
        if (isAlly) 
        {
            if (gameManager.isOnBattle)
            {
                findEnemyAndTrace();
            }
            else
            {
                HandleMouseInput();
            }
        }
        else
        {
            if (gameManager.isOnBattle)
            {
                findEnemyAndTrace();
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = CastingTool.CastRay();
            if (hit.transform == transform)
            {
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
                    HandleTilePlacement(newTile, ref isMovingSuccessful);
                    break;
                }
            }
            HandleUIInteraction();
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

    private void HandleTilePlacement(TileController newTile, ref bool isMovingSuccessful)
    {
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
    }

    private void HandleUIInteraction()
    {
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
    }

    private void findEnemyAndTrace()
    {
        if (!enemyFound)
        {
            enemy = null;
            targetTile = null;
            enemy = FindNearestEnemy(); //TODO: 이동하고(공격하고) 매번 가까운 확인해줘야함
            if (enemy != null)
            {
                enemyFound = true;
                targetTile = enemy.GetComponent<UnitController>().currentTile;
            }
        }
        if (!isOnRange)
        {
            int distance = getDistanceCost(currentTile, targetTile);
            int range = statManager.unitStat.attackRange;
            if (distance <= range)
            {
                isOnRange = true;
            }
        }
        if (!followingPath && enemyFound && !isOnRange)
        {
            nextTile = null;
            followingPath = true;
            PathRequestManager.RequestPath(currentTile, targetTile, OnPathFound);
        } 
        else if (!followingPath && enemyFound && isOnRange)
        {
            Attack();
        }
    }
    public void Attack()
    {

    }
    private void MovingFailure(bool isMovingSuccessful)
    {
        if (!isMovingSuccessful)
        {
            draggable = false;
            transform.position = currentTile.getLocation();
        }
    }

    public GameObject FindNearestEnemy()
    {
        float distance = float.MaxValue;
        GameObject nearestEnemy = null;
        if (isAlly)
        {
            foreach (GameObject enemy in EnemyFieldController.enemyList)
            {
                float dummyDistance = (transform.position - enemy.transform.position).magnitude;
                if (dummyDistance < distance)
                {
                    distance = dummyDistance;
                    nearestEnemy = enemy;
                }
            }
        }
        else
        {
            foreach (GameObject unit in UnitFieldController.unitList)
            {
                float dummyDistance = (transform.position - unit.transform.position).magnitude;
                if (dummyDistance < distance)
                {
                    distance = dummyDistance;
                    nearestEnemy = unit;
                }
            }
        }
        return nearestEnemy;
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
    int getDistanceCost(TileController TileA, TileController TileB)
    {
        int Dist = (int)Mathf.Abs(Vector3.Distance(TileA.transform.position, TileB.transform.position));
        return Dist;
    }
}
