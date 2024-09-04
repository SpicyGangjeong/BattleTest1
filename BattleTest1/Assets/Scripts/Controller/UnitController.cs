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
        }
        else
        {
            Debug.Log("PathNotFound: " + gameObject.name);
            enemyFound = false;
            findEnemyAndTrace();
        }
    }

    IEnumerator FollowPath()
    {
        TileController currentWaypoint = path[0];
        targetIndex = 0;
        followingPath = true;
        bool isInterupted = false;

        while (true) // TODO: 현재 Path를 전부 따라가도록 설계되었기에 적과 아군 동시에 FollolwPath하게 되면 제대로 이동하지 않는 문제,
                     // HOW: -> Path를 전부 따라가면 안됨, 한칸 이동하고 Path를 다시 갱신하도록 코드를 바꿔야함
                     // BUT: -> 과연 이러한 방식이 효율적이고 과부하를 발생하지 않는가?
        {
            if (transform.position == currentWaypoint.getLocation())
            {
                yield return new WaitForSeconds(0.2f);
                targetIndex++;
                if (enemy == null || enemy.GetComponent<UnitController>().currentTile != targetTile)
                {
                    isInterupted = true;
                    break;
                }
                if (targetIndex >= path.Length) // TODO: 스킬 혹은 공격 사거리에 들어왔는지 확인하는 과정 필요
                {
                    isOnRange = true; // TODO: 조건 만족하면 싸우는 중인 거 구현해야함
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
            yield return null;
        }

        if (isInterupted)
        {
            enemyFound = false;
            followingPath = false;
            findEnemyAndTrace();
            yield return null;
        }

        followingPath = false;
    }

    private void Update()
    {
        // Moving Inputs
        if (isAlly) 
        {
            if (gameManager.isOnBattle) // TODO
            {
                findEnemyAndTrace(); // 비활성화 시키면 아군 이동 안하고 적이 정상적으로 접근함
            }
            else
            {
                HandleMouseInput();
            }
        }
        else // 비활성화 시키면 적 이동안하고 정상적으로 접근함.
        {
            if (gameManager.isOnBattle) // TODO
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
            enemy = FindNearestEnemy();
            if (enemy != null)
            {
                enemyFound = true;
                targetTile = enemy.GetComponent<UnitController>().currentTile;
            }
        }

        if (!followingPath && enemyFound && !isOnRange)
        {
            path = null;
            PathRequestManager.RequestPath(currentTile, targetTile, OnPathFound);
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
}
