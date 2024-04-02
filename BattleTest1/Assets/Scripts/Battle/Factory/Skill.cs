using UnityEngine;

public class Skill
{
    public float duration
    {
        get { return duration; }
        set { duration = value; }
    }
    public Types.SkillType skillType
    {
        get { return skillType; }
        set { skillType = value; }
    }
    public GameObject[] target
    {
        get { return target; }
        set { target = value; }
    }
    public string name
    {
        get { return name; }
        set { name = value; }
    }

    public Skill(Types.SkillType _skillType, string name, GameObject[] _target, float _duration)
    {

    }
}