using Level.RoomEvents;
using UnityEngine;

namespace Level.Generation.EventGenerators
{
    public class BossEventGenerator : EventGenerator
    {
        [SerializeField] private GameObject boss;
        [SerializeField] private Vector3 bossSpawnOffset;
        public void GenerateBossEvent(GeneratedRoom generatedRoom)
        {
            var emptyGameObj = CreateTriggerInRoom(generatedRoom);

            BossRoomStartEvent startEvent = emptyGameObj.AddComponent<BossRoomStartEvent>();
            startEvent.BossSpawner = () => Instantiate(boss, generatedRoom.MiddleOfRoom + bossSpawnOffset, Quaternion.identity);
            
            RoomEndEvent roomEndEvent = emptyGameObj.AddComponent<RoomEndEvent>();
            roomEndEvent.IsRoomComplete = () => startEvent.HasEventFinished();
            roomEndEvent.OnRoomComplete = () =>
            {
                //Any end events/cutscenes etc. can happen here
                roomEndEvent.enabled = false;
            };
        }
    }
}