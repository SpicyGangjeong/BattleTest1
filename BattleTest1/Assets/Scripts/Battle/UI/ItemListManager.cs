using System.Collections;
using System.Collections.Generic;
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

        }
    }
    public void Purchase()
    {

    }
    public void Sell()
    {

    }
}
