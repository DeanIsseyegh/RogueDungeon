using Level.RoomEvents;
using UnityEngine;

public abstract class EventGenerator : MonoBehaviour
{
    [SerializeField] private GameObject closedEntranceTile;

    protected void ShutEntrance(RoomGenerator.EntranceLocation entranceLocation, GeneratedRoom previousRoom,
        RoomStartEvent startEvent)
    {
        if (previousRoom != null)
        {
            startEvent.ClosedEntranceTile = closedEntranceTile;
            startEvent.EntrancePos = entranceLocation.WithOffset;
            startEvent.EntranceRoomDoor = previousRoom.Exit;
        }
    }

    protected GameObject CreateTriggerInRoom(GeneratedRoom generatedRoom)
    {
        GameObject newObj = new GameObject("RoomTriggerGenerated");
        GameObject emptyGameObj = Instantiate(newObj, Vector3.zero, Quaternion.identity, generatedRoom.RoomParent.transform);
        Destroy(newObj);

        BoxCollider boxCollider = emptyGameObj.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.center = generatedRoom.MiddleOfRoom;
        boxCollider.size = new Vector3(generatedRoom.XSize * generatedRoom.XTileSize, 1,
            (generatedRoom.ZSize - 1) * generatedRoom.ZTileSize);
        return emptyGameObj;
    }

}