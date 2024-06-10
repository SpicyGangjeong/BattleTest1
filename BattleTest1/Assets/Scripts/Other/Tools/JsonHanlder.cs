using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonHandler
{
    public static Stat getStat(TextAsset jsonData, int unitCode)
    {
        string json = jsonData.text;

        try
        {
            // JSON ���ڿ��� ��ü�� ��ȯ
            var sList = Json.Deserialize(json) as Dictionary<string, object>;
            if (sList == null)
            {
                Debug.LogError("Failed to deserialize JSON.");
                return null;
            }

            // "statsList" Ű�� �����ϴ��� Ȯ��
            if (!sList.ContainsKey("statsList"))
            {
                Debug.LogError("Key 'statsList' not found in JSON.");
                return null;
            }

            // "statsList"�� ����Ʈ�� ��ȯ
            var statsList = sList["statsList"] as List<object>;
            if (statsList == null)
            {
                Debug.LogError("Failed to parse 'statsList' from JSON.");
                return null;
            }

            // �� ��ü�� ����� �α׷� ���
            foreach (var statObj in statsList)
            {
                var statDict = statObj as Dictionary<string, object>;
                if (Convert.ToInt32(statDict["unitCode"]) == unitCode)
                {
                    Stat stat = new Stat();
                    stat.unitCode = Convert.ToInt32(statDict["unitCode"]);
                    stat.unitType = (Types.UnitType)Enum.Parse(typeof(Types.UnitType), statDict["unitType"].ToString());
                    stat.job = (Types.UnitJob)Enum.Parse(typeof(Types.UnitJob), statDict["job"].ToString());
                    stat.faction = (Types.UnitFaction)Enum.Parse(typeof(Types.UnitFaction), statDict["faction"].ToString());
                    stat.rarity = (Types.UnitRarity)Enum.Parse(typeof(Types.UnitRarity), statDict["rarity"].ToString());
                    stat.unitName = statDict["unitName"].ToString();
                    stat.star = Convert.ToInt32(statDict["star"]);
                    stat.maxHp = Convert.ToInt32(statDict["maxHp"]);
                    stat.curHp = Convert.ToInt32(statDict["curHp"]);
                    stat.maxMp = Convert.ToInt32(statDict["maxMp"]);
                    stat.curMp = Convert.ToInt32(statDict["curMp"]);
                    stat.melee = Convert.ToInt32(statDict["melee"]);
                    stat.sorcery = Convert.ToInt32(statDict["sorcery"]);
                    stat.attackRange = Convert.ToInt32(statDict["attackRange"]);
                    stat.skillRange = Convert.ToInt32(statDict["skillRange"]);
                    stat.meleeGuard = Convert.ToInt32(statDict["meleeGuard"]);
                    stat.sorceryGuard = Convert.ToInt32(statDict["sorceryGuard"]);
                    stat.criticalChance = Convert.ToInt32(statDict["criticalChance"]);
                    stat.criticalMagnification = Convert.ToInt32(statDict["criticalMagnification"]);
                    stat.meleeSpeed = Convert.ToSingle(statDict["meleeSpeed"]);
                    stat.moveSpeed = Convert.ToSingle(statDict["moveSpeed"]);
                    return stat;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception during JSON parsing: " + ex.Message);
        }

        Debug.LogError("JsonHandler_Invalid_UnitCode");
        return null;
    }
}