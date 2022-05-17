using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomRoomDataGenerator : MonoBehaviour
{

    [Title("First Room")] 
    [SerializeField] private int[] startRoomXSizes = {3};
    [SerializeField] private int[] startRoomZSizes = {7};
    
    [Title("Hallway")] 
    [SerializeField] private int[] hallWayXSizes = {3, 5};
    [SerializeField] private int[] hallWayZSizes = {3, 5};
    
    [Title("Enemy Room")] 
    [SerializeField] private int[] enemyRoomXSizes = {5, 7, 9};
    [SerializeField] private int[] enemyRoomZSizes = {5, 7, 9};
    
    [Title("Puzzle Room")] 
    [SerializeField] private int[] puzzleRoomXSizes = {5, 7};
    [SerializeField] private int[] puzzleRoomZSizes = {3, 5};
    
    [Title("Boss Room")] 
    [SerializeField] private int[] bossRoomXSizes = {7};
    [SerializeField] private int[] bossRoomZSizes = {7};

    [SerializeField] private int[] numOfMainRoomSizes = {5,6,7};
    [SerializeField] private int initialEnemyCounter = 2;
    [SerializeField] private int enemyCounterIncreasePerRoom = 2;

    private Func<int> _xSizeRangeFirstRoom;
    private Func<int> _zSizeRangeFirstRoom;
    private Func<int> _xSizeRangeHallwayRoom;
    private Func<int> _zSizeRangeHallwayRoom;
    private Func<int> _xSizeRangeEnemyRoom;
    private Func<int> _zSizeRangeEnemyRoom;
    private Func<int> _xSizePuzzleRoom;
    private Func<int> _zSizePuzzleRoom;
    private Func<int> _xSizeBossRoom;
    private Func<int> _zSizeBossRoom;

    private void Start()
    {
        _xSizeRangeFirstRoom = () => GenerateRandomValue(startRoomXSizes);
        _zSizeRangeFirstRoom = () => GenerateRandomValue(startRoomZSizes);
        
        _xSizeRangeHallwayRoom = () => GenerateRandomValue(hallWayXSizes);
        _zSizeRangeHallwayRoom = () => GenerateRandomValue(hallWayZSizes);

        _xSizeRangeEnemyRoom = () => GenerateRandomValue(enemyRoomXSizes);
        _zSizeRangeEnemyRoom = () => GenerateRandomValue(enemyRoomZSizes);

        _xSizePuzzleRoom = () => GenerateRandomValue(puzzleRoomXSizes);
        _zSizePuzzleRoom = () => GenerateRandomValue(puzzleRoomZSizes);
        
        _xSizeBossRoom = () => GenerateRandomValue(bossRoomXSizes);
        _zSizeBossRoom = () => GenerateRandomValue(bossRoomZSizes);
    }

    private static int GenerateRandomValue(int[] possibleValues)
    {
        int randomIndex = Random.Range(0, possibleValues.Length);
        return possibleValues[randomIndex];
    }

    public List<RoomData> GenerateRoomData()
    {
        int numOfMainRooms = GenerateRandomValue(numOfMainRoomSizes);
        List<RoomData> roomData = new List<RoomData>();

        RoomData firstRoom = ScriptableObject.CreateInstance<RoomData>();
        firstRoom.name = "Room0";
        firstRoom.hasEntrance = false;
        firstRoom.hasExit = true;
        firstRoom.hasSpell = true;
        firstRoom.xSize = _xSizeRangeFirstRoom.Invoke();
        firstRoom.zSize = _zSizeRangeFirstRoom.Invoke();
        roomData.Add(firstRoom);

        RoomData prevRoom = firstRoom;
        RoomData prevHallwayRoom = firstRoom;
        int numOfEnemyRooms = 0;
        for (int i = 1; i < numOfMainRooms; i++)
        {
            RoomData newRoom = ScriptableObject.CreateInstance<RoomData>();
            newRoom.name = $"Room{i}";
            roomData.Add(newRoom);
            newRoom.hasEntrance = true;
            bool isLastRoom = i == numOfMainRooms - 1;
            newRoom.hasExit = !isLastRoom;

            if (isLastRoom)
            {
                GenerateBossRoom(newRoom, _xSizeBossRoom, _zSizeBossRoom);
            }
            else if (prevRoom.hasSpell || prevRoom.hasItem)
            {
                GenerateEnemyRoom(newRoom, numOfEnemyRooms, _xSizeRangeEnemyRoom, _zSizeRangeEnemyRoom);
                numOfEnemyRooms++;
            }
            else
            {
                prevHallwayRoom = GenerateRewardsRoom(prevHallwayRoom, newRoom, _xSizeRangeHallwayRoom, _zSizeRangeHallwayRoom);
            }

            if (newRoom.hasEnemies)
            {
                GenerateSideRooms(newRoom, _xSizePuzzleRoom, _zSizePuzzleRoom);
            }
            
            prevRoom = newRoom;
        }

        return roomData;
    }

    private static void GenerateBossRoom(RoomData newRoom, Func<int> xSizeBossRoom, Func<int> zSizeBossRoom)
    {
        newRoom.isBossRoom = true;
        newRoom.xSize = xSizeBossRoom.Invoke();
        newRoom.zSize = zSizeBossRoom.Invoke();
    }

    private void GenerateEnemyRoom(RoomData newRoom, int numOfEnemyRooms, Func<int> xSizeRangeEnemyRoom,
        Func<int> zSizeRangeEnemyRoom)
    {
        newRoom.hasEnemies = true;
        newRoom.numberOfEnemies = initialEnemyCounter + (numOfEnemyRooms * enemyCounterIncreasePerRoom);
        newRoom.xSize = xSizeRangeEnemyRoom.Invoke();
        newRoom.zSize = zSizeRangeEnemyRoom.Invoke();
    }

    private static RoomData GenerateRewardsRoom(RoomData prevHallwayRoom, RoomData newRoom, Func<int> xSizeRangeHallwayRoom,
        Func<int> zSizeRangeHallwayRoom)
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
        return prevHallwayRoom;
    }

    private static void GenerateSideRooms(RoomData newRoom, Func<int> xSizePuzzleRoom, Func<int> zSizePuzzleRoom)
    {
        bool doesHaveRightSideRoom = 0 == Random.Range(0, 2);
        bool doesHaveLeftSideRoom = 0 == Random.Range(0, 2);
        if (doesHaveLeftSideRoom)
        {
            newRoom.hasLeftSideRoom = true;
            newRoom.leftSideRoomData = GenerateRandomSideRoom(xSizePuzzleRoom, zSizePuzzleRoom);
            newRoom.leftSideRoomData.name = $"{newRoom.name}-LeftSideRoom";
        }

        if (doesHaveRightSideRoom)
        {
            newRoom.hasRightSideRoom = true;
            newRoom.rightSideRoomData = GenerateRandomSideRoom(xSizePuzzleRoom, zSizePuzzleRoom);
            newRoom.rightSideRoomData.name = $"{newRoom.name}-RightSideRoom";
        }
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