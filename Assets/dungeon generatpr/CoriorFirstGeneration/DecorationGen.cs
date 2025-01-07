using System.Collections.Generic;
using UnityEngine;

public class DecorationGen : AbstractGenerator
{
    public override void StartGeneration(List<Room> rooms, HashSet<Vector2Int> fullFloor, TilemapVisualizer tilemapVisualizer, SimpleRandomWalkScripObj parameters)
    {
        FindObjPos(rooms, parameters.decorationAmount);
        PlaceDecorations(rooms, tilemapVisualizer);
    }

    private void FindObjPos(List<Room> rooms, int decorationAmount)
    {
        foreach (var room in rooms)
        {
            foreach (var pos in room.FloorPos)
            {
                if (!room.NearWallFloorPosX8.Contains(pos))
                {
                    room.RoomDecorationPos.Add(pos);
                }
            }
            room.RoomDecorationPos = GetAmount(room.RoomDecorationPos, (int)(decorationAmount * room.CreationShift));
        }
    }

    private static void PlaceDecorations(List<Room> rooms, TilemapVisualizer tilemapVisualizer)
    {
        foreach (var room in rooms)
        {
            foreach (var pos in room.RoomDecorationPos)
            {
                tilemapVisualizer.PaintSingleRandomDecoration(pos);
            }
        }
    }
}
