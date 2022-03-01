using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpellEndEvent : MonoBehaviour
{
    public Func<bool> isRoomComplete;
    public Action onRoomComplete;

    void Start()
    {
        
    }

    // Update is called once per frame
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
