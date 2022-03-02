using System;
using UnityEngine;

public class RoomEndEvent : MonoBehaviour
{
    public Func<bool> isRoomComplete;
    public Action onRoomComplete;

    void Update()
    {
        if (isRoomComplete != null)
        {
            if (isRoomComplete.Invoke())
            {
                onRoomComplete.Invoke();
            }
        }
    }
}
