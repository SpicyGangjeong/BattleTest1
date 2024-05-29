using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBenchController : MonoBehaviour
{
    public static GameObject unitBench;
    public static GameObject[] unitList;
    // Start is called before the first frame update
    void Start()
    {
        unitBench = this.gameObject;
        unitList = new GameObject[9];
        for (int i = 0; i < unitList.Length; i++)
        {
            TileController tileController = AMapController.tiles[i, 0].GetComponent<TileController>();
            tileController.placable = true;
            tileController.AlterHaloAndCenterVisible(true);
            tileController.tileContainer = Types.TileContainer.UnitBench;
            tileController.tileIndex = i;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public static int PlaceItem(Item item)
    {
        if (item.GetType() == typeof(UnitItem))
        {
            int location = -1;
            for (int i = 0; i < unitList.Length; i++)
            {
                if (unitList[i] == null)
                {
                    location = i;
                    break;
                }
            }
            if (location != -1)
            {
                UnitItem unitItem = (UnitItem)item;
                GameObject itemObject = Instantiate(unitItem.loadItem());
                itemObject.GetComponent<UnitManager>().setInitialStatus(unitItem);
                //TODO ADD Stat;
                for (int index = 0; index < 9; index++)
                {
                    if (unitList[index] is null)
                    {
                        PlaceUnit(itemObject, index);
                        break;
                    }
                }
            }
            else
            {
                return -1;
            }
        }
        else if (item.GetType() == typeof(RelicItem))
        {

        }
        else if (item.GetType() == typeof(EquipmentItem))
        {

        }
        else if (item.GetType() == typeof(ConsumableItem))
        {

        }
        return 1;
    }
    public static void PlaceUnit(GameObject itemObject, int index)
    {
        unitList[index] = itemObject;

        TileController tileController = AMapController.tiles[index, 0].GetComponent<TileController>();
        itemObject.transform.position = tileController.getLocation();
        itemObject.transform.parent = unitBench.transform;
        tileController.placable = false;
        tileController.AlterHaloAndCenterVisible(false);
    }
    public static void PopUnit(GameObject itemObject, int index)
    {
        unitList[index] = null;

        TileController tileController = AMapController.tiles[index, 0].GetComponent<TileController>();
        tileController.placable = true;
        tileController.AlterHaloAndCenterVisible(true);
    }
}
