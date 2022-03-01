using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomItemStartEvent : MonoBehaviour
{
    private Vector3 _startingSpellHeightOffset;
    public RandomItemGenerator ItemGenerator { private get; set;  }
    public Vector3 MiddleOfRoomPos { private get; set; }

    public bool isItemCreated;
    
    public CollectibleSpell spells;
    private GameObject _createdItem;

    private void Awake()
    {
        ItemGenerator = FindObjectOfType<RandomItemGenerator>();
        _startingSpellHeightOffset = Vector3.up * 1.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("RoomStartTrigger entered");
            _createdItem = ItemGenerator.Generate(MiddleOfRoomPos + _startingSpellHeightOffset);
            isItemCreated = true;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public bool HasEventFinished()
    {
        return isItemCreated && _createdItem == null;
    }
}
