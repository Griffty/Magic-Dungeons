using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Room
{
    public int RoomIndex;
    public readonly float CreationShift;
    public AllRoomTypes RoomType;
    public int RoomDanger;
    public int RoomRange;
    public Vector2Int RoomCenter;
    public HashSet<Vector2Int> FloorPos;
    public static HashSet<Vector2Int> WallsPos;
    public HashSet<Vector2Int> NearWallFloorPosX4;
    public HashSet<Vector2Int> NearWallFloorPosX8;
    public HashSet<Vector2Int> NearCenterFloorPos;
    
    public List<Vector2Int> RoomDecorationPos;
    public List<Vector2Int> RoomDestroyablePos;
    public List<Vector2Int> RoomPillarsPos;

    public static List<Room> AllRooms;

    public List<Vector2Int> RoomDoorsPos;
    public Hashtable RoomDoorsState;
    public List<Vector2Int> RoomLightPos;
    public Hashtable RoomLightState;

    public bool IsCleared;
    public bool IsActive;
    public bool IsPlayerInside;

    public static HashSet<GameObject> Lights = new();
    public static HashSet<GameObject> Doors = new();
    public static HashSet<GameObject> Destroyable = new();


    public HashSet<Vector2Int> EnemiesPos = new();
    
    public List<Enemy> PreparedEnemies = new();
    public List<Enemy> ActiveEnemies = new();
    public List<Enemy> DeadEnemies = new();
 
    public Room(int roomIndex, Vector2Int roomCenter, HashSet<Vector2Int> floorPos, float creationShift,int roomDanger)
    {
        RoomIndex = roomIndex;
        CreationShift = creationShift;
        RoomCenter = roomCenter;
        FloorPos = floorPos;
        RoomDanger = roomDanger;
        WallsPos = new HashSet<Vector2Int>();
        RoomType = AllRoomTypes.MainRoom;
        RoomLightPos = new List<Vector2Int>();
        RoomLightState = new Hashtable();
        RoomDoorsPos = new List<Vector2Int>();
        RoomDoorsState = new Hashtable();
        RoomDecorationPos = new List<Vector2Int>();
        RoomDestroyablePos = new List<Vector2Int>();
        NearWallFloorPosX4 = new HashSet<Vector2Int>();
        NearWallFloorPosX8 = new HashSet<Vector2Int>();
        NearCenterFloorPos = new HashSet<Vector2Int>();
        RoomPillarsPos = new List<Vector2Int>();
        FindNearCenterPos();
    }


    public void FindNearWallPos(bool cardinalDirections)
    { 
        foreach (var pos in FloorPos)
        {
            if (cardinalDirections)
            {
                foreach (var dir in Direction2D.CardinalDirectionList)
                {
                    if (WallsPos.Contains(pos+dir))
                    {   
                        NearWallFloorPosX4.Add(pos);
                        break;
                    }
                }
            }
            else
            {
                foreach (var dir in Direction2D.EightDirectionsList)
                {
                    if (WallsPos.Contains(pos+dir))
                    {   
                        NearWallFloorPosX8.Add(pos);
                        break;
                    }
                }
            }

        }
    }
    public void FindNearCenterPos()
    {
        int maxX = 0, maxY = 0, minX = 0, minY = 0;
        foreach (var p in FloorPos)
        {
            Vector2Int pos = new Vector2Int(p.x - RoomCenter.x, p.y - RoomCenter.y);
            if (maxX < pos.x)
            {
                maxX = pos.x;
            }

            if (minX > pos.x)
            {
                minX = pos.x;
            }
            
            if (maxY < pos.y)
            {
                maxY = pos.y;
            }

            if (minY > pos.y)
            {
                minY = pos.y;
            }
        }
        
        // int lMaxX = maxX - RoomCenter.x, lMaxY = maxY - RoomCenter.y, lMinX = Math.Abs(minX - RoomCenter.x), lMinY = Math.Abs(minY - RoomCenter.y);
        Vector2Int realCenter = new Vector2Int((maxX+minX)/2+RoomCenter.x, (maxY+minY)/2+RoomCenter.y);
        foreach (var pos in FloorPos)
        {
            // if (pos.x - RoomCenter.x < maxX * 2 / 3 && pos.x - RoomCenter.x > minX * 2 / 3 && pos.y - RoomCenter.y < maxY * 2 / 3 && pos.y - RoomCenter.y > minY * 2 / 3)
            // {
            //     NearCenterFloorPos.Add(pos);
            // }

            if (Mathf.Pow(pos.x - realCenter.x, 2) + Mathf.Pow(pos.y - realCenter.y, 2) < Mathf.Pow(Mathf.Min(maxX, maxY) * 5f / 8, 2))
            {
                NearCenterFloorPos.Add(pos);
            }
        }
    }
    
    public static Room FindRoomByCenter(List<Room> rooms, Vector2Int center)
    {
        foreach (var room in rooms)
        {
            if (room.RoomCenter.Equals(center))
            {
                return room;
            }
        }
        return null;
    }
    
    public static List<Room> FindMaxDangerLevelRooms(List<Room> rooms)
    {
        List<Room> r = new List<Room>{rooms[0]};
        foreach (var room in rooms)
        {
            if(r[0].RoomDanger == room.RoomDanger)
            {
                r.Add(room);
            }
            else
            {
                if(r[0].RoomDanger < room.RoomDanger)
                {
                    r = new List<Room>();
                    r.Add(room);
                }
            }
        }
        return r;
    }
    
    public static Room GetBossRoom(List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            if (room.RoomType == AllRoomTypes.BossRoom)
            {
                return room;
            }
        }
        return null;
    }
    
    public enum AllRoomTypes
    {
        MainRoom,
        PuzzleRoom,
        BossRoom,
        SpawnRoom,
    }

    public static Room GetRoomByPos(Vector2Int pos)
    {
        Room targetedRoom = FindRoomByCenter(AllRooms, pos);
        if (targetedRoom != null)
        {
            return targetedRoom;
        }

        foreach (var room in AllRooms)
        {
            if (room.FloorPos.Contains(pos))
            {
                targetedRoom = room;
                break;
            }
        }
        
        return targetedRoom;
    }
}
