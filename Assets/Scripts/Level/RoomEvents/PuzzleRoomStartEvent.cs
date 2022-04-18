using System;
using Level.RoomEvents;
using UnityEngine;

public class PuzzleRoomStartEvent : RoomStartEvent
{
    public Action OnRoomStart { private get; set; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnRoomStart.Invoke();
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}