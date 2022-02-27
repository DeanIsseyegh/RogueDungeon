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
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private int xSize;
    [SerializeField] private int zSize;

    private List<NavMeshSurface> generatedFloor;

    public void GenerateFloor()
    {
        generatedFloor = new List<NavMeshSurface>();
        MeshRenderer floorRenderer = floorTile.GetComponent<MeshRenderer>();
        Vector3 floorSize = floorRenderer.bounds.size;
        Vector3 xOffset = new Vector3(floorSize.x, 0, 0);
        Vector3 zOffset = new Vector3(0, 0, floorSize.z);

        List<List<Vector3>> mapLayout = new List<List<Vector3>>();

        CreateGround(mapLayout, xOffset, zOffset);
        CreateWalls(mapLayout, zOffset, xOffset);

        generatedFloor.ForEach(it => it.BuildNavMesh());
    }

    private void CreateGround(List<List<Vector3>> mapLayout, Vector3 xOffset, Vector3 zOffset)
    {
        Vector3 currentPos = startingPos;
        for (int z = 0; z < zSize; z++)
        {
            mapLayout.Add(new List<Vector3>());
            for (int x = 0; x < xSize; x++)
            {
                mapLayout[z].Add(currentPos);
                currentPos += xOffset;
            }

            currentPos += zOffset;
            currentPos.x = startingPos.x;
        }

        mapLayout.ForEach(rowList =>
        {
            rowList.ForEach(posInRow =>
            {
                GameObject createdFloor = Instantiate(floorTile, posInRow, Quaternion.identity);
                NavMeshSurface navMeshSurface = createdFloor.GetComponent<NavMeshSurface>();
                generatedFloor.Add(navMeshSurface);
            });
        });
    }

    private void CreateWalls(List<List<Vector3>> mapLayout, Vector3 zOffset, Vector3 xOffset)
    {
        List<Vector3> firstRow = mapLayout.First();
        List<Vector3> lastRow = mapLayout.Last();
        List<Vector3> rightColumn = mapLayout.Select(row => row.First()).ToList();
        List<Vector3> leftColumn = mapLayout.Select(row => row.Last()).ToList();

        firstRow.ForEach(CreateWall(-zOffset / 2, Quaternion.Euler(0f, 0, 0f)));
        lastRow.ForEach(CreateWall(zOffset / 2, Quaternion.Euler(0f, 180, 0f)));
        rightColumn.ForEach(CreateWall(-xOffset / 2, Quaternion.Euler(0f, 90f, 0f)));
        leftColumn.ForEach(CreateWall(xOffset / 2, Quaternion.Euler(0f, 270f, 0f)));
    }

    private Action<Vector3> CreateWall(Vector3 offset, Quaternion rotation)
    {
        return pos =>
        {
            Instantiate(wallTile, pos + offset, rotation);
        };
    }
}