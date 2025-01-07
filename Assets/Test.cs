using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : Enemy
{
    private Tilemap _tilemap;
    private void Start()
    {

        foreach (var tilemap in FindObjectsByType<Tilemap>(FindObjectsSortMode.None))
        {
            if (tilemap.gameObject.name == "Floor")
            {
                _tilemap = tilemap;
                break;
            }
        }
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        var cellBounds = _tilemap.cellBounds;
        TileBase[] tiles = _tilemap.GetTilesBlock(cellBounds);
        for (int x = 0; x < cellBounds.size.x; x++)
        {
            for (int y = 0; y < cellBounds.size.y; y++)
            {
                if (tiles[x + y * cellBounds.size.x] != null)
                {

                    floor.Add(new Vector2Int(x - cellBounds.size.x / 2, y - cellBounds.size.y / 2));
                }
            }
        }

        AstarPath astarPath = FindObjectOfType<AstarPath>();
        astarPath.Scan();
        room = new Room(-1, new Vector2Int(0, 0), floor, 1, 0);

    }



    private bool isWaiting;
    private void Update()
    {
        if (_movement.hasPath)
        {
            _movement.MoveToTarget(1);
        }
        else
        {
            if (!isWaiting)
            {
                StartCoroutine(MakePath(1f));
            }
        }
        
    }
    
    IEnumerator MakePath(float t)
    {
        isWaiting = true;
        yield return new WaitForSeconds(t);
        _movement.MakePathToRandomSpot();
        isWaiting = false;
    }
}
