using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpellStartEvent : MonoBehaviour
{
    private Vector3 _startingSpellHeightOffset;
    public RandomSpellGenerator SpellGenerator { private get; set;  }
    public Vector3 MiddleOfRoomPos { private get; set; }

    public bool isSpellCreated;
    
    public CollectibleSpell spells;
    public GameObject createdSpell;

    private void Awake()
    {
        SpellGenerator = FindObjectOfType<RandomSpellGenerator>();
        _startingSpellHeightOffset = Vector3.up * 1.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("RoomStartTrigger entered");
            createdSpell = SpellGenerator.Generate(MiddleOfRoomPos + _startingSpellHeightOffset);
            isSpellCreated = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public bool HasEventFinished()
    {
        return isSpellCreated && createdSpell == null;
    }
}
