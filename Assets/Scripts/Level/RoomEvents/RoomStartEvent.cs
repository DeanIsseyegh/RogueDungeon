using UnityEngine;

namespace Level.RoomEvents
{
    public abstract class RoomStartEvent : MonoBehaviour
    {
        public Vector3 EntrancePos { private get; set; }
        public GameObject ClosedEntranceTile { private get; set; }
        public GameObject EntranceRoomDoor { get; set; }
        
        protected void CloseEntrance()
        {
            if (EntranceRoomDoor != null)
            {
                Instantiate(ClosedEntranceTile, EntrancePos, Quaternion.identity);
                Destroy(EntranceRoomDoor);
            }
        }
    }
}