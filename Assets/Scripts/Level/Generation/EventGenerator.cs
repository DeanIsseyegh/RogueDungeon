using System.Collections.Generic;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    public void GenerateEvent(RoomData roomData, GameObject roomParent,
        RoomGenerator.EntranceLocation entranceLocation, GeneratedRoom generatedRoom)
    {
        if (roomData.hasItem | roomData.hasSpell)
            GenerateCollectibleEvent(roomData, roomParent, entranceLocation, generatedRoom);
        else if (roomData.hasEnemies)
            GenerateEnemyEvent(roomData, generatedRoom, roomParent, entranceLocation);
    }

    private void GenerateEnemyEvent(RoomData roomData, GeneratedRoom generatedRoom, GameObject roomParent, RoomGenerator.EntranceLocation entranceLocation)
    {
        GameObject newObj = new GameObject("RoomTriggerGenerated");
        GameObject emptyGameObj = Instantiate(newObj, Vector3.zero, Quaternion.identity, roomParent.transform);
        Destroy(newObj);
        
        Vector3 roomZLength = new Vector3(0, 0, roomData.zSize * generatedRoom.ZTileSize / 2);
        Vector3 middleOfRoom = entranceLocation.WithoutOffset + roomZLength;
        BoxCollider boxCollider = emptyGameObj.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.center = middleOfRoom;
        boxCollider.size = new Vector3(roomData.xSize * generatedRoom.XTileSize, 1,
            (roomData.zSize - 1) * generatedRoom.ZTileSize);

        int enemiesToGenerate = roomData.numberOfEnemies;
        List<List<Vector3>> mapLayout = generatedRoom.MapLayout;
        int randomXPos = Random.Range(0, generatedRoom.XSize);
        int randomZPos = Random.Range(0, generatedRoom.ZSize);

        List<Vector3> spawnPositions = new List<Vector3>();
        Vector3 spawnPos = mapLayout[randomZPos][randomXPos];
        spawnPositions.Add(spawnPos);
        
        EnemyRoomStartEvent startEvent = emptyGameObj.AddComponent<EnemyRoomStartEvent>();
        startEvent.EnemyPositions = spawnPositions;
        RoomEndEvent roomEndEvent = emptyGameObj.AddComponent<RoomEndEvent>();
        roomEndEvent.isRoomComplete = () => startEvent.HasEventFinished();
        roomEndEvent.onRoomComplete = () =>
        {
            generatedRoom.Exit.GetComponentInChildren<Open>().enabled = true;
            startEvent.enabled = false;
            roomEndEvent.enabled = false;
        };
    }

    private void GenerateCollectibleEvent(RoomData roomData, GameObject roomParent,
        RoomGenerator.EntranceLocation entranceLocation, GeneratedRoom generatedRoom)
    {
        GameObject newObj = new GameObject("RoomTriggerGenerated");
        GameObject emptyGameObj = Instantiate(newObj, Vector3.zero, Quaternion.identity, roomParent.transform);
        Destroy(newObj);

        Vector3 roomZLength = new Vector3(0, 0, roomData.zSize * generatedRoom.ZTileSize / 2);
        Vector3 middleOfRoom = entranceLocation.WithoutOffset + roomZLength;
        BoxCollider boxCollider = emptyGameObj.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.center = middleOfRoom;
        boxCollider.size = new Vector3(roomData.xSize * generatedRoom.XTileSize, 1,
            (roomData.zSize - 1) * generatedRoom.ZTileSize);

        CollectibleRoomStartEvent startEvent = AddRoomStartEvent(roomData, emptyGameObj);
        startEvent.MiddleOfRoomPos = middleOfRoom;

        RoomEndEvent roomEndEvent = emptyGameObj.AddComponent<RoomEndEvent>();
        roomEndEvent.isRoomComplete = () => startEvent.HasEventFinished();
        roomEndEvent.onRoomComplete = () =>
        {
            generatedRoom.Exit.GetComponentInChildren<Open>().enabled = true;
            startEvent.enabled = false;
            roomEndEvent.enabled = false;
        };
    }

    private static CollectibleRoomStartEvent AddRoomStartEvent(RoomData roomData, GameObject emptyGameObj)
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