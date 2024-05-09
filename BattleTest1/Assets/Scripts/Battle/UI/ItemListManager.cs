
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemListManager : MonoBehaviour
{
    static int level;
    public static List<Item> itemList = new List<Item>(5);
    public static List<GameObject> itemPanelList = new List<GameObject>(5);
    GameObject itemPrefab;
    HorizontalLayoutGroup horizontalLayoutGroup;

    [SerializeField]
    public DeckManager deckManager;

    public void Start()
    {
        itemPrefab = Resources.Load("Prefabs/BattleScene/Item") as GameObject;
        horizontalLayoutGroup = transform.GetComponent<HorizontalLayoutGroup>();
        level = 0;
        deckManager.deck.setDeck(level);
        Reroll();
    }
    public void Reroll()
    {
        horizontalLayoutGroup.enabled = true;
        itemList.Clear();
        if (itemPanelList != null)
        {
            for (int i = 0; i < itemPanelList.Count; i++)
            {
                Destroy(itemPanelList[i].gameObject);
            }
        }
        while (itemList.Count < 5)
        {
            Item item = deckManager.deck.drawDeck();
            GameObject itemPanel = Instantiate(itemPrefab);
            itemList.Add(item);
            itemPanelList.Add(itemPanel);
            if (item is UnitItem) // cast
            {
                item = (UnitItem)item;
            }
            else if (item is EquipmentItem)
            {
                item = (EquipmentItem)item;
            }
            else if (item is RelicItem)
            {
                item = (RelicItem)item;
            }
            else if (item is ConsumableItem)
            {
                item = (ConsumableItem)item;
            }
            else
            {
                Debug.Log("Item Cast Error");
            }
            itemPanel.transform.parent = transform;
            itemPanel.GetComponent<ItemController>().item = item; ;
            itemPanel.SetActive(true);

        }
    }
    public void Sell()
    {

    }

    public static List<Item> getItemList()
    {
        return itemList;
    }
    public static List<GameObject> getItemPanelList()
    {
        return itemPanelList;
    }

}
