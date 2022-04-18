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
        [SerializeField] protected MemorizationPuzzleManager memorizationPuzzleManager;
        public void GenerateBalloonEvent(GeneratedRoom generatedRoom, bool isRightSideRoom)
        {
            List<GameObject> balloonEventWalls = new List<GameObject>();
            for (var i = 0; i <  generatedRoom.ZSize; i++)
            {
                Vector3 tile =  generatedRoom.MapLayout[i][generatedRoom.ZSize/2];
                GameObject wall = Instantiate(shortWall, tile, shortWall.transform.rotation, generatedRoom.RoomParent.transform);
                balloonEventWalls.Add(wall);
            }

            BalloonManager balloonManager = generatedRoom.RoomParent.AddComponent<BalloonManager>();
            int balloonSpawnColumn = isRightSideRoom ? 0 : generatedRoom.XSize - 1;
            for (int i = 0; i < 3; i++)
            {
                Vector3 balloonsSpawnPos = generatedRoom.MapLayout[i][balloonSpawnColumn];
                GameObject createdBalloon = Instantiate(balloon, balloonsSpawnPos + Vector3.up * (1 + i), balloon.transform.localRotation, generatedRoom.RoomParent.transform);
                balloonManager.Register(createdBalloon);
            }

            RoomEndEvent roomEndEvent = generatedRoom.RoomParent.AddComponent<RoomEndEvent>();
            roomEndEvent.IsRoomComplete = () => balloonManager.AreAllBalloonsDestroyed();
            roomEndEvent.OnRoomComplete = () =>
            {
                balloonEventWalls.ForEach(Destroy);
                puzzleRewardGenerator.Generate(generatedRoom.MiddleOfRoom + PuzzleRewardHeightOffset);
                roomEndEvent.enabled = false;
            };
        }

        public void GenerateSkullEvent(GeneratedRoom generatedRoom, bool isRightSideRoom)
        {
            var emptyGameObj = CreateTriggerInRoom(generatedRoom);
            Quaternion rotation = isRightSideRoom ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);
            int startingRow = isRightSideRoom ? (generatedRoom.ZSize / 2) + 1 : (generatedRoom.ZSize / 2) - 1;
            int startingColumn = isRightSideRoom ? 0 : generatedRoom.XSize - 1;
            Vector3 startingPos = generatedRoom.MapLayout[startingRow][startingColumn];
            MemorizationPuzzleManager memoryPuzzleManager = Instantiate(memorizationPuzzleManager, startingPos, rotation, generatedRoom.RoomParent.transform);
            
            PuzzleRoomStartEvent puzzleRoomStartEvent = emptyGameObj.AddComponent<PuzzleRoomStartEvent>();
            puzzleRoomStartEvent.OnRoomStart = () => memoryPuzzleManager.StartGame();
            
            RoomEndEvent roomEndEvent = generatedRoom.RoomParent.AddComponent<RoomEndEvent>();
            roomEndEvent.IsRoomComplete = () => memoryPuzzleManager.IsGameWon;
            roomEndEvent.OnRoomComplete = () =>
            {
                puzzleRewardGenerator.Generate(generatedRoom.MiddleOfRoom + PuzzleRewardHeightOffset);
                roomEndEvent.enabled = false;
            };
        }
    }
}