using System;
using System.Collections.Generic;
using Level.RoomEvents;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class EventGenerator : MonoBehaviour
{
    [SerializeField] private GameObject closedEntranceTile;

    protected static List<Vector3> GenerateEnemySpawnPos(GeneratedRoom generatedRoom, int enemiesToGenerate, List<List<Vector3>> mapLayout)
    {
        List<Vector3> spawnPositions = new List<Vector3>();
        for (int i = 0; i < enemiesToGenerate; i++)
        {
            int randomXPos = Random.Range(0, generatedRoom.XSize);
            int randomZPos = Random.Range(0, generatedRoom.ZSize);

            Vector3 spawnPos = mapLayout[randomZPos][randomXPos];
            spawnPositions.Add(spawnPos);
        }
        return spawnPositions;
    }

    protected void ShutEntrance(RoomGenerator.EntranceLocation entranceLocation, GeneratedRoom previousRoom,
        RoomStartEvent startEvent)
    {
        if (previousRoom != null)
        {
            startEvent.ClosedEntranceTile = closedEntranceTile;
            startEvent.EntrancePos = entranceLocation.WithOffset;
            startEvent.EntranceRoomDoor = previousRoom.Exit;
        }
    }

    protected GameObject CreateTriggerInRoom(GeneratedRoom generatedRoom)
    {
        GameObject newObj = new GameObject("RoomTriggerGenerated");
        GameObject emptyGameObj = Instantiate(newObj, Vector3.zero, Quaternion.identity, generatedRoom.RoomParent.transform);
        Destroy(newObj);

        BoxCollider boxCollider = emptyGameObj.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.center = generatedRoom.MiddleOfRoom;
        boxCollider.size = new Vector3(generatedRoom.XSize * generatedRoom.XTileSize, 1,
            (generatedRoom.ZSize - 1) * generatedRoom.ZTileSize);
        return emptyGameObj;
    }

    protected static CollectibleRoomStartEvent AddRoomStartEvent(RoomData roomData, GameObject emptyGameObj)
    {
        if (roomData.hasSpell)
        {
            return emptyGameObj.AddComponent<SpellRoomStartEvent>();
        }
        else
        {
            return emptyGameObj.AddComponent<ItemRoomStartEvent>();
        }
    }
}