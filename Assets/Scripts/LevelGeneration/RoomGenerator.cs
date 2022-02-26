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

    // Start is called before the first frame update
    void Start()
    {
        generatedFloor = new List<NavMeshSurface>();
        MeshRenderer floorRenderer = floorTile.GetComponent<MeshRenderer>();
        Vector3 floorSize = floorRenderer.bounds.size;
        Vector3 xOffset = new Vector3(floorSize.x, 0, 0);
        Vector3 zOffset = new Vector3(0, 0, floorSize.z);

        //Generate floor
        Vector3 currentPos = startingPos;
        for (int x = 0; x < xSize; x++)
        {
            currentPos += xOffset;
            currentPos.z = startingPos.z;
            for (int z = 0; z < zSize; z++)
            {
                GameObject createdFloor = Instantiate(floorTile, currentPos, Quaternion.identity);
                NavMeshSurface navMeshSurface = createdFloor.GetComponent<NavMeshSurface>();
                generatedFloor.Add(navMeshSurface);
                currentPos += zOffset;
            }
        }

        generatedFloor.ForEach(it => it.BuildNavMesh());
    }

    // Update is called once per frame
    void Update()
    {
    }
}