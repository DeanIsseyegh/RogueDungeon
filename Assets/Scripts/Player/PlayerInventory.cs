using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> Items { get; private set; }
    private PlayerHealth _playerHealth;
    private PlayerSpellManager _playerSpellManager;
    private NavMeshAgent _playerNavMeshAgent;

    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerSpellManager = GetComponent<PlayerSpellManager>();
        _playerNavMeshAgent = GetComponent<NavMeshAgent>();
        Items = new List<Item>();
    }

    public void PickupItem(Item item)
    {
        Items.Add(item);
        item.ApplyEffects(_playerHealth);
        item.ApplyEffects(_playerSpellManager);
        item.ApplyEffects(_playerNavMeshAgent);
    }

}
