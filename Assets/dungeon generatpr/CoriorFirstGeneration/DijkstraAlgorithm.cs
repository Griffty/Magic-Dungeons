using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DijkstraAlgorithm 
{
   // public static void SetRoomTypes(List<Room> rooms, HashSet<Vector2Int> roomPos, HashSet<Vector2Int> corridorPos, SimpleRandomWalkScripObj parameters)
   // {
   //    StartDijkstraAlgorithm(rooms, roomPos, corridorPos, parameters.corridorLenght);
   //    if (rooms.Count>3)
   //    {
   //       SetRoomTypes(rooms, parameters);
   //    }
   // }
   // private static void StartDijkstraAlgorithm(List<Room> rooms, HashSet<Vector2Int> roomPos, HashSet<Vector2Int> corridorPos, int corridorLength)
   // {
   //    int distance = 0;
   //    HashSet<Vector2Int> usedRoomPos = new HashSet<Vector2Int>();
   //    Queue<Vector2Int> roomCenterToCheck = new Queue<Vector2Int>();
   //    roomCenterToCheck.Enqueue(Vector2Int.zero);
   //
   //    while (roomCenterToCheck.Count>0)
   //    {
   //       List<Queue<Vector2Int>> newPointsToCheckList = new List<Queue<Vector2Int>>();
   //       while (roomCenterToCheck.Count>0)
   //       {
   //          var center = roomCenterToCheck.Dequeue();
   //          usedRoomPos.Add(center);
   //          newPointsToCheckList.Add(CheckRoom(center, rooms, roomPos, corridorPos, usedRoomPos, distance, corridorLength));
   //       }
   //       foreach (var pointToCheck in newPointsToCheckList)
   //       {
   //          foreach (var pos in pointToCheck)
   //          {
   //             roomCenterToCheck.Enqueue(pos);
   //          }
   //       }
   //       distance++;
   //    }
   // }
   //
   // private static Queue<Vector2Int> CheckRoom(Vector2Int center, List<Room> rooms, HashSet<Vector2Int> roomPos, HashSet<Vector2Int> corridorPos, HashSet<Vector2Int> usedRoomPos, int distance, int corridorLength)
   // {
   //    Queue<Vector2Int> newPointsToCheck = new Queue<Vector2Int>();
   //    Room room = Room.FindRoomByCenter(rooms, center);
   //    room.RoomDanger = distance;
   //
   //    foreach (var dir in Direction2D.CardinalDirectionList)
   //    {
   //       var pos = center + dir * corridorLength;
   //       if (!usedRoomPos.Contains(pos))
   //       {
   //          if (roomPos.Contains(pos))
   //          {
   //             if (IsReachable(center, dir, corridorPos, corridorLength))
   //             {
   //                newPointsToCheck.Enqueue(pos);
   //             }
   //          }  
   //       }
   //    }
   //
   //    return newPointsToCheck;
   // }
   
   private static bool IsReachable(Vector2Int center, Vector2Int dir, HashSet<Vector2Int> corridorPos, int corridorLength)
   {
      Vector2Int start = new Vector2Int(center.x, center.y);
      for (int i = 1; i < corridorLength; i++)
      {
         if (!corridorPos.Contains(start + dir * i))
         {
            return false;
         }
      }
      return true;
   }
   public static void SetRoomTypes(List<Room> rooms, SimpleRandomWalkScripObj parameters)
   {
      Room startRoom = Room.FindRoomByCenter(rooms, Vector2Int.zero);
      startRoom.RoomType = Room.AllRoomTypes.SpawnRoom;
      Room bossRoom = Room.GetBossRoom(rooms);
      if (parameters.needPuzzleRoom)
      {
         Room puzzleRoom;
         while (true)
         {
            int i = Random.Range(0, rooms.Count - 1);
            if (rooms[i] != startRoom && rooms[i] != bossRoom)
            {
               puzzleRoom = rooms[i];
               break;
            }
         }
         puzzleRoom.RoomType = Room.AllRoomTypes.PuzzleRoom;
      }
   }

   public static void GetRoomDangerFromCenter(Hashtable roomCenterToDanger, HashSet<Vector2Int> possibleRoomPos, HashSet<Vector2Int> corridorPos, SimpleRandomWalkScripObj parameters)
   {
      StartDijkstraAlgorithm(roomCenterToDanger, possibleRoomPos, corridorPos, parameters.corridorLenght);
   }

   private static void StartDijkstraAlgorithm(Hashtable roomCenterToDanger, HashSet<Vector2Int> possibleRoomPos, HashSet<Vector2Int> corridorPos, int corridorLength)
   {
      int distance = 0;
      HashSet<Vector2Int> usedRoomPos = new HashSet<Vector2Int>();
      Queue<Vector2Int> roomCenterToCheck = new Queue<Vector2Int>();
      roomCenterToCheck.Enqueue(Vector2Int.zero);

      while (roomCenterToCheck.Count>0)
      {
         List<Queue<Vector2Int>> newPointsToCheckList = new List<Queue<Vector2Int>>();
         while (roomCenterToCheck.Count>0)
         {
            var center = roomCenterToCheck.Dequeue();
            
            newPointsToCheckList.Add(CheckRoom(center, roomCenterToDanger, possibleRoomPos, corridorPos, usedRoomPos, distance, corridorLength));
            usedRoomPos.Add(center);
         }
         foreach (var pointToCheck in newPointsToCheckList)
         {
            foreach (var pos in pointToCheck)
            {
               roomCenterToCheck.Enqueue(pos);
            }
         }
         distance++;
      }
   }

   private static Queue<Vector2Int> CheckRoom(Vector2Int center, Hashtable roomCenterToDanger, HashSet<Vector2Int> possibleRoomPos, HashSet<Vector2Int> corridorPos, HashSet<Vector2Int> usedRoomPos, int distance, int corridorLength)
   {
      Queue<Vector2Int> newPointsToCheck = new Queue<Vector2Int>();
      if (roomCenterToDanger[center] == null)
      {
         roomCenterToDanger.Add(center, distance);
      }

      foreach (var dir in Direction2D.CardinalDirectionList)
      {
         var pos = center + dir * corridorLength;
         if (!usedRoomPos.Contains(pos))
         {
            if (possibleRoomPos.Contains(pos))
            {
               if (IsReachable(center, dir, corridorPos, corridorLength))
               {
                  newPointsToCheck.Enqueue(pos);
               }
            }  
         }
      }

      return newPointsToCheck;
   }
}

