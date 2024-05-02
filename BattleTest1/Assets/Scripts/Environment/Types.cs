using System.Collections.Generic;

public class Types
{
    public enum UnitCode
    {
        Valdric = 100,
        Lorien = 101,
        Thalador = 102,
        SkeletonWarrior = 200,
        SkeletonArcher = 201,
    }
    public enum UnitType
    {
        Master,
        Knight,
        Archer,
        Mage,
        Monster,
    }
    public enum UnitFaction
    {
        Human,
        Elf,
        Evil,
        Relic,
        Consumable,
    }
    public enum UnitJob
    {
        Knight,
        Archer,
        Mage,
        Skeleton,
        Relic,
        Consumable,

    }
    public enum UnitRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legend,
        Relic,
        Equipment,
        Consumable,
    }
    public enum SkillType
    {
        Scope,
        Single,
        Buff,

    }
    public enum RoomType
    {
        Boss,
        Elite,
        Battle,
        Rest,
        Treasure,
        Event,
        Merchant
    }
    public enum TileType
    {
        Wet,
        Oily,
        Dry,
    }
    public enum TileState
    {
        Block,
        Open,
    }
    public Dictionary<UnitRarity, int> DeckTables = new Dictionary<UnitRarity, int>
    { //TODO
        { UnitRarity.Common, 100 },
        { UnitRarity.Uncommon, 100 },
        { UnitRarity.Rare, 100 },
        { UnitRarity.Epic, 100 },
        { UnitRarity.Legend, 100 },
        { UnitRarity.Relic, 100 },
        { UnitRarity.Equipment, 100 },
        { UnitRarity.Consumable, 100 },
    };

    public Dictionary<RoomType, int> EncounterPairs = new Dictionary<RoomType, int>
    {
        { RoomType.Boss, 0 },
        { RoomType.Elite, 160 },
        { RoomType.Battle, 450 },
        { RoomType.Rest, 120 },
        { RoomType.Treasure, 0 },
        { RoomType.Event, 220 },
        { RoomType.Merchant, 50 },
    };
}
