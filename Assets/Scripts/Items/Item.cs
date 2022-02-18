using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Item : MonoBehaviour
{

    public abstract void ApplyEffects(PlayerHealth playerHealth);
    public abstract void ApplyEffects(PlayerSpellManager playerSpellManager);
    public abstract void ApplyEffects(NavMeshAgent playerNavMeshAgent);

    public abstract void ApplyEffects(GameObject spellPrefab);
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            playerInventory.PickupItem(this);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

}
