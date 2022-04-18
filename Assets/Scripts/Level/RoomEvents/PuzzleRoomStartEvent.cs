using System;
using Level.RoomEvents;
using UnityEngine;

public class PuzzleRoomStartEvent : RoomStartEvent
{
    public Action StartAction { private get; set; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartAction.Invoke();
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}