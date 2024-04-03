using System.Collections.Generic;

public class Types
{
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
    }
    public enum UnitJob
    {
        Knight,
        Archer,
        Mage,
        Skeleton,

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
