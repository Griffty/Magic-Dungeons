using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField] protected TilemapVisualizer tilemapVisualizer;
    [SerializeField] protected Vector2Int startPos = Vector2Int.zero;
    [SerializeField] protected SimpleRandomWalkScripObj parameters;
    protected abstract void RunProceduralGeneration(bool inUnity);
    
    public void GenerateDungeon()
    {
        RunProceduralGeneration(true);
    }
    public static float Shift { get; private set; }
    protected HashSet<Vector2Int> RunRandomWalk(Vector2Int position, float shift)
    {
        var currentPos = position;
        HashSet<Vector2Int> florPos = new HashSet<Vector2Int>();
        
        
        Shift = Random.Range(1f - parameters.roomSizeShift, 1f + parameters.roomSizeShift);
        if (shift != 0)
        {
            Shift = shift;
        }
        
        float halfShift;
        if (Shift > 1) { halfShift = 1 + Mathf.Abs(Shift - 1) / 2 ; }
        else { halfShift = 1 - Mathf.Abs(Shift - 1) / 2; }
        
        int lenght = (int)(parameters.walkLength * Shift);
        int iter = (int)(parameters.iteration * Shift);
        int range = (int)(parameters.roomWalkRange * halfShift);
        for (int i = 0; i < iter; i++)
        {
            var path = ProcedeGenerationAlgorithms.SimpleRandomWalk(currentPos, lenght, range);
            florPos.UnionWith(path);
            if (parameters.startRandomEachIteration)
            {
                currentPos = florPos.ElementAt(Random.Range(0, florPos.Count));
            }
        }
        return florPos;
    }
}
