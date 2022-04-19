using System.Collections.Generic;
using System.Linq;
using Level.Generation.WallDecorations.WaveFunctionCollapse;
using UnityEngine;

namespace Level.Generation
{
    public class WallDecorationsGenerator : MonoBehaviour
    {
        [SerializeField] private List<WallDecoration> wallDecos;
        [SerializeField] private WallDecoration defaultDeco;

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
            
            ////Wave Function Collapse prototype
            ApplyWfcToWall(walls.FirstRowWalls, weightedDecos);
            ApplyWfcToWall(walls.LastRowWalls, weightedDecos);
            ApplyWfcToWall(walls.RightColumnWalls, weightedDecos);
            ApplyWfcToWall(walls.LeftColumnWalls, weightedDecos);
        }

        private void ApplyWfcToWall(List<GameObject> walls, List<WallDecoration> weightedDecos)
        {
            //1. Create uncollapsed WFC collection
            var wfcCollection = new List<WfcGrid>();
            int gridSize = walls.Count;
            for (int i = 0; i < gridSize; i++)
            {
                List<WallDecoration> newWeightedList = new List<WallDecoration>(weightedDecos);
                List<WallDecoration> newUnweightedList = new List<WallDecoration>(wallDecos);
                wfcCollection.Add(new WfcGrid(newWeightedList, newUnweightedList, defaultDeco));
            }

            //2. Link WFC grids
            for (int i = 0; i < wfcCollection.Count; i++)
            {
                WfcGrid wfcGrid = wfcCollection[i];
                if (i > 0)
                {
                    WfcGrid leftNeighbour = wfcCollection[i - 1];
                    wfcGrid.LinkLeftNeighbour(leftNeighbour);
                }

                if (i < wfcCollection.Count - 1)
                {
                    WfcGrid rightNeighbour = wfcCollection[i + 1];
                    wfcGrid.LinkRightNeighbour(rightNeighbour);
                }
            }

            //2. Collapse a random grid randomly
            var randomGrid = wfcCollection[Random.Range(0, wfcCollection.Count)];
            randomGrid.RandomCollapse();

            //3. Iterate and Collapse
            IterateAndCollapse(wfcCollection);

            //4. Create decos based on WFC algo
            for (int i = 0; i < gridSize; i++)
            {
                GenerateDeco(wfcCollection[i].CollapsedValue, walls[i]);
            }
        }

        private static void IterateAndCollapse(List<WfcGrid> wfcCollection)
        {
            for (var i = 0; i < wfcCollection.Count; i++)
            {
                wfcCollection[i].Collapse();
            }

            if (wfcCollection.Any(it => !it.IsCollapsed))
            {
                WfcGrid unCollapsedGrid = wfcCollection.Find(it => !it.IsCollapsed);
                unCollapsedGrid.RandomCollapse();
                IterateAndCollapse(wfcCollection);
            }
        }

        private static void GenerateDeco(WallDecoration wallDeco, GameObject wall)
        {
            GameObject createdDeco = Instantiate(wallDeco.prefab, wall.transform);
            createdDeco.transform.localPosition += wallDeco.offset;
        }
    }


}