using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    public abstract void StartGeneration(List<Room> rooms, HashSet<Vector2Int> fullFloor, TilemapVisualizer tilemapVisualizer, SimpleRandomWalkScripObj parameters);
    protected static List<Vector2Int> GetAmount(List<Vector2Int> objPos, int amount)
    {
        objPos = objPos.OrderBy( _ => Guid.NewGuid()).Take(amount).ToList();
        return objPos;
    }
}
