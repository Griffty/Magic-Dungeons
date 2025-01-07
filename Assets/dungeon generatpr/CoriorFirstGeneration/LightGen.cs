using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class LightGen : AbstractGenerator
{
    [SerializeField] private Sprite torchTileTop, torchTileRight, torchTileBottom, torchTileLeft;
    [SerializeField] private Object light2DPrefab;
    [SerializeField] private Transform light2DParent;

    public override void StartGeneration(List<Room> rooms, HashSet<Vector2Int> fullFloor, TilemapVisualizer tilemapVisualizer,
        SimpleRandomWalkScripObj parameters)
    {
        if (!parameters.InUnity)
        {
            FindObjPos(rooms, parameters.lightAmount, tilemapVisualizer.showDebugOnTiles);
            CreateBasicLight(rooms);
        }
    }

    private void FindObjPos(List<Room> rooms, int lightAmount, bool debug)
    {
        foreach (var room in rooms)
        {
            foreach (var pos in room.FloorPos)
            {
                if (room.NearWallFloorPosX4.Contains(pos))
                {
                    room.RoomLightPos.Add(pos);
                }
            }
            room.RoomLightPos = GetAmount(room.RoomLightPos, (int)(lightAmount * room.CreationShift));
            
            foreach (var lightPos in room.RoomLightPos)
            {
                var lightState = -1;
                if (Room.WallsPos.Contains(lightPos + Direction2D.CardinalDirectionList[0]))
                {
                    lightState = 0;
                }else if (Room.WallsPos.Contains(lightPos + Direction2D.CardinalDirectionList[1]))
                {
                    lightState = 1;
                }else if (Room.WallsPos.Contains(lightPos + Direction2D.CardinalDirectionList[2]))
                {
                    lightState = 2;
                }else if (Room.WallsPos.Contains(lightPos + Direction2D.CardinalDirectionList[3]))
                {
                    lightState = 3;
                }
                else
                {
                    Debug.Log("N-word");
                }

                if (debug)
                {
                    Debug.Log(".");
                }
                
                room.RoomLightState.Add(lightPos, lightState);
            }
            
            
            foreach (var pil in room.RoomPillarsPos)
            {
                Vector2Int p1 = new Vector2Int(pil.x + 2, pil.y);
                room.RoomLightPos.Add(p1);
                room.RoomLightState.Add(p1, 3);
                Vector2Int p2 = new Vector2Int(pil.x - 2, pil.y);
                room.RoomLightPos.Add(p2);
                room.RoomLightState.Add(p2, 1);
                Vector2Int p3 = new Vector2Int(pil.x, pil.y + 2);
                room.RoomLightPos.Add(p3);
                room.RoomLightState.Add(p3, 2);
                Vector2Int p4 = new Vector2Int(pil.x, pil.y - 2);
                room.RoomLightPos.Add(p4);
                room.RoomLightState.Add(p4, 0);
            }
        }
    }

    private void CreateBasicLight(List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            foreach (var pos in room.RoomLightPos)
            {
                int i = (int)room.RoomLightState[pos];
                Sprite light2DSprite = null;
                switch (i)
                {
                    case 0:
                        light2DSprite = torchTileTop;
                        break;
                    case 1:
                        light2DSprite = torchTileRight;
                        break;
                    case 2:
                        light2DSprite = torchTileBottom;
                        break;
                    case 3:
                        light2DSprite = torchTileLeft;
                        break;
                }
                PlaceLightObject(pos, light2DSprite, i);          
            }
        }
    }
    private void PlaceLightObject(Vector2Int pos, Sprite light2DSprite, int state)
    {
        var posV3 = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0);
        if (state == 0)
        {
            posV3.y++;
        }
        
        GameObject light2D = (GameObject) Instantiate(light2DPrefab, posV3, light2DParent.rotation, light2DParent);
        SpriteRenderer spriteRenderer = light2D.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = light2DSprite;
        Animator animator = light2D.GetComponent<Animator>();
        animator.SetInteger("lightState", state);
        Room.Lights.Add(light2D);
    }
}
