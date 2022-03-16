using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerSpellManager : MonoBehaviour
{
    private UIManager _uiManager;
    private PlayerInventory _inventory;
    private Dictionary<KeyCode, Spell> _spellInputsMap;
    private List<KeyCode> _spellInputs;

    private NavMeshAgent _navMeshAgent;
    private PlayerInventory _playerInventory;

    private PlayerMana _playerMana;

    private Animator _animator;

    private void Awake()
    {
        _uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
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

    private void Update()
    {
        UpdateCooldownUI();
    }

    public void AddSpell(Spell spell, Sprite spellIcon)
    {
        int currentSpellIndex = _spellInputsMap.Count;
        _spellInputsMap[_spellInputs[currentSpellIndex]] = spell;
        _uiManager.UpdateSpellIcon(spellIcon, currentSpellIndex);
    }

    public bool IsCastingSpell()
    {
        return _spellInputs.Count > 0 && _spellInputsMap.Values.Any(spell => spell.IsCastingSpell);
    }

    public Spell RetrieveSpell(KeyCode spellKeyPressed)
    {
        KeyCode mappedSpellKey = _spellInputsMap.Keys.FirstOrDefault(x => x == spellKeyPressed);
        if (mappedSpellKey == KeyCode.None) return null;
        Spell spell = _spellInputsMap[mappedSpellKey];
        return spell;
    }

    private void UpdateCooldownUI()
    {
        List<Spell> spells = _spellInputsMap.Values.ToList();
        for (int i = 0; i < spells.Count; i++)
        {
            Spell spell = spells[i];
            _uiManager.UpdateSpellCooldown(spell.data.spellCooldown,
                spell.data.spellCooldown - spell.TimeSinceLastSpell, i);
        }
    }
}