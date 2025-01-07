
using System.Collections.Generic;
using UnityEngine;

public class WallGen : AbstractGenerator
{
    public override void StartGeneration(List<Room> rooms, HashSet<Vector2Int> fullFloor, TilemapVisualizer tilemapVisualizer, SimpleRandomWalkScripObj parameters)
    {
        CreateBasicWalls(tilemapVisualizer, FileWallsinDirections(fullFloor, Direction2D.CardinalDirectionList), fullFloor);
        CreateCornerWalls(tilemapVisualizer, FileWallsinDirections(fullFloor, Direction2D.DiagonalDirectionList), fullFloor);
        foreach (var room in rooms)
        {
            room.FindNearWallPos(true);
            room.FindNearWallPos(false);
        }
    }
    private static void CreateBasicWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPosition, HashSet<Vector2Int> floorPos)
    {
        foreach (var pos in basicWallPosition)
        {
            string neighboursBinary = "";
            foreach (var dir in Direction2D.CardinalDirectionList)
            {
                var neighbourPos = pos + dir;
                if (floorPos.Contains(neighbourPos))
                {
                    neighboursBinary += "1";
                }
                else
                {
                    neighboursBinary += "0";
                }
            }
            tilemapVisualizer.PainSingleWall(pos, neighboursBinary);
        }
    }
    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPosition, HashSet<Vector2Int> floorPos)
    {
        foreach (var pos in cornerWallPosition)
        {
            string neighboursBinary = "";
            foreach (var dir in Direction2D.EightDirectionsList)
            {
                var neighbourPos = pos + dir;
                if (floorPos.Contains(neighbourPos))
                {
                    neighboursBinary += "1";
                }
                else
                {
                    neighboursBinary += "0";
                }
            }
            tilemapVisualizer.PainSingleCornerWall(pos, neighboursBinary);
        }
    }
    private static HashSet<Vector2Int> FileWallsinDirections(HashSet<Vector2Int> floorPos, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPos = new HashSet<Vector2Int>();
        foreach (var pos in floorPos)
        {
            foreach (var dir in directionList)
            {
                var neighbourPos = pos + dir;
                if (floorPos.Contains(neighbourPos) == false)
                {
                    wallPos.Add(neighbourPos);
                }
            }
        }
        Room.WallsPos.UnionWith(new HashSet<Vector2Int>(wallPos));
        return wallPos;
    }
}
