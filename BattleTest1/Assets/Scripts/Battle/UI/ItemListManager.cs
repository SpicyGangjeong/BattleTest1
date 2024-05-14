
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
    public static List<Item> itemList = new List<Item>(5);
    public static List<GameObject> itemPanelList = new List<GameObject>(5);
    GameObject itemPrefab;
    HorizontalLayoutGroup horizontalLayoutGroup;

    [SerializeField]
    public DeckManager deckManager;
    [SerializeField]
    public static int level;

    public void Start()
    {
        itemPrefab = Resources.Load("Prefabs/BattleScene/Item") as GameObject;
        horizontalLayoutGroup = transform.GetComponent<HorizontalLayoutGroup>();
        LevelUp();
        Reroll();
    }
    public void LevelUp()
    {
        Debug.Log("LevelUp To " + level);
        if (level == null)
        {
            level = 0;
        }
        else
        {
            level++;
        }
        deckManager.deck.setDeck(level);
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
            itemPanel.transform.SetParent(transform);
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
