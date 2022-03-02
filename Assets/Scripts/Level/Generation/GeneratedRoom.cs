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
    public Vector3 StartPos { get; }
    public int XSize { get; }
    public int ZSize { get; }

    public GeneratedRoom(List<List<Vector3>> mapLayout, RoomGenerator.ExitLocation exitLocation,
        RoomGenerator.EntranceLocation entranceLocation, List<NavMeshSurface> navMeshSurfaces, float xTileSize,
        float zTileSize, Vector3 startPos, GameObject entrance, GameObject exit)
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
    }

    public void BuildNavmesh()
    {
        _navMeshSurfaces.ForEach(it => it.BuildNavMesh());
    }
}