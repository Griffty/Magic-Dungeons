using UnityEngine;
[CreateAssetMenu(fileName = "GameData", menuName = "Custom/GameData")]
public class GameData : ScriptableObject
{
    public int currentScene;
    public int lastLevel;
    private int _levelToLoad;
    
    public void levelToLoad(int a, Object obj){
        _levelToLoad = a;
    }

    public int levelToLoad()
    {
        return _levelToLoad;
    }
}
