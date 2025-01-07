using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ExitGen : AbstractGenerator
{
    public GameObject exitPref;
    public static GameObject Exit;
    public override void StartGeneration(List<Room> rooms, HashSet<Vector2Int> fullFloor, TilemapVisualizer tilemapVisualizer,
        SimpleRandomWalkScripObj parameters)
    {
        if (rooms.Count>3)
        {
            Room room = Room.GetBossRoom(rooms);
            //Room room = Room.FindRoomByCenter(rooms, Vector2Int.zero); //debug
            PlaceExit(room, tilemapVisualizer);
        }
    }
    
    private void PlaceExit(Room room, TilemapVisualizer tilemapVisualizer)
    {
        List<Vector2Int> floor = new List<Vector2Int>(room.FloorPos);
        Vector2Int exitPos = floor[Random.Range(0, floor.Count)];
        while (room.RoomLightPos.Contains(exitPos) || room.RoomDecorationPos.Contains(exitPos) || room.NearWallFloorPosX8.Contains(exitPos))
        {
            exitPos = floor[Random.Range(0, floor.Count)];
        }
        Exit = Instantiate(exitPref, new Vector3(exitPos.x + 0.5f, exitPos.y + 0.5f, 0), exitPref.transform.rotation);
        Exit.GetComponent<Exit>()._room = room;
        if (tilemapVisualizer.showDebugOnTiles)
        {
            Debug.Log(Exit.transform.position);
        }
    }
}
