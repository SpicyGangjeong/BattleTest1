using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Deck
{
    WeightedRandomPicker<Types.UnitRarity> picker;
    int relicCount = 0; 
    int equipmentCount = 0;
    int consumableCount = 0;
    int specialCount = 0;
    public void setDeck(int level)
    {
        Types types = new Types();
        picker = new WeightedRandomPicker<Types.UnitRarity>();
        foreach (KeyValuePair<Types.UnitRarity, int[]> item in types.DeckTables)
        {
            picker.AddOrSetItem(item.Key, item.Value[level]);
        }
    }
    public Item drawDeck()
    {
        Types.UnitRarity unitRarity = picker.PickRandom();
        switch (unitRarity)
        {
            case Types.UnitRarity.Common:
                return drawItem(Types.UnitRarity.Common);
            case Types.UnitRarity.Uncommon:
                return drawItem(Types.UnitRarity.Uncommon);
            case Types.UnitRarity.Rare:
                return drawItem(Types.UnitRarity.Rare);
            case Types.UnitRarity.Epic:
                return drawItem(Types.UnitRarity.Epic);
            case Types.UnitRarity.Legend:
                return drawItem(Types.UnitRarity.Legend);
            case Types.UnitRarity.Relic:
                if (relicCount != 0)
                {
                    relicCount--;
                    return drawItem(Types.UnitRarity.Relic);
                }
                else
                {
                    return drawDeck();
                }
            case Types.UnitRarity.Equipment:
                if (equipmentCount != 0)
                {
                    equipmentCount--;
                    return drawItem(Types.UnitRarity.Equipment);
                }
                else
                {
                    return drawDeck();
                }
            case Types.UnitRarity.Consumable:
                if (consumableCount != 0)
                {
                    consumableCount--;
                    return drawItem(Types.UnitRarity.Consumable);
                }
                else
                {
                    return drawDeck();
                }
            case Types.UnitRarity.Special:
                if (specialCount != 0)
                {
                    specialCount--;
                    return drawItem(Types.UnitRarity.Special);
                }
                else
                {
                    return drawDeck();
                }
            default: return drawDeck();
        }
    }
    public Item drawItem(Types.UnitRarity unitRarity)
    { 
        Item item = null;
        int itemId = -1;
        Types types = new Types();
        if (types.DrawTable.TryGetValue(unitRarity, out int[] itemIds))
        {
            int randomIndex = Random.Range(0, itemIds.Length);
            itemId = itemIds[randomIndex];
        }
        switch (unitRarity)
        {
            case Types.UnitRarity.Common:
            case Types.UnitRarity.Uncommon:
            case Types.UnitRarity.Rare:
            case Types.UnitRarity.Epic:
            case Types.UnitRarity.Legend:
                item = new UnitItem(itemId);
                break;
            case Types.UnitRarity.Relic:
                item = new RelicItem(itemId);
                break;
            case Types.UnitRarity.Equipment:
                item = new EquipmentItem(itemId);
                break;
            case Types.UnitRarity.Consumable:
                item = new ConsumableItem(itemId);
                break;
            case Types.UnitRarity.Special:
                item = new UnitItem(itemId); // TODO
                break;
            default: break;
        }
        return item;
    }
    public void IncreaseCount(Types.UnitRarity unitRarity)
    {
        switch (unitRarity)
        {
            case Types.UnitRarity.Relic:
                relicCount++;
                break;
            case Types.UnitRarity.Equipment:
                equipmentCount++;
                break;
            case Types.UnitRarity.Consumable:
                consumableCount++;
                break;
            case Types.UnitRarity.Special:
                specialCount++;
                break;
            default: break;
        }
    }
    public void DecreaseCount(Types.UnitRarity unitRarity)
    {
        switch (unitRarity)
        {
            case Types.UnitRarity.Relic:
                relicCount--;
                break;
            case Types.UnitRarity.Equipment:
                equipmentCount--;
                break;
            case Types.UnitRarity.Consumable:
                consumableCount--;
                break;
            case Types.UnitRarity.Special:
                specialCount--;
                break;
            default: break;
        }
    }
}