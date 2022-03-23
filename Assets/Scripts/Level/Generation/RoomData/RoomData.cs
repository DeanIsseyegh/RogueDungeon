using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Room", menuName = "Room Data")]
public class RoomData : ScriptableObject
{
    public float xSize;
    public float zSize;
    public bool hasSpell;
    public bool hasItem;
    public bool hasEnemies;
    public int numberOfEnemies = 0; 
    public bool hasEntrance = true;
    public bool hasExit = true;
    
    public bool hasRightSideRoom;
    public RoomData rightSideRoomData;
    
    public bool hasLeftSideRoom;
    public RoomData leftSideRoomData;
    public bool isPuzzleRoom;
    public bool isBalloonRoom;
}
