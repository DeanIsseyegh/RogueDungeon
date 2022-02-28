using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GeneratedRoom
{
    public Vector3 ExitLocation { get; }
    private readonly List<List<Vector3>> _mapLayout;
    private readonly List<NavMeshSurface> _navMeshSurfaces;
    private readonly float _xTileSize;
    private readonly float _zTileSize;
    public Vector3 StartPos { get; }
    public int XSize { get; }
    public int ZSize { get; }

    public GeneratedRoom(List<List<Vector3>> mapLayout, Vector3 exitLocation, List<NavMeshSurface> navMeshSurfaces,
        float xTileSize, float zTileSize, Vector3 startPos)
    {
        ExitLocation = exitLocation;
        _mapLayout = mapLayout;
        _navMeshSurfaces = navMeshSurfaces;
        _xTileSize = xTileSize;
        _zTileSize = zTileSize;
        StartPos = startPos;
        XSize = _mapLayout.First().Count;
        ZSize = _mapLayout.Count;
        ExitLocation = exitLocation;
    }

    public Vector3 ExitLocationWithOffset()
    {
        return ExitLocation + new Vector3(-_xTileSize, 0, _zTileSize);
    }

    public void BuildNavmesh()
    {
        _navMeshSurfaces.ForEach(it => it.BuildNavMesh());
    }
}