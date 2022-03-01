using UnityEngine;

public class EventGenerator : MonoBehaviour
{
    public void GenerateEvent(RoomData roomData, GameObject roomParent, RoomGenerator.EntranceLocation entranceLocation, GameObject exitObj, Vector3 tileSize)
    {
        if (roomData.hasSpell)
        {
            GameObject newObj = new GameObject("RoomTriggerGenerated");
            Vector3 roomZLength = new Vector3(0, 0, roomData.zSize * tileSize.z / 2);
            Vector3 middleOfRoom = entranceLocation.WithoutOffset + roomZLength;
            GameObject emptyGameObj = Instantiate(newObj, Vector3.zero, Quaternion.identity, roomParent.transform);
            Destroy(newObj);
            BoxCollider boxCollider = emptyGameObj.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.center = middleOfRoom;
            boxCollider.size = new Vector3(roomData.xSize * tileSize.x, 1, (roomData.zSize - 1) * tileSize.z);
            
            RoomSpellStartEvent roomSpellStartEvent = emptyGameObj.AddComponent<RoomSpellStartEvent>();
            roomSpellStartEvent.MiddleOfRoomPos = middleOfRoom;
            RoomSpellEndEvent roomSpellEndEvent = emptyGameObj.AddComponent<RoomSpellEndEvent>();
            roomSpellEndEvent.isRoomComplete = () => roomSpellStartEvent.HasEventFinished();
            roomSpellEndEvent.onRoomComplete = () =>
            {
                exitObj.GetComponentInChildren<Open>().enabled = true;
                roomSpellStartEvent.enabled = false;
                roomSpellEndEvent.enabled = false;
            };
        }

        if (roomData.hasItem)
        {
            GameObject newObj = new GameObject("RoomTriggerGenerated");
            Vector3 roomZLength = new Vector3(0, 0, roomData.zSize * tileSize.z / 2);
            Vector3 middleOfRoom = entranceLocation.WithoutOffset + roomZLength;
            GameObject emptyGameObj = Instantiate(newObj, Vector3.zero, Quaternion.identity, roomParent.transform);
            Destroy(newObj);
            BoxCollider boxCollider = emptyGameObj.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.center = middleOfRoom;
            boxCollider.size = new Vector3(roomData.xSize * tileSize.x, 1, (roomData.zSize - 1) * tileSize.z);
            
            RoomItemStartEvent roomItemStartEvent = emptyGameObj.AddComponent<RoomItemStartEvent>();
            roomItemStartEvent.MiddleOfRoomPos = middleOfRoom;
            RoomItemEndEvent roomItemEndEvent = emptyGameObj.AddComponent<RoomItemEndEvent>();
            roomItemEndEvent.isRoomComplete = () => roomItemStartEvent.HasEventFinished();
            roomItemEndEvent.onRoomComplete = () =>
            {
                exitObj.GetComponentInChildren<Open>().enabled = true;
                roomItemStartEvent.enabled = false;
                roomItemEndEvent.enabled = false;
            };  
        }
    }
}