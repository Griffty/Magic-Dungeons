using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private RoomManager roomManager;
    private Room _activeRoom;
    
    private void Update()
    {
        if (_activeRoom == null)
        {
            return;
        }
        if (_activeRoom.ActiveEnemies.Count == 0)
        {
            SetRoomCleaned();
        }
    }
    private void SetRoomCleaned()
    {
        _activeRoom.IsCleared = true;
        RoomManager.IsOpen = true;
        RoomManager.ChangeDoorState();
        _activeRoom = null;
    }
    
    public void Spawn(Room room)
    {
        _activeRoom = room;
        if (room.RoomType == Room.AllRoomTypes.BossRoom)
        {
            SpawnBoss();
        }
        else
        {
            SpawnEnemies();
        }
    }
    
    private void SpawnEnemies()
    {
        _activeRoom.ActiveEnemies = new List<Enemy>(_activeRoom.PreparedEnemies);
        foreach (var e in _activeRoom.ActiveEnemies)
        {
            if (e == null)
            {
                continue;
            }
            e.gameObject.SetActive(true);
        }
    }
    
    private void SpawnBoss()
    {
        _activeRoom.ActiveEnemies = new List<Enemy>(_activeRoom.PreparedEnemies);
        _activeRoom.ActiveEnemies[0].gameObject.SetActive(true);
    }
}