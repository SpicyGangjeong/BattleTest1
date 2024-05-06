using System.Collections.Generic;

public class Types
{
    public enum UnitCode
    {
        Valdric = 100,
        Lorien = 101,
        Thalador = 102,
        Gromdur = 103,
        Broggrim = 104,
        Valdar = 105,
        Grimshade = 106,
        Drakynblood = 107,
        Nightfall = 108,
        Warchot = 109,
        Ironclash = 110,
        Gearwatch = 111,
        Waterweaver = 112,
        SkeletonWarrior = 200,
        SkeletonArcher = 201,
    }
    public enum UnitType
    {
        Master,
        Knight,
        Archer,
        Mage,
        Swordman,
        Guardian,
        Fighter,
        Assassin,
        Sorcerer,
        Scouter,
        Gunslinger,
        Monster,
    }
    public enum UnitFaction
    {
        Human,
        Elf,
        Orc,
        Dwarf,
        Ancient,
        Spirit,
        Draken,
        Swarm,
        Legion,
        Mutant,
        Machine,
        Werebeast,
        Evil,
        Relic,
        Equipment,
        Consumable,
        Special,
    }
    public enum UnitJob
    {
        Knight,
        Archer,
        Mage,
        Swordman,
        Guardian,
        Fighter,
        Assassin,
        Sorcerer,
        Scouter,
        Gunslinger,
        Skeleton,
        Relic,
        Equipment,
        Consumable,
        Special,

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
        Special,
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
    public Dictionary<UnitRarity, int[]> DrawTable = new Dictionary<UnitRarity, int[]>
    { //TODO
        { UnitRarity.Common, new int[] {101,102,103,104} },
        { UnitRarity.Uncommon, new int[] {110,111,112} },
        { UnitRarity.Rare, new int[] {107,108,109} },
        { UnitRarity.Epic, new int[] {100} },
        { UnitRarity.Legend, new int[] {105,106} },
        { UnitRarity.Relic, new int[] {} },
        { UnitRarity.Equipment, new int[] {} },
        { UnitRarity.Consumable, new int[] {} },
        { UnitRarity.Special,   new int[] {} },
    };
    public Dictionary<UnitRarity, int[]> DeckTables = new Dictionary<UnitRarity, int[]>
    { //TODO
        { UnitRarity.Common, new int[] {3600, 3600, 2700, 1980, 1620, 1080, 720, 648, 540, 180} },
        { UnitRarity.Uncommon, new int[] { 0, 0, 900, 1080, 1188, 1440, 1188, 972, 720, 360 } },
        { UnitRarity.Rare, new int[] { 0, 0, 0, 540, 720, 900, 1296, 1152, 900, 720 } },
        { UnitRarity.Epic, new int[] { 0, 0, 0, 0, 72, 180, 360, 720, 1080, 1440 } },
        { UnitRarity.Legend, new int[] { 0, 0, 0, 0, 0, 0, 36, 108, 360, 900 } },
        { UnitRarity.Relic, new int[] { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 } },
        { UnitRarity.Equipment, new int[] { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 } },
        { UnitRarity.Consumable, new int[] { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 } },
        { UnitRarity.Special,   new int[] { 100, 100, 100, 100, 100, 100, 100, 100, 100, 100 } },
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
