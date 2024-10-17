using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Types;

public class EnemyFieldController : MonoBehaviour
{
    public static GameObject[] enemyList;
    public GameObject dummyUnit;
    public GameObject[,] enemyField = new GameObject[7,4];
    void Start()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("Prefabs/BattleScene/unitStat");
        for (int y = 5; y < 9; y++) // Ÿ�� �迭 ���
        {
            for (int x = 1; x <= 7; x++)
            {
                enemyField[x - 1, y - 5] = AMapController.tiles[x, y];
            }
        }
        for (int i = 0; i< 4; i++)
        {
            dummyUnit = Instantiate(Resources.Load<GameObject>("Prefabs/BattleScene/SkeletonWarrior")); //TODO
            dummyUnit.transform.parent = transform;
            dummyUnit.name = "dummyUnit(" + i + ")";
            dummyUnit.transform.position = enemyField[i + 2, 1].GetComponent<TileController>().getLocation();
            dummyUnit.GetComponent<UnitController>().setCurrentTile();
            dummyUnit.GetComponent<UnitController>().isAlly = false;
            dummyUnit.GetComponent<UnitController>().statManager = dummyUnit.GetComponent<StatManager>();
            dummyUnit.GetComponent<UnitController>().statManager.setStats(JsonHandler.getStat(jsonData, 200));
        }

        UpdateEnemyList();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            for (int y = 0; y < 4; y++) // Ÿ�� �迭 ���
            {
                for (int x = 0; x < 7; x++)
                {
                    enemyField[x, y].GetComponent<TileController>().tileState = Types.TileState.Open;
                }
            }
            for (int i =0; i< enemyList.Length; i++)
            {
                enemyList[i].GetComponent<UnitController>().currentTile.GetComponent<TileController>().tileState = Types.TileState.Object;
            }
        }
    }
    public void UpdateEnemyList()
    {
        enemyList = null;
        enemyList = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            enemyList[i] = transform.GetChild(i).gameObject;
        }
    }
}