using System.Collections.Generic;
using UnityEngine;

namespace Level.Generation.FloorDecorations
{
    public class CarpetGenerator : MonoBehaviour
    {

        [SerializeField] private GameObject carpet;
        
        public void Generate(GeneratedRoom room)
        {
            List<List<Vector3>> floorLayout = room.MapLayout;

            int zStart = room.EntranceLocation.ZGridPos;
            int xPos = room.EntranceLocation.XGridPos;
            for (int i = zStart; i < floorLayout.Count; i++)
            {
                Instantiate(carpet, floorLayout[i][xPos], Quaternion.identity);
            }
        }
    }
}