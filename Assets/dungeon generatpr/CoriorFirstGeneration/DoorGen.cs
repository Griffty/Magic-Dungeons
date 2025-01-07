using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class DoorGen : AbstractGenerator
{
    public Object doorPrefab;
    public Transform doorParent;
    public override void StartGeneration(List<Room> rooms, HashSet<Vector2Int> fullFloor, TilemapVisualizer tilemapVisualizer,
        SimpleRandomWalkScripObj parameters)
    {
        if (!parameters.InUnity)
        {
            FindAllDoors(rooms, fullFloor, tilemapVisualizer.showDebugOnTiles);
            PlaceDoor(rooms);
        }
    }
    
    private void FindAllDoors(List<Room> rooms, HashSet<Vector2Int> fullFloor, bool debug)
    {
        foreach (var room in rooms)
        {
            Hashtable doorsState = new Hashtable();
            HashSet<Vector2Int> doorsPos = new HashSet<Vector2Int>();
            HashSet<Vector2Int> usedPos = new HashSet<Vector2Int>();
            foreach (var pos in room.FloorPos)
            {
                if (!usedPos.Contains(pos))
                {
                    for (int i = 0; i < Direction2D.CardinalDirectionList.Count; i++)
                    {
                        Vector2Int dir = Direction2D.CardinalDirectionList[i];
                        if (!room.FloorPos.Contains(pos + dir))
                        {
                            dir *= 2;
                            if (fullFloor.Contains(pos + dir))
                            {
                                if (!doorsState.ContainsKey(pos + dir))
                                {
                                    // if (room.NearCenterFloorPos.Contains(pos + dir) )
                                    // {
                                    //     continue;
                                    // }
                                    
                                    for (int x = -2; x < 3; x++)
                                    {
                                        for (int y = -2; y < 3; y++)
                                        {
                                            usedPos.Add(new Vector2Int(pos.x + x, pos.y + y));
                                        }
                                    }
                                    doorsPos.Add(pos + dir);
                                    bool rightDoor = IsRightDoor(pos + dir, dir/2, fullFloor);
                                    if (rightDoor)
                                    {
                                        doorsState.Add(pos + dir, i + 4);
                                        Vector2Int secondDoorPos= FindAnotherDoor(i + 4, pos + dir, fullFloor);
                                        doorsPos.Add(secondDoorPos);
                                        doorsState.Add(secondDoorPos, i);
                                    }
                                    else
                                    {
                                        doorsState.Add(pos + dir, i);
                                        FindAnotherDoor(i, pos + dir, fullFloor);
                                        Vector2Int secondDoorPos= FindAnotherDoor(i, pos + dir, fullFloor);
                                        doorsPos.Add(secondDoorPos);
                                        doorsState.Add(secondDoorPos, i + 4);
                                    }
                                    if (debug)
                                    {
                                        Debug.Log(".");
                                    }
                                    
                                    break;
                                }
                                Debug.Log("two doors in one place");
                            }
                        }
                    }
                }
            }
            room.RoomDoorsPos = new List<Vector2Int>(doorsPos);
            room.RoomDoorsState = doorsState;
        }
    }

    private Vector2Int FindAnotherDoor(int i, Vector2Int pos, HashSet<Vector2Int> fullFloor)
    {
        switch (i)
        {
            case 0:
                if (fullFloor.Contains(pos+Vector2Int.right))
                {
                    return pos+Vector2Int.right;
                }
                break;
            
            case 1:
                if (fullFloor.Contains(pos+Vector2Int.down))
                {
                    return pos+Vector2Int.down;
                }
                break;
            
            case 2:
                if (fullFloor.Contains(pos+Vector2Int.left))
                {
                    return pos+Vector2Int.left;
                }
                break;
            case 3:
                if (fullFloor.Contains(pos+Vector2Int.up))
                {
                    return pos+Vector2Int.up;
                }
                break;
            case 4:
                if (fullFloor.Contains(pos+Vector2Int.left))
                {
                    return pos+Vector2Int.left;
                }
                break;
            
            case 5:
                if (fullFloor.Contains(pos+Vector2Int.up))
                {
                    return pos+Vector2Int.up;
                }
                break;
            
            case 6:
                if (fullFloor.Contains(pos+Vector2Int.right))
                {
                    return pos+Vector2Int.right;
                }
                break;
            case 7:
                if (fullFloor.Contains(pos+Vector2Int.down))
                {
                    return pos+Vector2Int.down;
                }
                break;
        }

        throw new Exception("MotherFucker");
    }

    private static bool IsRightDoor(Vector2Int pos, Vector2Int dir, HashSet<Vector2Int> fullFloor)
    {
        if (dir == Vector2Int.up)
        {
            if (fullFloor.Contains(pos+Vector2Int.right))
            {
                return false;
            }if (fullFloor.Contains(pos+Vector2Int.left))
            {
                return true;
            }
            throw new Exception("Fuck up");
        }

        if (dir == Vector2Int.right)
        {
            if (fullFloor.Contains(pos+Vector2Int.down))
            {
                return false;
            }if (fullFloor.Contains(pos+Vector2Int.up))
            {
                return true;
            }
            throw new Exception("Fuck right");
        }
        
        if (dir == Vector2Int.down)
        {
            if (fullFloor.Contains(pos+Vector2Int.left))
            {
                return false;
            }if (fullFloor.Contains(pos+Vector2Int.right))
            {
                return true;
            }
            throw new Exception("Fuck down");
        }
        
        if (dir == Vector2Int.left)
        {
            if (fullFloor.Contains(pos+Vector2Int.up))
            {
                return false;
            }if (fullFloor.Contains(pos+Vector2Int.down))
            {
                return true;
            }
            throw new Exception("Fuck down");
        }

        throw new Exception("Fuck everywhere");
    }

    private void PlaceDoor(List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            foreach (var doorPos in room.RoomDoorsPos)
            {
                int doorState = (int)room.RoomDoorsState[doorPos];
                PlaceDoor(room, doorPos, doorState);
            }
        }
    }
    private void PlaceDoor(Room room, Vector2Int doorPos, int doorState)
    {
        GameObject doorObject = (GameObject)Instantiate(doorPrefab, new Vector3(doorPos.x+0.5f, doorPos.y+0.5f, 0), doorParent.rotation, doorParent);
        Room.Doors.Add(doorObject);
        Door doorScript = doorObject.GetComponent<Door>();
        if (doorScript == null)
        {
            throw new Exception("No scripts today");
        }
        doorScript.Setup(room, doorState);
    }
}
