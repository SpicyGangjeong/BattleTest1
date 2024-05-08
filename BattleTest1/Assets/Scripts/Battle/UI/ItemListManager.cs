
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemListManager : MonoBehaviour
{
    int level;
    List<Item> itemList = new List<Item>(5);
    List<GameObject> itemPanelList = new List<GameObject>(5);
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
            itemPanel.GetComponent<Button>().onClick.AddListener(() => Purchase(eventData));
            GameObject itemSprite = itemPanel.transform.Find("ItemSprite").GameObject();
            GameObject itemCost = itemPanel.transform.Find("CostPanel").GameObject();
            GameObject itemFaction = itemPanel.transform.Find("FactionPanel").GameObject();
            GameObject itemJob = itemPanel.transform.Find("JobPanel").GameObject();
            GameObject itemName = itemPanel.transform.Find("NamePanel").GameObject();
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
            itemPanel.SetActive(true);
            // TODO itemSprite.GetComponent(typeof(Image))
            itemCost.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(item.getCost());
            // TODO itemFaction.transform.GetChild(0).GetComponent<Image>().sprite = (Sprite)Resources.Load("");
            // TODO itemJob.transform.GetChild(0).GetComponent<Image>().sprite = (Sprite)Resources.Load("");
            itemName.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Convert.ToString(item.getName());
        }
    }
    public void Purchase(PointerEventData eventData)
    {
        if(horizontalLayoutGroup.enabled) horizontalLayoutGroup.enabled = false;
        GameObject clickedObject = eventData.pointerPress;
        Item item = null;
        for (int i = 0; i < itemPanelList.Count; i++)
        {
            if (itemPanelList[i].GetInstanceID() == clickedObject.GetInstanceID())
            {
                clickedObject.SetActive(false);
                item = itemList[i];
            }
        }
    }
    public void Sell()
    {

    }
    IEnumerator GridEnableFalse()
    {
        yield return horizontalLayoutGroup.enabled = false;
    }
}
