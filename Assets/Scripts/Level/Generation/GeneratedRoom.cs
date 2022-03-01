using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GeneratedRoom
{
    public RoomGenerator.ExitLocation ExitLocation { get; }
    private readonly List<List<Vector3>> _mapLayout;
    private readonly List<NavMeshSurface> _navMeshSurfaces;
    private readonly float _xTileSize;
    private readonly float _zTileSize;
    public GameObject Entrance { get; }
    public GameObject Exit { get; }
    public Vector3 StartPos { get; }
    public int XSize { get; }
    public int ZSize { get; }

    public GeneratedRoom(List<List<Vector3>> mapLayout, RoomGenerator.ExitLocation exitLocation, List<NavMeshSurface> navMeshSurfaces,
        float xTileSize, float zTileSize, Vector3 startPos,  GameObject entrance, GameObject exit)
    {
        _mapLayout = mapLayout;
        _navMeshSurfaces = navMeshSurfaces;
        _xTileSize = xTileSize;
        _zTileSize = zTileSize;
        Entrance = entrance;
        Exit = exit;
        StartPos = startPos;
        XSize = _mapLayout.First().Count;
        ZSize = _mapLayout.Count;
        ExitLocation = exitLocation;
    }

    public void BuildNavmesh()
    {
        _navMeshSurfaces.ForEach(it => it.BuildNavMesh());
    }
}