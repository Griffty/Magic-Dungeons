using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyGenerator : MonoBehaviour
{
    private EnemyStyle _style;
    [SerializeField] private LevelEnemyData levelEnemyData;
    [SerializeField] private Transform enemyParent;
    
    public List<GameObject> skeletons = new();
    
    public List<GameObject> undead = new();
    
    public List<GameObject> knights = new();
    
    public List<GameObject> goblins = new();
    
    public List<GameObject> spirits = new();

    private List<List<GameObject>> _allStyles;

    public void StartGeneration(List<Room> rooms)
    {
        _allStyles = new List<List<GameObject>>
        {
            skeletons,
            undead,
            knights,
            goblins,
            spirits,
        };
        _style = levelEnemyData.EnemyStyle;

        foreach (var room in rooms)
        {
            if (room.RoomType == Room.AllRoomTypes.SpawnRoom)
            {
                continue;
            }

            if (room.RoomType == Room.AllRoomTypes.BossRoom)
            {
                room.PreparedEnemies.Add(PrepareEnemy(_style, room, out int enemyPower));
                SetEnemyStats(room.PreparedEnemies[0], room.PreparedEnemies[0].enemyType, 10, room);
                return;
            }
            int roomPower = 0;
            while (roomPower < levelEnemyData.averageRoomPower * room.CreationShift)
            {
                room.PreparedEnemies.Add(PrepareEnemy(_style, room, out int enemyPower));
                roomPower += enemyPower;
            }
        }
    }
    
    private void SetEnemyStats(Enemy enemy, EnemyType type, int enemyPower, Room room)
    {
        enemy.EnemyDanger = enemyPower;
        enemy.enemyStyle = _style;
        enemy.enemyType = type;
        enemy.healthHandler.SetMaxHealth(enemy.enemyData.maxHealth * enemyPower * room.RoomDanger, true);
    }
    
    private Enemy PrepareEnemy(EnemyStyle style, Room room, out int enemyPower)
    {
        GameObject enemyPref;
        enemyPower = Random.Range(1, 5);
        EnemyType enemyType = (EnemyType)Random.Range(0, _allStyles[(int)style].Count);
        GameObject e = _allStyles[(int)style][(int)enemyType];

        if (e.TryGetComponent(out MeleeEnemy meleeEnemy))
        {
            enemyPref = meleeEnemy.gameObject;
        }
        else if (e.TryGetComponent(out RangeEnemy rangeEnemy))
        {
            enemyPref = rangeEnemy.gameObject;
        }
        else if (e.TryGetComponent(out StaticEnemy staticEnemy))
        {
            enemyPref = staticEnemy.gameObject;
        }
        else if (e.TryGetComponent(out SupportEnemy supportEnemy))
        {
            enemyPref = supportEnemy.gameObject;
        }
        else
        {
            throw new Exception("Skeleton Sucker");
        }
        
        Vector2Int pos = room.FloorPos.ElementAt(Random.Range(0, room.FloorPos.Count - 1));
        while (room.EnemiesPos.Contains(pos) || !room.NearCenterFloorPos.Contains(pos))
        {
            pos = room.FloorPos.ElementAt(Random.Range(0, room.FloorPos.Count - 1));
        }

        GameObject enemyObject = Instantiate(enemyPref, new Vector3(pos.x, pos.y, 0), enemyPref.transform.rotation,
            enemyParent);
        room.EnemiesPos.Add(pos);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        SetEnemyStats(enemy, enemyType, enemyPower, room);
        enemy.room = room;
        enemyObject.SetActive(false);
        return enemy;
    }
}
public enum EnemyType
{
    Melee,
    Range,
    Static,
    Support,
    Any,
}
public enum EnemyStyle
{
    Skeleton,
    Undead,
    Knight,
    Goblin,
    Spirit,
    Any,
}
public enum EnemyDanger
{
    Common,
    Rare,
}
