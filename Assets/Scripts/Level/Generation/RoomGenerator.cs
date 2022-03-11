using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject entranceTileWithDoor;
    [SerializeField] private GameObject entranceTileWithoutDoor;
    [SerializeField] private GameObject levelParent;
    [SerializeField] private EventGenerator eventGenerator;
    [SerializeField] private GameObject roomStartTriggerPrefab;
    
    private Vector3 _xTileSize;
    private Vector3 _zTileSize;
    private Vector3 _tileSize;

    private void Awake()
    {
        MeshRenderer floorRenderer = floorTile.GetComponent<MeshRenderer>();
        Vector3 floorSize = floorRenderer.bounds.size;
        _xTileSize = new Vector3(floorSize.x, 0, 0);
        _zTileSize = new Vector3(0, 0, floorSize.z);
        _tileSize = new Vector3(floorSize.x, 0, floorSize.z);
    }

    public GeneratedRoom GenerateFloor(Vector3 startingPos, RoomData roomData, GeneratedRoom previousRoom)
    {
        GameObject roomObj = new GameObject(roomData.name);
        GameObject roomParent = Instantiate(roomObj, levelParent.transform);
        Destroy(roomObj);
        
        float xSize = roomData.xSize;
        float zSize = roomData.zSize;

        List<List<Vector3>> mapLayout = CreateMapLayout(startingPos, xSize, zSize);
        List<NavMeshSurface> generatedFloor = CreateFloor(mapLayout, roomParent);
        EntranceLocation entranceLocation = new EntranceLocation(mapLayout, _zTileSize);
        ExitLocation exitLocation = new ExitLocation(mapLayout, _zTileSize);
        GameObject entrance = CreateEntrance(entranceLocation, roomParent);
        GameObject exit = CreateExit(exitLocation, roomParent);
        CreateWalls(mapLayout, _zTileSize, _xTileSize, exitLocation, entranceLocation, roomParent);
        
        GeneratedRoom generatedRoom = new GeneratedRoom(mapLayout, exitLocation, entranceLocation, generatedFloor, 
            _tileSize.x, _tileSize.z, startingPos, entrance, exit);
        
        eventGenerator.GenerateEvent(roomData, roomParent, entranceLocation, generatedRoom, previousRoom);
        
        return generatedRoom;
    }

    public Vector3 CalculateStartPosBasedOnPrevRoom(GeneratedRoom prevRoom, RoomData nextRoom)
    {
        var xOffset = (prevRoom.StartPos.x + (prevRoom.XSize - nextRoom.xSize) / 2 * _tileSize.x);
        Vector3 newRoomStartPos = prevRoom.ExitLocation.WithoutOffset + new Vector3(xOffset, 0, _tileSize.z);
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
                    Instantiate(floorTile, posInRow, Quaternion.identity, roomParent.transform);
                NavMeshSurface navMeshSurface = createdFloor.GetComponent<NavMeshSurface>();
                generatedFloor.Add(navMeshSurface);
            });
        });
        return generatedFloor;
    }

    private GameObject CreateExit(ExitLocation exitLocation, GameObject roomParent)
    {
        GameObject createdExit = Instantiate(entranceTileWithDoor, exitLocation.WithOffset,
            Quaternion.Euler(0f, 180, 0f), roomParent.transform);
        return createdExit;
    }

    private GameObject CreateEntrance(EntranceLocation entranceLocation, GameObject roomParent)
    {
        GameObject createdEntrance = Instantiate(entranceTileWithoutDoor, entranceLocation.WithOffset,
            Quaternion.Euler(0f, 0, 0f), roomParent.transform);
        return createdEntrance;
    }

    private void CreateWalls(List<List<Vector3>> mapLayout, Vector3 zTileSize, Vector3 xTileSize,
        ExitLocation exitLocation, EntranceLocation entranceLocation, GameObject roomParent)
    {
        List<Vector3> firstRow = mapLayout.First();
        List<Vector3> lastRow = mapLayout.Last();
        List<Vector3> rightColumn = mapLayout.Select(row => row.First()).ToList();
        List<Vector3> leftColumn = mapLayout.Select(row => row.Last()).ToList();

        firstRow.ForEach(CreateWall(-zTileSize / 2, Quaternion.Euler(0f, 0, 0f), exitLocation.WithoutOffset,
            entranceLocation.WithoutOffset, roomParent));
        lastRow.ForEach(CreateWall(zTileSize / 2, Quaternion.Euler(0f, 180, 0f), exitLocation.WithoutOffset,
            entranceLocation.WithoutOffset, roomParent));
        rightColumn.ForEach(CreateWall(-xTileSize / 2, Quaternion.Euler(0f, 90f, 0f), exitLocation.WithoutOffset,
            entranceLocation.WithoutOffset, roomParent));
        leftColumn.ForEach(CreateWall(xTileSize / 2, Quaternion.Euler(0f, 270f, 0f), exitLocation.WithoutOffset,
            entranceLocation.WithoutOffset, roomParent));
    }

    private Action<Vector3> CreateWall(Vector3 offset, Quaternion rotation, Vector3 exitLocation,
        Vector3 entranceLocation, GameObject roomParent)
    {
        return pos =>
        {
            if (!exitLocation.Equals(pos) && !entranceLocation.Equals(pos))
            {
                Instantiate(wallTile, pos + offset, rotation, roomParent.transform);
            }
        };
    }

    public class EntranceLocation
    {
        public Vector3 WithOffset { get; }
        public Vector3 WithoutOffset { get; }

        public EntranceLocation(List<List<Vector3>> mapLayout, Vector3 zTileSize)
        {
            List<Vector3> firstRow = mapLayout.First();
            Vector3 entranceLocation = firstRow[firstRow.Count / 2];
            WithOffset = entranceLocation - zTileSize / 2;
            WithoutOffset = entranceLocation;
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
    }
}