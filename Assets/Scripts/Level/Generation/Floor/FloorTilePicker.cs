using System.Collections.Generic;
using UnityEngine;

namespace Level.Generation.Floor
{
    public class FloorTilePicker : MonoBehaviour
    {

        [SerializeField] private List<FloorTile> floorTiles;
        private List<FloorTile> _weightedFloorTiles;

        private void Awake()
        {
            _weightedFloorTiles = new List<FloorTile>();
            for (var i = 0; i < floorTiles.Count; i++)
            {
                FloorTile floorTile = floorTiles[i];
                for (var j = 0; j < floorTile.rngWeighting; j++)
                {
                    _weightedFloorTiles.Add(floorTile);
                }
            }
        }

        public GameObject PickFloorTile()
        {
            return _weightedFloorTiles[Random.Range(0, _weightedFloorTiles.Count)].prefab;
        }
        
    }
}