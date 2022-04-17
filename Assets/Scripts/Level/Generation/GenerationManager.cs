using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private RoomGenerator roomGenerator;
    [SerializeField] private List<RoomData> roomsData;

    private bool hasGenerationStarted;

    void Start()
    {
        GenerateRandomRooms();
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
            generatedRooms.Add(newRoom);
            prevRoom = newRoom;
        }

        generatedRooms.ForEach(room => room.BuildNavmesh());
    }

    //Todo refactor this - just prototype for now
    public void GenerateRandomRooms()
    {
        int numOfMainRooms = Random.Range(5, 9);
        int initialEnemyCounter = 2;
        int enemyCounterIncreasePerRoom = 2;

        List<RoomData> roomData = new List<RoomData>();

        Func<int> sizeRangeHallwayRoom = () =>
        {
            int[] possibleSizes = {3, 5};
            int randomIndex = Random.Range(0, possibleSizes.Length);
            return possibleSizes[randomIndex];
        };

        Func<int> sizeRangeEnemyRoom = () =>
        {
            int[] possibleSizes = {5, 7, 9};
            int randomIndex = Random.Range(0, possibleSizes.Length);
            return possibleSizes[randomIndex];
        };
        
        Func<int> xSizePuzzleRoom = () =>
        {
            int[] possibleSizes = {5, 7};
            int randomIndex = Random.Range(0, possibleSizes.Length);
            return possibleSizes[randomIndex];
        };
        
        Func<int> zSizePuzzleRoom = () =>
        {
            int[] possibleSizes = {3, 5};
            int randomIndex = Random.Range(0, possibleSizes.Length);
            return possibleSizes[randomIndex];
        };


        RoomData firstRoom = ScriptableObject.CreateInstance<RoomData>();
        firstRoom.hasEntrance = false;
        firstRoom.hasExit = true;
        firstRoom.hasSpell = true;
        firstRoom.xSize = sizeRangeHallwayRoom.Invoke();
        firstRoom.zSize = sizeRangeHallwayRoom.Invoke();
        roomData.Add(firstRoom);

        RoomData prevRoom = firstRoom;
        RoomData prevHallwayRoom = firstRoom;
        int numOfEnemyRooms = 0;
        for (int i = 1; i < numOfMainRooms; i++)
        {
            // Main Rooms
            RoomData newRoom = ScriptableObject.CreateInstance<RoomData>();
            roomData.Add(newRoom);
            newRoom.hasEntrance = true;
            newRoom.hasExit = i != numOfMainRooms - 1;
            if (prevRoom.hasSpell || prevRoom.hasItem)
            {
                newRoom.hasEnemies = true;
                newRoom.numberOfEnemies = initialEnemyCounter + (numOfEnemyRooms++ * enemyCounterIncreasePerRoom);
                newRoom.xSize = sizeRangeEnemyRoom.Invoke();
                newRoom.zSize = sizeRangeEnemyRoom.Invoke();
            }
            else
            {
                if (prevHallwayRoom.hasItem)
                {
                    newRoom.hasSpell = true;
                }
                else
                {
                    newRoom.hasItem = true;
                }

                newRoom.xSize = sizeRangeHallwayRoom.Invoke();
                newRoom.zSize = sizeRangeHallwayRoom.Invoke();
                prevHallwayRoom = newRoom;
            }

            // Side Rooms
            if (newRoom.hasEnemies)
            {
                bool doesHaveRightSideRoom = 0 == Random.Range(0, 2);
                bool doesHaveLeftSideRoom = 0 == Random.Range(0, 2);
                if (doesHaveLeftSideRoom)
                {
                    newRoom.hasLeftSideRoom = true;
                    newRoom.leftSideRoomData = GenerateRandomSideRoom(xSizePuzzleRoom, zSizePuzzleRoom);
                }
            
                if (doesHaveRightSideRoom)
                {
                    newRoom.hasRightSideRoom = true;
                    newRoom.rightSideRoomData = GenerateRandomSideRoom(xSizePuzzleRoom, zSizePuzzleRoom);
                }
            }
            
            prevRoom = newRoom;
        }

        GenerateRooms(roomData);
    }

    private static RoomData GenerateRandomSideRoom(Func<int> xSizePuzzleRoom, Func<int> zSizePuzzleRoom)
    {
        RoomData newSideRoom = ScriptableObject.CreateInstance<RoomData>();
        newSideRoom.isPuzzleRoom = true;
        newSideRoom.isBalloonRoom = 0 == Random.Range(0, 2);
        newSideRoom.isSkullRoom = !newSideRoom.isBalloonRoom;
        newSideRoom.xSize = xSizePuzzleRoom.Invoke();
        newSideRoom.zSize = zSizePuzzleRoom.Invoke();
        newSideRoom.hasEntrance = true;
        newSideRoom.hasExit = false;
        return newSideRoom;
    }
}