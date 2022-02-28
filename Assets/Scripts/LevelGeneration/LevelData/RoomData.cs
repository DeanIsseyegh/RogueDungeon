using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Room Data")]
public class RoomData : ScriptableObject
{
    public float xSize;
    public float zSize;
    public bool hasItems;
    public bool hasEnemies;
}
