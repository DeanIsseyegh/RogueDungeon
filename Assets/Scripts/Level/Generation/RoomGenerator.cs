using System;
using System.Collections.Generic;
using System.Linq;
using Level.Generation;
using Level.Generation.Floor;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private FloorTilePicker floorTilePicker;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject entranceTileWithDoor;
    [SerializeField] private GameObject entranceTileWithoutDoor;
    [SerializeField] private GameObject levelParent;
    [SerializeField] private EventGenerationManager eventGenerationManager;

    private Vector3 _xTileSize;
    private Vector3 _zTileSize;
    private Vector3 _tileSize;

    private void Start()
    {
        MeshRenderer floorRenderer = floorTilePicker.PickFloorTile().GetComponent<MeshRenderer>();
        Vector3 floorSize = floorRenderer.bounds.size;
        _xTileSize = new Vector3(floorSize.x, 0, 0);
        _zTileSize = new Vector3(0, 0, floorSize.z);
        _tileSize = new Vector3(floorSize.x, 0, floorSize.z);
    }

    public GeneratedRoom GenerateRoom(Vector3 startingPos, RoomData roomData, GeneratedRoom previousRoom)
    {
        GameObject roomObj = new GameObject(roomData.name);
        GameObject roomParent = Instantiate(roomObj, levelParent.transform);
        Destroy(roomObj);

        float xSize = roomData.xSize;
        float zSize = roomData.zSize;

        List<List<Vector3>> mapLayout = CreateMapLayout(startingPos, xSize, zSize);
        List<NavMeshSurface> generatedFloor = CreateFloor(mapLayout, roomParent);

        EntranceLocation entranceLocation = EntranceLocation.None();
        GameObject entrance = null;
        if (roomData.hasEntrance)
        {
            entranceLocation = new EntranceLocation(mapLayout, _zTileSize);
            entrance = CreateEntrance(entranceLocation, roomParent);
        }

        ExitLocation exitLocation = ExitLocation.None();
        GameObject exit = null;
        if (roomData.hasExit)
        {
            exitLocation = new ExitLocation(mapLayout, _zTileSize);
            exit = CreateExit(exitLocation, roomParent);
        }

        List<GameObject> sideExits = new List<GameObject>();

        RightSideExitLocation rightSideExitLocation = RightSideExitLocation.None();
        GeneratedRoom rightSideRoom = null;
        if (roomData.hasRightSideRoom)
        {
            rightSideExitLocation = new RightSideExitLocation(mapLayout, _xTileSize);
            GameObject rightSideExit = CreateRightSideExit(rightSideExitLocation, roomParent);
            sideExits.Add(rightSideExit);
            Vector3 sideRoomStartingPos = rightSideExitLocation.WithoutOffset +
                                          -(roomData.rightSideRoomData.xSize * _xTileSize) +
                                          -(roomData.rightSideRoomData.zSize * _zTileSize / 2) + _zTileSize / 2;
            rightSideRoom = GenerateSideRoom(sideRoomStartingPos, roomData.rightSideRoomData,
                rightSideExitLocation.WithoutOffset, true);
        }

        LeftSideExitLocation leftSideExitLocation = LeftSideExitLocation.None();
        GeneratedRoom leftSideRoom = null;
        if (roomData.hasLeftSideRoom)
        {
            leftSideExitLocation = new LeftSideExitLocation(mapLayout, _xTileSize);
            GameObject leftSideExit = CreateLeftSideExit(leftSideExitLocation, roomParent);
            sideExits.Add(leftSideExit);
            Vector3 sideRoomStartingPos = leftSideExitLocation.WithoutOffset +
                                          (_xTileSize) +
                                          -(roomData.leftSideRoomData.zSize * _zTileSize / 2) + _zTileSize / 2;
            leftSideRoom = GenerateSideRoom(sideRoomStartingPos, roomData.leftSideRoomData,
                leftSideExitLocation.WithoutOffset, false);
        }

        List<Vector3> noWallPositions = new List<Vector3>()
        {
            entranceLocation.WithoutOffset, exitLocation.WithoutOffset,
            rightSideExitLocation.WithoutOffset, leftSideExitLocation.WithoutOffset
        };
        GeneratedWalls generatedWalls = CreateWalls(mapLayout, _zTileSize, _xTileSize, noWallPositions, roomParent);

        Vector3 middleOfRoom = CalculateMiddleOfRoom(roomData, startingPos, _tileSize);
        GeneratedRoom generatedRoom = new GeneratedRoom(mapLayout, startingPos, exitLocation, entranceLocation,
            generatedFloor, _tileSize.x, _tileSize.z, entrance, exit, roomParent, roomData, middleOfRoom, generatedWalls);

        eventGenerationManager.GenerateEvent(generatedRoom, previousRoom, sideExits);

        return generatedRoom;
    }

    public GeneratedRoom GenerateSideRoom(Vector3 startingPos, RoomData roomData, Vector3 previousRoomEntrance,
        bool isRightSideRoom)
    {
        GameObject roomObj = new GameObject(roomData.name);
        GameObject roomParent = Instantiate(roomObj, levelParent.transform);
        Destroy(roomObj);

        float xSize = roomData.xSize;
        float zSize = roomData.zSize;

        List<List<Vector3>> mapLayout = CreateMapLayout(startingPos, xSize, zSize);
        List<NavMeshSurface> generatedFloor = CreateFloor(mapLayout, roomParent);


        List<Vector3> noWallPositions = new List<Vector3>();
        if (isRightSideRoom)
        {
            noWallPositions.Add(previousRoomEntrance - _xTileSize);
        }
        else
        {
            noWallPositions.Add(previousRoomEntrance + _xTileSize);
        }

        GeneratedWalls generatedWalls = CreateWalls(mapLayout, _zTileSize, _xTileSize, noWallPositions, roomParent);

        Vector3 middleOfRoom = CalculateMiddleOfRoom(roomData, startingPos, _tileSize);
        GeneratedRoom generatedRoom = new GeneratedRoom(mapLayout, startingPos, null, null,
            generatedFloor, _tileSize.x, _tileSize.z, null, null, roomParent, roomData, middleOfRoom, generatedWalls);

        eventGenerationManager.GenerateSideEvent(generatedRoom, isRightSideRoom);

        return generatedRoom;
    }

    public Vector3 CalculateStartPosBasedOnPrevRoom(GeneratedRoom prevRoom, RoomData newRoom)
    {
        var xOffset = newRoom.xSize / 2 * _tileSize.x;
        Vector3 newRoomStartPos = prevRoom.ExitLocation.WithoutOffset + new Vector3(-xOffset, 0, _tileSize.z);
        return newRoomStartPos;
    }

    private List<List<Vector3>> CreateMapLayout(Vector3 startingPos, float xSize, float zSize)
    {
        List<List<Vector3>> mapLayout = new List<List<Vector3>>();
        Vector3 currentPos = startingPos;
        for (int z = 0; z < zSize; z++)
        {
            mapLayout.Add(new List<Vector3>());
            for (int x = 0; x < xSize; x++)
            {
                mapLayout[z].Add(currentPos);
                currentPos += _xTileSize;
            }

            currentPos += _zTileSize;
            currentPos.x = startingPos.x;
        }

        return mapLayout;
    }

    private List<NavMeshSurface> CreateFloor(List<List<Vector3>> mapLayout, GameObject roomParent)
    {
        List<NavMeshSurface> generatedFloor = new List<NavMeshSurface>();
        mapLayout.ForEach(rowList =>
        {
            rowList.ForEach(posInRow =>
            {
                GameObject createdFloor =
                    Instantiate(floorTilePicker.PickFloorTile(), posInRow, Quaternion.identity, roomParent.transform);
                NavMeshSurface navMeshSurface = createdFloor.GetComponent<NavMeshSurface>();
                generatedFloor.Add(navMeshSurface);
            });
        });
        return generatedFloor;
    }

    private GameObject CreateEntrance(EntranceLocation entranceLocation, GameObject roomParent)
    {
        GameObject createdEntrance = Instantiate(entranceTileWithoutDoor, entranceLocation.WithOffset,
            Quaternion.Euler(0f, 0, 0f), roomParent.transform);
        return createdEntrance;
    }

    private GameObject CreateExit(ExitLocation exitLocation, GameObject roomParent)
    {
        GameObject createdExit = Instantiate(entranceTileWithDoor, exitLocation.WithOffset,
            Quaternion.Euler(0f, 180, 0f), roomParent.transform);
        return createdExit;
    }

    private GameObject CreateRightSideExit(RightSideExitLocation exitLocation, GameObject roomParent)
    {
        GameObject createdExit = Instantiate(entranceTileWithDoor, exitLocation.WithOffset,
            Quaternion.Euler(0f, 90, 0f), roomParent.transform);
        return createdExit;
    }

    private GameObject CreateLeftSideExit(LeftSideExitLocation exitLocation, GameObject roomParent)
    {
        GameObject createdExit = Instantiate(entranceTileWithDoor, exitLocation.WithOffset,
            Quaternion.Euler(0f, 270, 0f), roomParent.transform);
        return createdExit;
    }

    private GeneratedWalls CreateWalls(List<List<Vector3>> mapLayout, Vector3 zTileSize, Vector3 xTileSize,
        List<Vector3> ignorePositions, GameObject roomParent)
    {
        List<Vector3> firstRow = mapLayout.First().Where(pos => !ignorePositions.Contains(pos)).ToList();
        List<Vector3> lastRow = mapLayout.Last().Where(pos => !ignorePositions.Contains(pos)).ToList();
        List<Vector3> rightColumn = mapLayout.Select(row => row.First())
            .Where(pos => !ignorePositions.Contains(pos)).ToList();
        List<Vector3> leftColumn = mapLayout.Select(row => row.Last()).Where(pos => !ignorePositions.Contains(pos))
            .ToList();

        List<GameObject> firstRowWalls = firstRow.Select(CreateWall(-zTileSize / 2, Quaternion.Euler(0f, 0, 0f), roomParent)).ToList();
        List<GameObject> lastRowWalls = lastRow.Select(CreateWall(zTileSize / 2, Quaternion.Euler(0f, 180, 0f), roomParent)).ToList();
        List<GameObject> rightColumnWalls = rightColumn.Select(CreateWall(-xTileSize / 2, Quaternion.Euler(0f, 90f, 0f), roomParent)).ToList();
        List<GameObject> leftColumnWalls = leftColumn.Select(CreateWall(xTileSize / 2, Quaternion.Euler(0f, 270f, 0f), roomParent)).ToList();
        return new GeneratedWalls(firstRowWalls, lastRowWalls, rightColumnWalls, leftColumnWalls);
    }

    private Func<Vector3, GameObject> CreateWall(Vector3 offset, Quaternion rotation, GameObject roomParent)
    {
        return pos => Instantiate(wallTile, pos + offset, rotation, roomParent.transform);
    }

    private static Vector3 CalculateMiddleOfRoom(RoomData roomData, Vector3 startingPos, Vector3 tileSize)
    {
        Vector3 tileOffset = new Vector3(tileSize.x / 2, 0, tileSize.z / 2);
        Vector3 middleRoomOffset = new Vector3(
            roomData.xSize * tileSize.x / 2,
            0,
            roomData.zSize * tileSize.z / 2);
        Vector3 middleOfRoom = startingPos +
                               middleRoomOffset +
                               -tileOffset;
        return middleOfRoom;
    }

    public class EntranceLocation
    {
        public Vector3 WithOffset { get; }
        public Vector3 WithoutOffset { get; }

        public int ZGridPos { get; }
        public int XGridPos { get; }

        public EntranceLocation(List<List<Vector3>> mapLayout, Vector3 zTileSize)
        {
            ZGridPos = 0;
            List<Vector3> firstRow = mapLayout[XGridPos];
            XGridPos = firstRow.Count / 2;
            Vector3 entranceLocation = firstRow[XGridPos];
            WithOffset = entranceLocation - zTileSize / 2;
            WithoutOffset = entranceLocation;
        }

        private EntranceLocation()
        {
            WithOffset = Vector3.negativeInfinity;
            WithoutOffset = Vector3.negativeInfinity;
        }

        public static EntranceLocation None()
        {
            return new EntranceLocation();
        }
    }

    public class ExitLocation
    {
        public Vector3 WithOffset { get; }
        public Vector3 WithoutOffset { get; }

        public ExitLocation(List<List<Vector3>> mapLayout, Vector3 zTileSize)
        {
            List<Vector3> lastRow = mapLayout.Last();
            Vector3 exitLocation = lastRow[lastRow.Count / 2];
            WithOffset = exitLocation + zTileSize / 2;
            WithoutOffset = exitLocation;
        }

        private ExitLocation()
        {
            WithOffset = Vector3.negativeInfinity;
            WithoutOffset = Vector3.negativeInfinity;
        }

        public static ExitLocation None()
        {
            return new ExitLocation();
        }
    }

    public class RightSideExitLocation
    {
        public Vector3 WithOffset { get; }
        public Vector3 WithoutOffset { get; }

        public RightSideExitLocation(List<List<Vector3>> mapLayout, Vector3 xTileSize)
        {
            List<Vector3> midRow = mapLayout[mapLayout.Count / 2];
            Vector3 exitLocation = midRow.First();
            WithOffset = exitLocation - xTileSize / 2;
            WithoutOffset = exitLocation;
        }

        private RightSideExitLocation()
        {
            WithOffset = Vector3.negativeInfinity;
            WithoutOffset = Vector3.negativeInfinity;
        }

        public static RightSideExitLocation None()
        {
            return new RightSideExitLocation();
        }
    }

    public class LeftSideExitLocation
    {
        public Vector3 WithOffset { get; }
        public Vector3 WithoutOffset { get; }

        public LeftSideExitLocation(List<List<Vector3>> mapLayout, Vector3 xTileSize)
        {
            List<Vector3> midRow = mapLayout[mapLayout.Count / 2];
            Vector3 exitLocation = midRow.Last();
            WithOffset = exitLocation + xTileSize / 2;
            WithoutOffset = exitLocation;
        }

        private LeftSideExitLocation()
        {
            WithOffset = Vector3.negativeInfinity;
            WithoutOffset = Vector3.negativeInfinity;
        }

        public static LeftSideExitLocation None()
        {
            return new LeftSideExitLocation();
        }
    }
}