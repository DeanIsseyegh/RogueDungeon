using System.Collections.Generic;
using Level.RoomEvents;
using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    [SerializeField] private GameObject closedEntranceTile;
    [SerializeField] private GameObject balloonEventWall;
    [SerializeField] private GameObject balloon;

    public void GenerateEvent(GeneratedRoom generatedRoom, GeneratedRoom previousRoom,
        List<GameObject> sideExits)
    {
        RoomData roomData = generatedRoom.RoomData;
        if (roomData.hasItem | roomData.hasSpell)
            GenerateCollectibleEvent(generatedRoom, previousRoom, sideExits);
        else if (roomData.hasEnemies)
            GenerateEnemyEvent(generatedRoom, previousRoom, sideExits);
    }

    public void GenerateSideEvent(GeneratedRoom generatedRoom, bool isRightSideRoom)
    {
        RoomData roomData = generatedRoom.RoomData;
        if (roomData.isPuzzleRoom && roomData.isBalloonRoom)
        {
            GeneratePuzzleEvent(generatedRoom, isRightSideRoom);
        }
    }

    private void GeneratePuzzleEvent(GeneratedRoom generatedRoom, bool isRightSideRoom)
    {
        Vector3 zTile = new Vector3(generatedRoom.ZTileSize, 0, 0);
        Vector3 startPuzzleWall = generatedRoom.MiddleOfRoom + (zTile * generatedRoom.XSize);
        
        for (var i = 0; i <  generatedRoom.ZSize; i++)
        {
            Vector3 tile =  generatedRoom.MapLayout[i][generatedRoom.ZSize/2];
            Instantiate(balloonEventWall, tile, balloonEventWall.transform.rotation);
        }

        for (int i = 0; i < 3; i++)
        {
            Vector3 balloonsSpawnPos = generatedRoom.MapLayout[i][1];
            Instantiate(balloon, balloonsSpawnPos + Vector3.up * (2 + i), balloon.transform.localRotation);
        }
        
    }

    private void GenerateEnemyEvent(GeneratedRoom generatedRoom, GeneratedRoom previousRoom,
        List<GameObject> sideExits)
    {
        var emptyGameObj = CreateTriggerInRoom(generatedRoom);
        List<List<Vector3>> mapLayout = generatedRoom.MapLayout;

        int enemiesToGenerate = generatedRoom.RoomData.numberOfEnemies;
        List<Vector3> spawnPositions = GenerateEnemySpawnPos(generatedRoom, enemiesToGenerate, mapLayout);
        EnemyRoomStartEvent startEvent = emptyGameObj.AddComponent<EnemyRoomStartEvent>();
        startEvent.EnemyPositions = spawnPositions;
        ShutEntrance(generatedRoom.EntranceLocation, previousRoom, startEvent);

        RoomEndEvent roomEndEvent = emptyGameObj.AddComponent<RoomEndEvent>();
        roomEndEvent.isRoomComplete = () => startEvent.HasEventFinished();
        roomEndEvent.onRoomComplete = () =>
        {
            generatedRoom.Exit.GetComponentInChildren<Open>().enabled = true;
            sideExits.ForEach( it => it.GetComponentInChildren<Open>().enabled = true);
            startEvent.enabled = false;
            roomEndEvent.enabled = false;
        };
    }

    private static List<Vector3> GenerateEnemySpawnPos(GeneratedRoom generatedRoom, int enemiesToGenerate, List<List<Vector3>> mapLayout)
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

    private void GenerateCollectibleEvent(GeneratedRoom generatedRoom, GeneratedRoom previousRoom,
        List<GameObject> sideExits)
    {
        var emptyGameObj = CreateTriggerInRoom(generatedRoom);

        CollectibleRoomStartEvent startEvent = AddRoomStartEvent(generatedRoom.RoomData, emptyGameObj);
        ShutEntrance(generatedRoom.EntranceLocation, previousRoom, startEvent);
        startEvent.MiddleOfRoomPos = generatedRoom.MiddleOfRoom;

        RoomEndEvent roomEndEvent = emptyGameObj.AddComponent<RoomEndEvent>();
        roomEndEvent.isRoomComplete = () => startEvent.HasEventFinished();
        roomEndEvent.onRoomComplete = () =>
        {
            startEvent.RemoveCollectibles();
            startEvent.HideChoiceUi();
            startEvent.enabled = false;
            generatedRoom.Exit.GetComponentInChildren<Open>().enabled = true;
            sideExits.ForEach( it => it.GetComponentInChildren<Open>().enabled = true);
            roomEndEvent.enabled = false;
        };
    }

    private void ShutEntrance(RoomGenerator.EntranceLocation entranceLocation, GeneratedRoom previousRoom,
        RoomStartEvent startEvent)
    {
        if (previousRoom != null)
        {
            startEvent.ClosedEntranceTile = closedEntranceTile;
            startEvent.EntrancePos = entranceLocation.WithOffset;
            startEvent.EntranceRoomDoor = previousRoom.Exit;
        }
    }

    private GameObject CreateTriggerInRoom(GeneratedRoom generatedRoom)
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