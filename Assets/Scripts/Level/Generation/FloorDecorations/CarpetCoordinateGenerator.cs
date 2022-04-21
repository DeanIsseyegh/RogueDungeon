using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level.Generation.FloorDecorations
{
    public class CarpetCoordinateGenerator : MonoBehaviour
    {
        public List<Tuple<int, int>> Generate(GeneratedRoom room)
        {
            List<List<Vector3>> floorLayout = room.MapLayout;
            int zStart = room.EntranceLocation.ZGridPos;
            int xPos = room.EntranceLocation.XGridPos;

            List<Tuple<int, int>> carpetPositions = new List<Tuple<int, int>>();
            for (int i = zStart; i < floorLayout.Count; i++)
            {
                carpetPositions.Add(new Tuple<int, int>(i, xPos));
            }

            return carpetPositions;
        }
    }
}