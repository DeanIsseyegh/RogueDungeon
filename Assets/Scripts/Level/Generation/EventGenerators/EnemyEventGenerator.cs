using System.Collections.Generic;
using UnityEngine;

namespace Level.RoomEvents
{
    public class EnemyEventGenerator : EventGenerator
    {
        public void GenerateEnemyEvent(GeneratedRoom generatedRoom, GeneratedRoom previousRoom,
            List<GameObject> sideExits)
        {
            var emptyGameObj = CreateTriggerInRoom(generatedRoom);
            List<List<Vector3>> mapLayout = generatedRoom.MapLayout;

            int enemiesToGenerate = generatedRoom.RoomData.numberOfEnemies;
            List<Vector3> spawnPositions = GenerateEnemySpawnPos(generatedRoom, enemiesToGenerate, mapLayout);
            EnemyRoomStartEvent startEvent = emptyGameObj.AddComponent<EnemyRoomStartEvent>();
            startEvent.EnemyPositions = spawnPositions;
            ShutEntrance(generatedRoom.EntranceLocation, previousRoom, startEvent);

            RoomEndEvent roomEndEvent = emptyGameObj.AddComponent<RoomEndEvent>();
            roomEndEvent.IsRoomComplete = () => startEvent.HasEventFinished();
            roomEndEvent.OnRoomComplete = () =>
            {
                startEvent.enabled = false;

                if (generatedRoom.RoomData.hasExit)
                    generatedRoom.Exit.GetComponentInChildren<Open>().enabled = true;
                if (generatedRoom.RoomData.hasLeftSideRoom || generatedRoom.RoomData.hasRightSideRoom)
                    sideExits.ForEach( it => it.GetComponentInChildren<Open>().enabled = true);

                roomEndEvent.enabled = false;
            };
        }
        
        private static List<Vector3> GenerateEnemySpawnPos(GeneratedRoom generatedRoom, int enemiesToGenerate, List<List<Vector3>> mapLayout)
        {
            List<Vector3> spawnPositions = new List<Vector3>();
            for (int i = 0; i < enemiesToGenerate; i++)
            {
                int randomXPos = Random.Range(0, generatedRoom.XSize);
                int randomZPos = Random.Range(0, generatedRoom.ZSize);

                Vector3 spawnPos = mapLayout[randomZPos][randomXPos];
                spawnPositions.Add(spawnPos);
            }
            return spawnPositions;
        }
    }
}