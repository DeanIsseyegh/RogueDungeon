using System.Collections.Generic;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    [SerializeField] private GameObject closedEntranceTile;

    public void GenerateEvent(RoomData roomData, GameObject roomParent,
        RoomGenerator.EntranceLocation entranceLocation, GeneratedRoom generatedRoom, GeneratedRoom previousRoom)
    {
        if (roomData.hasItem | roomData.hasSpell)
            GenerateCollectibleEvent(roomData, roomParent, entranceLocation, generatedRoom, previousRoom);
        else if (roomData.hasEnemies)
            GenerateEnemyEvent(roomData, generatedRoom, roomParent, entranceLocation);
    }

    private void GenerateEnemyEvent(RoomData roomData, GeneratedRoom generatedRoom, GameObject roomParent,
        RoomGenerator.EntranceLocation entranceLocation)
    {
        Vector3 middleOfRoom = CalculateMiddleOfRoom(roomData, entranceLocation, generatedRoom);
        var emptyGameObj =
            CreateTriggerInRoom(roomData, generatedRoom, roomParent, entranceLocation, middleOfRoom);
        List<List<Vector3>> mapLayout = generatedRoom.MapLayout;

        int enemiesToGenerate = roomData.numberOfEnemies;

        List<Vector3> spawnPositions = new List<Vector3>();
        for (int i = 0; i < enemiesToGenerate; i++)
        {
            int randomXPos = Random.Range(0, generatedRoom.XSize);
            int randomZPos = Random.Range(0, generatedRoom.ZSize);

            Vector3 spawnPos = mapLayout[randomZPos][randomXPos];
            spawnPositions.Add(spawnPos);
        }

        EnemyRoomStartEvent startEvent = emptyGameObj.AddComponent<EnemyRoomStartEvent>();
        startEvent.EnemyPositions = spawnPositions;
        startEvent.ClosedEntranceTile = closedEntranceTile;
        startEvent.EntrancePos = entranceLocation.WithOffset;
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
        RoomGenerator.EntranceLocation entranceLocation, GeneratedRoom generatedRoom, GeneratedRoom previousRoom)
    {
        Vector3 middleOfRoom = CalculateMiddleOfRoom(roomData, entranceLocation, generatedRoom);
        var emptyGameObj =
            CreateTriggerInRoom(roomData, generatedRoom, roomParent, entranceLocation, middleOfRoom);

        CollectibleRoomStartEvent startEvent = AddRoomStartEvent(roomData, emptyGameObj);
        startEvent.MiddleOfRoomPos = middleOfRoom;
        if (previousRoom != null)
        {
            startEvent.ClosedEntranceTile = closedEntranceTile;
            startEvent.EntrancePos = entranceLocation.WithOffset;
            startEvent.EntranceRoomDoor = previousRoom.Exit;
        }

        RoomEndEvent roomEndEvent = emptyGameObj.AddComponent<RoomEndEvent>();
        roomEndEvent.isRoomComplete = () => startEvent.HasEventFinished();
        roomEndEvent.onRoomComplete = () =>
        {
            startEvent.RemoveCollectibles();
            startEvent.HideChoiceUi();
            startEvent.enabled = false;
            generatedRoom.Exit.GetComponentInChildren<Open>().enabled = true;
            roomEndEvent.enabled = false;
        };
    }

    private static Vector3 CalculateMiddleOfRoom(RoomData roomData,
        RoomGenerator.EntranceLocation entranceLocation,
        GeneratedRoom generatedRoom)
    {
        Vector3 roomZLength = new Vector3(0, 0, roomData.zSize * generatedRoom.ZTileSize / 2);
        Vector3 middleOfRoom = entranceLocation.WithoutOffset + roomZLength;
        return middleOfRoom;
    }

    private GameObject CreateTriggerInRoom(RoomData roomData, GeneratedRoom generatedRoom,
        GameObject roomParent, RoomGenerator.EntranceLocation entranceLocation, Vector3 middleOfRoom)
    {
        GameObject newObj = new GameObject("RoomTriggerGenerated");
        GameObject emptyGameObj = Instantiate(newObj, Vector3.zero, Quaternion.identity, roomParent.transform);
        Destroy(newObj);

        BoxCollider boxCollider = emptyGameObj.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.center = middleOfRoom;
        boxCollider.size = new Vector3(roomData.xSize * generatedRoom.XTileSize, 1,
            (roomData.zSize - 1) * generatedRoom.ZTileSize);
        return emptyGameObj;
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