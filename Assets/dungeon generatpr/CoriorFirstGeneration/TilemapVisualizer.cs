using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] public bool showDebugOnTiles;
    private static SimpleRandomWalkScripObj _parameters;
    [SerializeField] private Tilemap floorTilemap, wallTilemap, decorationsTilemap;
    [SerializeField] private TileBase floorTileCenter, floorTileCorridor, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull, wallInnerCornerDownLeft, wallInnerCornerDownRight, wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;
    [SerializeField] private List<TileBase> floorTiles = new();
    [SerializeField] private List<TileBase> decorationsTiles = new();
    
    public void PainFloorTiles(IEnumerable<Vector2Int> floorPos, List<Room> rooms, HashSet<Vector2Int> corridorPos)
    {
        PainTiles(floorPos, floorTilemap, floorTiles, rooms, corridorPos);
    }
    
    private void PainTiles(IEnumerable<Vector2Int> position, Tilemap tilemap, List<TileBase> tileBases, List<Room> rooms, HashSet<Vector2Int> corridorPos)
    {
        HashSet<Vector2Int> roomCenter = new HashSet<Vector2Int>();
        if (showDebugOnTiles)
        {
            foreach (var room in rooms)
            {
                roomCenter.Add(room.RoomCenter);
            }
        }
        
        foreach (var pos in position)
        {
            if (showDebugOnTiles)
            {
                if (roomCenter.Contains(pos))
                {
                    Debug.Log("Center: " + Room.FindRoomByCenter(rooms, pos).RoomCenter + "||| Danger: " + Room.FindRoomByCenter(rooms, pos).RoomDanger);
                    PainSingleTile(tilemap, floorTileCenter, pos);
                }
                if (corridorPos.Contains(pos))
                {
                    PainSingleTile(tilemap, floorTileCorridor, pos);
                }else
                {
                    var tileBase = GetRandomTileBase(tileBases);
                    PainSingleTile(tilemap, tileBase, pos);
                }
            }
            else
            {
                var tileBase = GetRandomTileBase(tileBases);
                PainSingleTile(tilemap, tileBase, pos);
            }

            
        }
    }

    private void PainSingleTile(Tilemap tilemap, TileBase tileBase, Vector2Int position)
    {
        var tilePos = tilemap.WorldToCell((Vector3Int) position);
        tilemap.SetTile(tilePos, tileBase);
    }
    private TileBase GetRandomTileBase(List<TileBase> tileBases)
    {
        return tileBases[Random.Range(0, tileBases.Count)];
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        decorationsTilemap.ClearAllTiles();
        ClearObjects(_parameters.InUnity);
    }

    private void ClearObjects(bool inUnity)
    {
        if (inUnity)
        {
            foreach (var destroyable in Room.Destroyable)
            {
                DestroyImmediate(destroyable);
            }

            foreach (var door in Room.Doors)
            {
                DestroyImmediate(door);
            }

            foreach (var light2d in Room.Lights)
            {
                DestroyImmediate(light2d);
            }

            DestroyImmediate(ExitGen.Exit);

        }else{
            foreach (var destroyable in Room.Destroyable)
            {
                Destroy(destroyable);
            }

            foreach (var door in Room.Doors)
            {
                Destroy(door);
            }
            
            foreach (var light2d in Room.Lights)
            {
                Destroy(light2d);
            }
            
            Destroy(ExitGen.Exit);
        }

        Room.Lights = new HashSet<GameObject>();
        Room.Destroyable = new HashSet<GameObject>();
        Room.Doors = new HashSet<GameObject>();
        ExitGen.Exit = null;
    }

    public void PainSingleWall(Vector2Int pos, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        if (tile != null)
        {
            PainSingleTile(wallTilemap, tile, pos);
        }
    }

    public void PainSingleCornerWall(Vector2Int pos, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;
        }else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;
        }else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight;
        }else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight;
        }else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        if (tile!=null)
        {
            PainSingleTile(wallTilemap, tile, pos);
        }
    }

    public void PaintSingleRandomDecoration(Vector2Int pos)
    {
        var tileBase = GetRandomTileBase(decorationsTiles);
        PainSingleTile(decorationsTilemap, tileBase, pos);
    }

    public static void SetParameters(SimpleRandomWalkScripObj parameters)
    {
        _parameters = parameters;
    }

    public void MakePillar(Vector2Int pos)
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                PainSingleTile(floorTilemap, null, new Vector2Int(pos.x + x, pos.y + y));
            }
        }
        
        
        PainSingleTile(wallTilemap, wallTop, new Vector2Int(pos.x, pos.y - 1));
        PainSingleTile(wallTilemap, wallTop, new Vector2Int(pos.x + 1, pos.y - 1));
        PainSingleTile(wallTilemap, wallTop, new Vector2Int(pos.x - 1, pos.y - 1));
        
        PainSingleTile(wallTilemap, wallBottom, new Vector2Int(pos.x, pos.y + 1));
        
        PainSingleTile(wallTilemap, wallSideLeft, new Vector2Int(pos.x + 1, pos.y));
        PainSingleTile(wallTilemap, wallSideRight, new Vector2Int(pos.x - 1, pos.y));

        PainSingleTile(wallTilemap, wallInnerCornerDownRight, new Vector2Int(pos.x - 1, pos.y + 1));
        PainSingleTile(wallTilemap, wallInnerCornerDownLeft, new Vector2Int(pos.x + 1, pos.y + 1));
        
        
    }
}
