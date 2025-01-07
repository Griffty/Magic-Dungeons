using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProcedeGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength, int walkRange)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPos);
        var prevPos = startPos;
        for (int i = 0; i < walkLength; i++)
        {
            var newPos = prevPos + Direction2D.GetRandomDir();
            AddNearest(path, newPos, walkRange);
            prevPos = newPos;
        }
        return path;
    }

    private static List<Vector2Int> _prevDir = new()
    {
        new Vector2Int(0, 0),
        new Vector2Int(0, 0),
        new Vector2Int(0, 0),
        new Vector2Int(0, 0),
    };
    private static int _count;
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int walkLength, int walkRange)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomDir();
        while (ContainsMoreThan(_prevDir, direction))
        {
            direction = Direction2D.GetRandomDir();
        }
        var currentPos = startPos;
        corridor.Add(currentPos);
        for (int i = 0; i < walkLength; i++)
        {
            currentPos += direction;
            AddNearest(corridor, currentPos, direction, walkRange);
        }

        if (_count % 4 == 0) { _prevDir[0] = direction; }
        else if (_count % 4 == 1) { _prevDir[1] = direction; }
        else if (_count % 4 == 2) { _prevDir[2] = direction; }
        else { _prevDir[3] = direction; }
        _count++;
        return corridor;
    }


    private static bool ContainsMoreThan(List<Vector2Int> prevDir, Vector2Int direction)
    {
        int count = 0;
        foreach (var dir in prevDir)
        {
            if (dir.Equals(direction))
            {
                count++;
            }
        }
        if (count>1)
        {
            return true;
        }

        return false;
    }

    private static void AddNearest(List<Vector2Int> corridor, Vector2Int currentPos, Vector2Int dir, int radius)
    {
        if (dir.Equals(Direction2D.CardinalDirectionList[0]) || dir.Equals(Direction2D.CardinalDirectionList[2]))
        {
            for (int x = radius / 2 * -1; x < radius / 2; x++)
            {
                corridor.Add(new Vector2Int(currentPos.x + x, currentPos.y));
            }
        }
        else
        {
            for (int y = radius / 2 * -1; y < radius / 2; y++)
            {
                corridor.Add(new Vector2Int(currentPos.x, currentPos.y + y));
            }
        }
    }
    private static void AddNearest(HashSet<Vector2Int> corridor, Vector2Int currentPos, int radius)
    {
        for (int x = 0; x < radius; x++)
        {
            for (int y = 0; y < radius; y++)
            {
                corridor.Add(new Vector2Int(currentPos.x + x - (radius/2), currentPos.y + y - (radius/2)));
            }
        }
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomsList = new List<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);
        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.x >= minWidth && room.size.y >= minHeight){
                if (Random.value <0.5f)
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minWidth, roomsQueue, room);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minHeight, roomsQueue, room);
                    }else
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minHeight, roomsQueue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minWidth, roomsQueue, room);
                    }else
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }

        return roomsList;
    }
    
    private static void SplitVertically(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var xSplit = Random.Range(minHeight, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z)); //Error
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        var ySplit = Random.Range(minWidth, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z), new Vector3Int(room.size.x, room.size.y  - ySplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }


}

public static class Direction2D
{
    public static List<Vector2Int> CardinalDirectionList = new()
    {
        new(0, 1), //Up
        new(1, 0), // Right
        new(0, -1), // Down
        new(-1, 0), // Left
    };
    public static List<Vector2Int> DiagonalDirectionList = new()
    {
        new(1, 1),
        new(1, -1),
        new(-1, -1),
        new(-1, 1),
    };
    public static List<Vector2Int> EightDirectionsList = new()
    {
        new(0, 1),
        new(1, 1),
        new(1, 0),
        new(1, -1),
        new(0, -1),
        new(-1, -1),
        new(-1, 0),
        new(-1, 1),
    };

    public static Vector2Int GetRandomDir()
    {
        return CardinalDirectionList[Random.Range(0, CardinalDirectionList.Count)];
    }
}