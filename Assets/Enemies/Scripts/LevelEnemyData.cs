using UnityEngine;

[CreateAssetMenu(fileName = "LevelEnemyData", menuName = "Custom/LevelEnemyData")]
public class LevelEnemyData : ScriptableObject
{
    public EnemyStyle EnemyStyle;
    public int averageRoomPower;
    public int levelNumber = 1;
}