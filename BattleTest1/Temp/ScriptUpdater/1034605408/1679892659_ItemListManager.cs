
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemListManager : MonoBehaviour
{
    int level;
    List<Item> itemList = new List<Item>(5);
    
    [SerializeField]
    public DeckManager deckManager;
    
    public void Start()
    {
        level = 0;
        deckManager.deck.setDeck(level);
        Reroll();
    }
    public void Reroll()
    {
        itemList.Clear();
        while(itemList.Capacity < 5)
        {
            Item item = deckManager.deck.drawDeck();
            GameObject ItemPanel = (GameObject)Resources.Load("Prefabs/BattleScene/Item");
            GameObject ItemSprite = ItemPanel.transform.Find("ItemSprite").GameObject();
            GameObject ItemCost = ItemPanel.transform.Find("CostPanel").GameObject();
            GameObject ItemFaction = ItemPanel.transform.Find("FactionPanel").GameObject();
            GameObject ItemJob = ItemPanel.transform.Find("JobPanel").GameObject();
            GameObject ItemName = ItemPanel.transform.Find("NamePanel").GameObject();
            if (item is UnitItem)
            {
                item = (UnitItem)item;
            } // cast
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
                Console.WriteLine("Item Cast Error");
            }
            ItemPanel.SetActive(true);
            // ItemSprite.GetComponent(typeof(Image))
            ItemCost.transform.GetChild(0).GetComponent<TextMeshPro>().text = item.getCost();
        }
    }
    public void Purchase()
    {

    }
    public void Sell()
    {

    }
}
