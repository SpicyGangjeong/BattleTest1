using UnityEditor;
using UnityEngine;

public class GameManager
{
    public bool isOnBattle = false;
    static GameManager _Instance = null;
    public static GameManager getInstance()
    {
        if (_Instance == null)
        {
            _Instance = new GameManager();
        }
        return _Instance;
    }
    public GameManager()
    {

    }


}