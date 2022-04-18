using System;
using UnityEngine;

public class RoomEndEvent : MonoBehaviour
{
    public Func<bool> IsRoomComplete { private get; set; }
    public Action OnRoomComplete { private get; set; }

    void Update()
    {
        if (IsRoomComplete != null)
        {
            if (IsRoomComplete.Invoke())
            {
                OnRoomComplete.Invoke();
            }
        }
    }
}
