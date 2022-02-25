using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSpellManager : MonoBehaviour
{
    private PlayerInventory _inventory;
    private Dictionary<KeyCode, Spell> _spellInputsMap;
    private List<KeyCode> _spellInputs;

    private NavMeshAgent _navMeshAgent;
    private PlayerInventory _playerInventory;
    private PlayerMana _playerMana;
    private Animator _animator;

    private void Awake()
    {
        _inventory = GetComponent<PlayerInventory>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _playerInventory = GetComponent<PlayerInventory>();
        _playerMana = GetComponent<PlayerMana>();
    }

    private void Start()
    {
        _spellInputs = new List<KeyCode>()
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4
        };
        _spellInputsMap = new Dictionary<KeyCode, Spell>();
    }

    public void AddSpell(Spell spell)
    {
        int numOfSpells = _spellInputsMap.Count;
        _spellInputsMap[_spellInputs[numOfSpells]] = spell;
    }

    public bool IsCastingSpell()
    {
        return _spellInputs.Count > 0 && _spellInputsMap.Values.Any(spell => spell.IsCastingSpell);
    }

    public void HandleSpell()
    {
        if (!IsCastingSpell())
        {
            KeyCode spellKeyPressed = _spellInputsMap.Keys.FirstOrDefault(Input.GetKeyDown);
            if (spellKeyPressed == KeyCode.None) return;
            Spell spell = _spellInputsMap[spellKeyPressed];
            spell.Cast(_navMeshAgent, _animator, _playerInventory, _playerMana);
        }
    }
}