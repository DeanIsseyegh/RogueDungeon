using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Level.Generation
{
    public class WallDecorationsGenerator : MonoBehaviour
    {
        [SerializeField] private WallDecoration wallDeco;
        
        public void Generate(GeneratedRoom generatedRoom)
        {
            GeneratedWalls walls = generatedRoom.GeneratedWalls;
            walls.FirstRowWalls.ForEach(wall =>
            {
                GameObject createdDeco = Instantiate(wallDeco.prefab, wall.transform);
                createdDeco.transform.position += wallDeco.offset;
            });
            walls.LastRowWalls.ForEach(wall =>
            {
                GameObject createdDeco = Instantiate(wallDeco.prefab, wall.transform);
                // createdDeco.transform.position += wallDeco.offset;
                createdDeco.transform.localPosition += wallDeco.offset;
            });


            // List<List<Vector3>> generatedRoomMapLayout = generatedRoom.MapLayout;
            // List<Vector3> top = generatedRoomMapLayout[0];
            // List<Vector3> bottom = generatedRoomMapLayout[generatedRoomMapLayout.Count - 1];
            // List<Vector3> rightSide = generatedRoomMapLayout.Select(it => it[it.Count - 1]).ToList();
            // List<Vector3> leftSide = generatedRoomMapLayout.Select(it => it[0]).ToList();
            //
            // for (var i = top.Count - 1; i >= 0; i--)
            // {
            //     var positionForDeco = top[i];
            //     GameObject createdDeco = Instantiate(wallDeco, positionForDeco, Quaternion.identity);
            // }
            //
            // for (var i = bottom.Count - 1; i >= 0; i--)
            // {
            //     var positionForDeco = bottom[i];
            //     GameObject createdDeco = Instantiate(wallDeco, positionForDeco, Quaternion.identity);
            // }
        }
        
    }
}