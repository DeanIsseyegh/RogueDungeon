using System;
using UnityEngine;

namespace Level.RoomEvents
{
    public class BossRoomStartEvent : RoomStartEvent
    {
        private GameObject _createdBoss;
        private bool _hasCreatedBoss;

        public Func<GameObject> BossSpawner { private get; set; }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _createdBoss = BossSpawner.Invoke();
                _hasCreatedBoss = true;
                CloseEntrance();
                GetComponent<BoxCollider>().enabled = false;
            }
        }
        
        public bool HasEventFinished()
        {
            return _hasCreatedBoss && _createdBoss == null;
        }
    }
}