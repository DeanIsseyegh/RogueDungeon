using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private GameObject player;

    [SerializeField] private List<RoomData> roomsData;

    void Start()
    {
        GenerateRooms();
    }

    public void GenerateRooms()
    {
        List<GeneratedRoom> generatedRooms = new List<GeneratedRoom>();
        
        GeneratedRoom firstRoom = roomGenerator
            .GenerateFloor(startingPos, roomsData[0]);
        generatedRooms.Add(firstRoom);
        GeneratedRoom prevRoom = firstRoom;
        for (var i = 1; i < roomsData.Count; i++)
        {
            var newRoomStartPos = roomGenerator.CalculateStartPosBasedOnPrevRoom(prevRoom, roomsData[i]);
            var newRoom = roomGenerator.GenerateFloor(newRoomStartPos, roomsData[i]);
            generatedRooms.Add(newRoom);
            prevRoom = newRoom;
        }
        
        generatedRooms.ForEach(room => room.BuildNavmesh());
    }

}