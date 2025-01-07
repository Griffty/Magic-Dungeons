
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PillarsGen : AbstractGenerator
{
    public override void StartGeneration(List<Room> rooms, HashSet<Vector2Int> fullFloor, TilemapVisualizer tilemapVisualizer,
        SimpleRandomWalkScripObj parameters)
    {
        foreach (var room in rooms) 
        {
            HashSet<Vector2Int> usedPos = new HashSet<Vector2Int>();
            int pillarAmount = (int)(parameters.pillarAmount * Math.Pow(room.CreationShift + 0.1, 2));
            for (int i = 0; i < pillarAmount; i++)
            {
                Vector2Int pos = room.NearCenterFloorPos.ElementAt(Random.Range(0, room.NearCenterFloorPos.Count-1));
                int j = 0;
                while (usedPos.Contains(pos) && j < 200 || (pos.x is > -3 and < 4 && pos.y is > -3 and < 4))
                {
                    j++;
                    pos = room.NearCenterFloorPos.ElementAt(Random.Range(0, room.NearCenterFloorPos.Count));
                }
            
                if (j >= 199)
                {
                    break;
                }
                
                for (int x = -6; x < 7; x++)
                {
                    for (int y = -6; y < 7; y++)
                    {
                        usedPos.Add(new Vector2Int(pos.x + x, pos.y + y));
                    }
                }
            
                AddPillar(room, pos);
                tilemapVisualizer.MakePillar(pos);
            }
        }
        
    }

    private void AddPillar(Room room, Vector2Int pos)
    {
        room.RoomPillarsPos.Add(pos);
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                Vector2Int p = new Vector2Int(x + pos.x, y + pos.y);
                room.FloorPos.Remove(p);
                // if (x == 1 && y == 1)
                // {
                //  continue;   
                // }
                // Room.WallsPos.Add(p);
            }   
        }
    }
}
