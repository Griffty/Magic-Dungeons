using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungAlgorithm : AbstractDungeonGenerator // WTF IS Happening with corridors generation???
{
    [SerializeField] private List<AbstractGenerator> allGenerators;
    private RoomManager _roomManager;
    private EnemyGenerator _enemyGenerator;
    private AstarPath _pathFinder;  

    private void Start()
    {
        RunProceduralGeneration(false);
    }

    protected override void RunProceduralGeneration(bool inUnity)
    {
        TilemapVisualizer.SetParameters(parameters);
        tilemapVisualizer.Clear();
        parameters.InUnity = inUnity;
        _roomManager =  FindObjectOfType<RoomManager>();
        _enemyGenerator = FindObjectOfType<EnemyGenerator>();
        _pathFinder = FindObjectOfType<AstarPath>();
        CorridorFirstGen();
    }

    private void CorridorFirstGen()
    {
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> possibleRoomPos = new HashSet<Vector2Int>();
        
        CreateCorridors(floorPos, possibleRoomPos);
        HashSet<Vector2Int> corridorPos = new HashSet<Vector2Int>(floorPos);
        
        List<Room> rooms = CreateRooms(possibleRoomPos, corridorPos);
        
        
        Room.AllRooms = rooms;
        HashSet<Vector2Int> roomsFloorPos = CreateFullFloor(rooms);
        floorPos.UnionWith(roomsFloorPos);

        tilemapVisualizer.PainFloorTiles(floorPos, rooms, corridorPos);
        foreach (var generator in allGenerators)
        {
            generator.StartGeneration(rooms, floorPos, tilemapVisualizer, parameters);
        }
        
        if (!parameters.InUnity)
        {
            if (_roomManager != null) { _roomManager.Setup(rooms); }
            else { Debug.Log("No Room Manager"); }
            if (_enemyGenerator != null ) { _enemyGenerator.StartGeneration(rooms); }
            else { Debug.Log("No Enemy Generator"); }
            if (_pathFinder) { SetupPathFinding(roomsFloorPos); }
            else { Debug.Log("No Path Finder"); }
        }
    }

    private void SetupPathFinding(HashSet<Vector2Int> roomsFloorPos)
    {
        int maxX = 0, maxY = 0, minX = 0, minY = 0;
        foreach (var pos in roomsFloorPos)
        {
            if (maxX < pos.x)
            {
                maxX = pos.x;
            }

            if (minX > pos.x)
            {
                minX = pos.x;
            }
            
            if (maxY < pos.y)
            {
                maxY = pos.y;
            }

            if (minY > pos.y)
            {
                minY = pos.y;
            }
        }

        maxX += 5;
        maxY += 5;
        minX -= 5;
        minY -= 5;

        Vector2Int center = new Vector2Int((maxX + minX) / 2, (maxY + minY) / 2);
        
        _pathFinder.data.gridGraph.center = new Vector3(center.x, center.y, 0);
        int width = (maxX - minX)*2;
        int depth = (maxY - minY)*2;
        _pathFinder.data.gridGraph.SetDimensions(width, depth,0.5f);

        StartCoroutine(ScanArea());
    }

    private IEnumerator ScanArea()
    {
        yield return new WaitForSeconds(0.5f);
        _pathFinder.Scan();
    }

    private HashSet<Vector2Int> CreateFullFloor(List<Room> rooms)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var r in rooms)
        {
            floor.UnionWith(r.FloorPos);
        }
        return floor;
    }

    private List<Room> CreateRooms(HashSet<Vector2Int> possibleRoomPos, HashSet<Vector2Int> corridorPos)
    {
        Hashtable roomCenterToDanger = new Hashtable();
        DijkstraAlgorithm.GetRoomDangerFromCenter(roomCenterToDanger, possibleRoomPos, corridorPos, parameters);
        List<Room> rooms = new List<Room>();
        int i = 0;
        int boosDanger = possibleRoomPos.Select(p => (int)roomCenterToDanger[p]).Prepend(0).Max();
        Vector2Int bossPos = new Vector2Int();
        foreach (var pos in possibleRoomPos.Where(pos => (int)roomCenterToDanger[pos] == boosDanger))
        {
            bossPos = pos;
            break;
        }
        foreach (var pos in possibleRoomPos)
        {
            HashSet<Vector2Int> roomFloor;
            roomFloor = RunRandomWalk(pos, pos == bossPos ? 1.8f : 0);
            rooms.Add(new Room(i, pos, roomFloor, Shift, (int)roomCenterToDanger[pos]));
            if (pos==bossPos)
            {
                rooms[^1].RoomType = Room.AllRoomTypes.BossRoom;
            }
            i++;
        }
        DijkstraAlgorithm.SetRoomTypes(rooms, parameters);
        return rooms;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPos, HashSet<Vector2Int> possibleRoomPos)
    {
        var currentPos = startPos;
        possibleRoomPos.Add(currentPos);
        
        for (int i = 0; i < parameters.corridorAmount; i++)
        {
            var corridor = ProcedeGenerationAlgorithms.RandomWalkCorridor(currentPos, parameters.corridorLenght, parameters.corridorWalkRange);
            currentPos = corridor[corridor.Count - (parameters.corridorWalkRange / 2)]; // WTF???
            possibleRoomPos.Add(currentPos);
            floorPos.UnionWith(corridor);
        }
    }
}

