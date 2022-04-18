using System.Collections.Generic;
using System.Linq;
using Level.Generation;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private RandomRoomDataGenerator randomRoomDataGenerator;
    [SerializeField] private WallDecorationsGenerator wallDecorationsGenerator;
    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private List<RoomData> testRoomData;
    [SerializeField] private bool useTestData = false;

    void Start()
    {
        if (useTestData)
        {
            GenerateRooms(testRoomData);
        }
        else
        {
            List<RoomData> roomData = randomRoomDataGenerator.GenerateRoomData();
            GenerateRooms(roomData);
        }
    }

    public void GenerateRooms(List<RoomData> roomsData)
    {
        List<GeneratedRoom> generatedRooms = new List<GeneratedRoom>();
        GeneratedRoom firstRoom = roomGenerator
            .GenerateRoom(startingPos, roomsData.First(), null);
        generatedRooms.Add(firstRoom);
        GeneratedRoom prevRoom = firstRoom;
        for (var i = 1; i < roomsData.Count; i++)
        {
            var newRoomStartPos = roomGenerator.CalculateStartPosBasedOnPrevRoom(prevRoom, roomsData[i]);
            var newRoom = roomGenerator.GenerateRoom(newRoomStartPos, roomsData[i], prevRoom);
            wallDecorationsGenerator.Generate(newRoom);
            generatedRooms.Add(newRoom);
            prevRoom = newRoom;
        }

        generatedRooms.Last().BuildNavmesh();
    }
}