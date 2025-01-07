using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private EnemyManager enemyManager;
    private static List<Room> _allRooms;
    public static bool IsOpen;
    private Room _playersRoom;

    public void Setup(List<Room> rooms)
    {
        _allRooms = rooms;
        _playersRoom = Room.FindRoomByCenter(_allRooms, Vector2Int.zero);
        _playersRoom.IsPlayerInside = true;
        _playersRoom.IsCleared = true;
    }

    public void OnEnteringRoom(Room room)
    {
        _playersRoom = room;
        room.IsPlayerInside = true;
        if (room.IsCleared)
        {
            return;
        }
        SetRoomActive(room);
        enemyManager.Spawn(room);
    }
    private void SetRoomActive(Room room)
    {
        IsOpen = false;
        ChangeDoorState();
        room.IsActive = true;
    }

    public void OnExitingRoom(Room room)
    {
        _playersRoom = null;
        room.IsPlayerInside = false;
    }

    public static void ChangeDoorState()
    {
        if (IsOpen)
        {
            foreach (var door in Room.Doors)
            {
                door.GetComponent<Door>().SetOpen();
            } 
        }
        else
        {
            foreach (var door in Room.Doors)
            {
                door.GetComponent<Door>().SetClose();
            }
        }
    }
}
