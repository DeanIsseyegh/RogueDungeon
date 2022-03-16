using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GeneratedRoom
{
    public RoomGenerator.ExitLocation ExitLocation { get; }
    public List<List<Vector3>> MapLayout { get; }
    public RoomGenerator.EntranceLocation EntranceLocation { get; }
    private readonly List<NavMeshSurface> _navMeshSurfaces;
    public float XTileSize { get; }
    public float ZTileSize { get; }
    public GameObject Entrance { get; }
    public GameObject Exit { get; }
    public GameObject RoomParent { get; }
    public RoomData RoomData { get; }
    public Vector3 StartPos { get; }
    public int XSize { get; }
    public int ZSize { get; }
    public Vector3 MiddleOfRoom { get; }

    public GeneratedRoom(List<List<Vector3>> mapLayout, Vector3 startPos, RoomGenerator.ExitLocation exitLocation,
        RoomGenerator.EntranceLocation entranceLocation, List<NavMeshSurface> navMeshSurfaces, float xTileSize,
        float zTileSize, GameObject entrance, GameObject exit, GameObject roomParent, RoomData roomData, Vector3 middleOfRoom)
    {
        MapLayout = mapLayout;
        EntranceLocation = entranceLocation;
        _navMeshSurfaces = navMeshSurfaces;
        XTileSize = xTileSize;
        ZTileSize = zTileSize;
        Entrance = entrance;
        Exit = exit;
        StartPos = startPos;
        XSize = MapLayout.First().Count;
        ZSize = MapLayout.Count;
        ExitLocation = exitLocation;
        RoomParent = roomParent;
        RoomData = roomData;
        MiddleOfRoom = middleOfRoom;
    }

    public void BuildNavmesh()
    {
        _navMeshSurfaces.ForEach(it => it.BuildNavMesh());
    }
    
}