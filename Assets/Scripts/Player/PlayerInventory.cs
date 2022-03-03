using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInventory : MonoBehaviour
{
    public List<Collectible> Items { get; private set; }
    private PlayerHealth _playerHealth;
    private PlayerSpellManager _playerSpellManager;
    private NavMeshAgent _playerNavMeshAgent;
    private UIManager _uiManager;

    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerSpellManager = GetComponent<PlayerSpellManager>();
        _playerNavMeshAgent = GetComponent<NavMeshAgent>();
        _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        Items = new List<Collectible>();
    }

    public void PickupItem(Collectible collectible)
    {
        Items.Add(collectible);
        collectible.ApplyEffects(_uiManager);
        collectible.ApplyEffects(_playerHealth);
        collectible.ApplyEffects(_playerSpellManager);
        collectible.ApplyEffects(_playerNavMeshAgent);
    }

}
