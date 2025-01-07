using System.Collections.Generic;
using UnityEngine;

public static class TransformUtil
{
    public static Vector2 GetDirFromPos(Vector2 pos, Vector2 targetPos)
    {
        Vector2 relativeMousePos = targetPos - pos;
        return relativeMousePos / Mathf.Max(Mathf.Abs(relativeMousePos.x), Mathf.Abs(relativeMousePos.y));
    }
    
    public static float GetRotFromDir(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    
    public static Vector3 GetRandomPosAroundTarget(int innerRadius, int outerRadius, Vector3 targetPos, HashSet<Vector2Int> possiblePos = null)
    {
        Vector3 newPos = new Vector3((int)(Random.Range(innerRadius, outerRadius) * (Random.Range(0,2)*2-1) + targetPos.x), (int)(Random.Range(innerRadius, outerRadius) * (Random.Range(0,2)*2-1) + targetPos.y), 0);
        if (possiblePos != null)
        {
            while (!possiblePos.Contains(new Vector2Int((int)newPos.x, (int)newPos.y)))
            {
                newPos = new Vector3(
                    (int)(Random.Range(innerRadius, outerRadius) * (Random.Range(0, 2) * 2 - 1) + targetPos.x),
                    (int)(Random.Range(innerRadius, outerRadius) * (Random.Range(0, 2) * 2 - 1) + targetPos.y),
                    0);
            }
        }
        return newPos;
    }
}
