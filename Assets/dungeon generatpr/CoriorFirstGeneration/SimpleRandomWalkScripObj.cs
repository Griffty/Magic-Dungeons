using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkPar_", menuName = "Custom/SimpleRandomWalkData")]
public class SimpleRandomWalkScripObj : ScriptableObject
{
    public int iteration = 10, walkLength = 10;
    public int roomWalkRange, corridorWalkRange;
    public int corridorAmount, corridorLenght;
    public float roomSizeShift = 0.1f;
    public bool startRandomEachIteration = true;
    public bool needPuzzleRoom = true;
    public int decorationAmount = 10;
    public int lightAmount = 10;
    public int destroyableAmount = 10;
    public int pillarAmount = 1;
    
    public bool InUnity{get;  set;}
}
