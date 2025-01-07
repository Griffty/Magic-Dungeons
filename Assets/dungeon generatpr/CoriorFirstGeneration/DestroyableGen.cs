using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DestroyableGen : AbstractGenerator
{
    public List<Loot> _allPossibleLoot = new();
    public int maxMoney;
    public GameObject destroyablePref;
    public Transform destroyableParent;

    public override void StartGeneration(List<Room> rooms, HashSet<Vector2Int> fullFloor, TilemapVisualizer tilemapVisualizer,
        SimpleRandomWalkScripObj parameters)
    {
        if (!parameters.InUnity)
        {
            FindAllObjects(rooms, parameters.destroyableAmount);
            PlaceDestroyable(rooms);
            GenerateLootInDestroyable();
        }
    }
    
    private void FindAllObjects(List<Room> rooms, int amount)
    {
        foreach (var room in rooms)
        {
            foreach (var pos in room.FloorPos)
            {
                if(room.FloorPos.Contains(pos)){ 
                    if (!room.NearWallFloorPosX8.Contains(pos))
                    {
                        if (!room.RoomDecorationPos.Contains(pos))
                        {
                            if (pos != new Vector2Int((int)ExitGen.Exit.transform.position.x, (int)ExitGen.Exit.transform.position.y))
                            {
                                room.RoomDestroyablePos.Add(pos);
                            }
                        }
                    }
                }
            }
            room.RoomDestroyablePos = GetAmount(room.RoomDestroyablePos, (int)(amount * room.CreationShift));
        }
    }
    
    private void PlaceDestroyable(List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            foreach (var pos in room.RoomDestroyablePos)
            {
                var posV3 = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0);

                GameObject destroyable = Instantiate(destroyablePref, posV3, destroyableParent.rotation, destroyableParent);
                destroyable.GetComponent<Destroyable>().Room = room;
                Room.Destroyable.Add(destroyable);
            }
        }
    }
    
    private void GenerateLootInDestroyable()
    {
        // RunTest();
        WeightedRandomItemGenerator<ItemData> itemGenerator = new WeightedRandomItemGenerator<ItemData>();
        foreach (var loot in _allPossibleLoot)
        {
            itemGenerator.AddItem(loot.itemData, loot.weight);
        }
        foreach (var destScript in Room.Destroyable.Select(dest => dest.GetComponent<Destroyable>()))
        {
            destScript.itemToDrop = itemGenerator.GetRandomItem();
            destScript.moneyToDrop = Random.Range(0, maxMoney);
        }
    }

    private void RunTest()
    {
        List<ItemData> items = new List<ItemData>();
        WeightedRandomItemGenerator<ItemData> itemGeneratorTest = new WeightedRandomItemGenerator<ItemData>();
        foreach (var loot in _allPossibleLoot)
        {
            itemGeneratorTest.AddItem(loot.itemData, loot.weight);
            items.Add(loot.itemData);
        }
        
        
        int[] ans = new int[3];
        for (int i = 0; i < 100; i++)
        {
            ItemData iD = itemGeneratorTest.GetRandomItem();
            int index = items.IndexOf(iD);
            ans[index]++;
        }
    
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(ans[i]);
        }
    }
}

[Serializable]
public struct Loot
{
    public int weight;
    public ItemData itemData;
} 