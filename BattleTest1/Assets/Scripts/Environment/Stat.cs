[System.Serializable]
public class Stat
{
    public int unitCode;
    public Types.UnitType unitType;
    public Types.UnitJob job;
    public Types.UnitFaction faction;
    public string unitName;
    public int star;
    public int maxHp;
    public int curHp;
    public int maxMp;
    public int curMp;
    public int melee;
    public int sorcery;
    public int attackRange;
    public int skillRange;
    public int meleeGuard;
    public int sorceryGuard;
    public int rarity;
    public int criticalChance;
    public int criticalMagnification;
    public float meleeSpeed;
    public float moveSpeed;
}

[System.Serializable]
public class StatList
{
    public Stat[] statsList;
}