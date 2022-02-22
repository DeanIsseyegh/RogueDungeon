using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSpellManager : MonoBehaviour
{
    [SerializeField] private SpellObjManager spellObjManager;
    private PlayerInventory _inventory;
    private Dictionary<KeyCode, Spell> _spellInputsMap;

    private NavMeshAgent _navMeshAgent;
    private PlayerInventory _playerInventory;
    private Animator _animator;


    private void Awake()
    {
        _inventory = GetComponent<PlayerInventory>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _playerInventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        List<KeyCode> spellInputs = new List<KeyCode>()
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4
        };
        _spellInputsMap = spellObjManager.Spells.Select((spell, index) =>
        {
            var spellKeyCode = spellInputs[index];
            return new {spellKeyCode, spell};
        }).ToDictionary(e => e.spellKeyCode, e => e.spell);
    }

    public bool IsNotCastingSpell()
    {
        return !spellObjManager.Spells.Any(spell => spell.IsCastingSpell);
    }

    public void HandleSpell()
    {
        if (IsNotCastingSpell())
        {
            KeyCode spellKeyPressed = _spellInputsMap.Keys.FirstOrDefault(Input.GetKeyDown);
            if (spellKeyPressed == KeyCode.None) return;
            Spell spell = _spellInputsMap[spellKeyPressed];
            bool isASpellInProgress = spellObjManager.Spells.Any(spell => spell.IsCastingSpell);
            if (!isASpellInProgress)
            {
                spell.Cast(_navMeshAgent, _animator, _playerInventory);
            }
        }
    }

}