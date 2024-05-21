using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    GameObject itemListPanel;
    GameObject itemSprite;
    GameObject itemCost;
    GameObject itemFaction;
    GameObject itemJob;
    GameObject itemName;
    public Item item;
    [SerializeField]
    public GameObject aMap;
    void Start()
    {
        itemListPanel = transform.parent.gameObject;
        GameObject itemSprite = transform.Find("ItemSprite").gameObject;
        GameObject itemCost = transform.Find("CostPanel").gameObject;
        GameObject itemFaction = transform.Find("FactionPanel").gameObject;
        GameObject itemJob = transform.Find("JobPanel").gameObject;
        GameObject itemName = transform.Find("NamePanel").gameObject;
        // TODO itemSprite.GetComponent(typeof(Image))
        itemCost.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(item.getCost());
        // TODO itemFaction.transform.GetChild(0).GetComponent<Image>().sprite = (Sprite)Resources.Load("");
        // TODO itemJob.transform.GetChild(0).GetComponent<Image>().sprite = (Sprite)Resources.Load("");
        itemName.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(item.getName());
    }
    void Update()
    {
        
    }

    public void Purchase()
    {
        Boolean isPlacable = false;
        for (int i = 0; i < AMap.unitList.Length; i++)
        {
            if (AMap.unitList[i] == null)
            {
                isPlacable = true;
                break;
            }
        }
        if (isPlacable)
        {
            int isSuccess = AMap.placeItem((UnitItem)item);
            if (itemListPanel.GetComponent<HorizontalLayoutGroup>().enabled)
                itemListPanel.GetComponent<HorizontalLayoutGroup>().enabled = false;
            List<GameObject> itemPanelList = ItemListManager.itemPanelList;
            for (int i = 0; i < itemPanelList.Count; i++)
            {
                if (itemPanelList[i].GetInstanceID() == gameObject.GetInstanceID())
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
