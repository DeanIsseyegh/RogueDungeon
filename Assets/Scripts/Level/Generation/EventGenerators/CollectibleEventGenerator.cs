using System.Collections.Generic;
using UnityEngine;

namespace Level.RoomEvents
{
    public class CollectibleEventGenerator : EventGenerator
    {
        public void GenerateCollectibleEvent(GeneratedRoom generatedRoom, GeneratedRoom previousRoom,
            List<GameObject> sideExits)
        {
            var emptyGameObj = CreateTriggerInRoom(generatedRoom);

            CollectibleRoomStartEvent startEvent = AddRoomStartEvent(generatedRoom.RoomData, emptyGameObj);
            ShutEntrance(generatedRoom.EntranceLocation, previousRoom, startEvent);
            startEvent.MiddleOfRoomPos = generatedRoom.MiddleOfRoom;

            RoomEndEvent roomEndEvent = emptyGameObj.AddComponent<RoomEndEvent>();
            roomEndEvent.IsRoomComplete = () => startEvent.HasEventFinished();
            roomEndEvent.OnRoomComplete = () =>
            {
                startEvent.RemoveCollectibles();
                startEvent.HideChoiceUi();
                startEvent.enabled = false;

                if (generatedRoom.RoomData.hasExit)
                    generatedRoom.Exit.GetComponentInChildren<Open>().enabled = true;
                if (generatedRoom.RoomData.hasLeftSideRoom || generatedRoom.RoomData.hasRightSideRoom)
                    sideExits.ForEach( it => it.GetComponentInChildren<Open>().enabled = true);

                roomEndEvent.enabled = false;
            };
        }

        private static CollectibleRoomStartEvent AddRoomStartEvent(RoomData roomData, GameObject emptyGameObj)
        {
            if (roomData.hasSpell)
            {
                return emptyGameObj.AddComponent<SpellRoomStartEvent>();
            }
            else
            {
                return emptyGameObj.AddComponent<ItemRoomStartEvent>();
            }
        }
    }
}