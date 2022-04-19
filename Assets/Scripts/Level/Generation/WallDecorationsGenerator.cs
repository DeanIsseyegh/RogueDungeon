using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level.Generation
{
    public class WallDecorationsGenerator : MonoBehaviour
    {
        [SerializeField] private List<WallDecoration> wallDecos;

        public void Generate(GeneratedRoom generatedRoom)
        {
            List<WallDecoration> weightedDecos = new List<WallDecoration>();
            for (var i = wallDecos.Count - 1; i >= 0; i--)
            {
                var wallDeco = wallDecos[i];
                for (var j = 0; j < wallDeco.rngWeighting; j++)
                {
                    weightedDecos.Add(wallDeco);
                }
            }

            GeneratedWalls walls = generatedRoom.GeneratedWalls;
            walls.FirstRowWalls.ForEach(wall => GenerateDeco(weightedDecos, wall));
            walls.LastRowWalls.ForEach(wall => GenerateDeco(weightedDecos, wall));
            walls.RightColumnWalls.ForEach(wall => GenerateDeco(weightedDecos, wall));
            walls.LeftColumnWalls.ForEach(wall => GenerateDeco(weightedDecos, wall));

        }

        private static void GenerateDeco(List<WallDecoration> weightedDecos, GameObject wall)
        {
            int randomIndex = Random.Range(0, weightedDecos.Count);
            var wallDeco = weightedDecos[randomIndex];
            GameObject createdDeco = Instantiate(wallDeco.prefab, wall.transform);
            createdDeco.transform.localPosition += wallDeco.offset;
        }
    }
}