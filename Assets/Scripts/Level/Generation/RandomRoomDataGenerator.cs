using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomRoomDataGenerator : MonoBehaviour
{

    [Title("Hallway")] 
    [SerializeField] private int[] hallWayXSizes = {3, 5};
    [SerializeField] private int[] hallWayZSizes = {3, 5};
    
    [Title("Enemy Room")] 
    [SerializeField] private int[] enemyRoomXSizes = {5, 7, 9};
    [SerializeField] private int[] enemyRoomZSizes = {5, 7, 9};
    
    [Title("Puzzle Room")] 
    [SerializeField] private int[] puzzleRoomXSizes = {5, 7};
    [SerializeField] private int[] puzzleRoomZSizes = {3, 5};

    [SerializeField] private int[] numOfMainRoomSizes = {5,6,7};
    [SerializeField] private int initialEnemyCounter = 2;
    [SerializeField] private int enemyCounterIncreasePerRoom = 2;

    private static int GenerateRandomValue(int[] possibleValues)
    {
        int randomIndex = Random.Range(0, possibleValues.Length);
        return possibleValues[randomIndex];
    }

    public List<RoomData> GenerateRoomData()
    {
        int numOfMainRooms = GenerateRandomValue(numOfMainRoomSizes);
        List<RoomData> roomData = new List<RoomData>();

        Func<int> xSizeRangeHallwayRoom = () => GenerateRandomValue(hallWayXSizes);
        Func<int> zSizeRangeHallwayRoom = () => GenerateRandomValue(hallWayZSizes);

        Func<int> xSizeRangeEnemyRoom = () => GenerateRandomValue(enemyRoomXSizes);
        Func<int> zSizeRangeEnemyRoom = () => GenerateRandomValue(enemyRoomZSizes);

        Func<int> xSizePuzzleRoom = () => GenerateRandomValue(puzzleRoomXSizes);
        Func<int> zSizePuzzleRoom = () => GenerateRandomValue(puzzleRoomZSizes);

        RoomData firstRoom = ScriptableObject.CreateInstance<RoomData>();
        firstRoom.hasEntrance = false;
        firstRoom.hasExit = true;
        firstRoom.hasSpell = true;
        firstRoom.xSize = xSizeRangeHallwayRoom.Invoke();
        firstRoom.zSize = zSizeRangeHallwayRoom.Invoke();
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
                newRoom.xSize = xSizeRangeEnemyRoom.Invoke();
                newRoom.zSize = zSizeRangeEnemyRoom.Invoke();
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

                newRoom.xSize = xSizeRangeHallwayRoom.Invoke();
                newRoom.zSize = zSizeRangeHallwayRoom.Invoke();
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

        return roomData;
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