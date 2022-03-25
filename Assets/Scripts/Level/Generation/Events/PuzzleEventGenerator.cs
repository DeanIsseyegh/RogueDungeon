using System.Collections.Generic;
using UnityEngine;

namespace Level.RoomEvents
{
    public class PuzzleEventGenerator : EventGenerator
    {
        private static readonly Vector3 PuzzleRewardHeightOffset = Vector3.up * 1.5f;
        
        [SerializeField] protected GameObject shortWall;
        [SerializeField] protected GameObject balloon;
        [SerializeField] protected RandomGameObjGenerator puzzleRewardGenerator;
        public void GeneratePuzzleEvent(GeneratedRoom generatedRoom, bool isRightSideRoom)
        {
            List<GameObject> balloonEventWalls = new List<GameObject>();
            for (var i = 0; i <  generatedRoom.ZSize; i++)
            {
                Vector3 tile =  generatedRoom.MapLayout[i][generatedRoom.ZSize/2];
                GameObject wall = Instantiate(shortWall, tile, shortWall.transform.rotation);
                balloonEventWalls.Add(wall);
            }

            BalloonManager balloonManager = generatedRoom.RoomParent.AddComponent<BalloonManager>();
            int balloonSpawnColumn = isRightSideRoom ? 1 : generatedRoom.XSize - 1;
            for (int i = 0; i < 3; i++)
            {
                Vector3 balloonsSpawnPos = generatedRoom.MapLayout[i][balloonSpawnColumn];
                GameObject createdBalloon = Instantiate(balloon, balloonsSpawnPos + Vector3.up * (1 + i), balloon.transform.localRotation);
                balloonManager.Register(createdBalloon);
            }

            RoomEndEvent roomEndEvent = generatedRoom.RoomParent.AddComponent<RoomEndEvent>();
            roomEndEvent.isRoomComplete = () => balloonManager.AreAllBalloonsDestroyed();
            roomEndEvent.onRoomComplete = () =>
            {
                balloonEventWalls.ForEach(Destroy);
                puzzleRewardGenerator.Generate(generatedRoom.MiddleOfRoom + PuzzleRewardHeightOffset);
                roomEndEvent.enabled = false;
            };
        }
    }
}