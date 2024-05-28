using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public TextAsset jsonData;
    public Stat unitStat;
    public void setStats(Types.UnitCode unitCode)
    {
        string json = jsonData.text;
        StatList sList = JsonUtility.FromJson<StatList>(json);

        // 변환된 객체를 사용하여 작업 수행
        foreach (Stat _stat in sList.statsList)
        {
            if (_stat.unitCode == (int)unitCode)
            {
                unitStat = _stat; 
                break;

            }
        }
    }
    public void starUp(int destStar)
    {
        while(destStar != unitStat.star)
        {
            unitStat.star++;
            unitStat.maxHp = (int)Math.Round(unitStat.maxHp * 1.8);
            unitStat.curHp = (int)Math.Round(unitStat.curHp * 1.8);
            unitStat.melee = (int)Math.Round(unitStat.melee * 1.5);
        }
    }
    public void getDamaged()
    {

    }
    public void putGear()
    {

    }
}