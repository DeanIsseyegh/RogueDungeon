using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpellManager : MonoBehaviour
{
    [SerializeField] private SpellObjManager spellObjManager;
    private PlayerInventory _inventory;
    private float _spellCooldown = 1;
    private float _timeSinceLastSpell = 999;
    private Dictionary<KeyCode, Spell> _spellInputsMap;


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
        _inventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        _timeSinceLastSpell += Time.deltaTime;
    }

    public bool IsNotCastingSpell()
    {
        // return _timeSinceLastSpell > _spellCooldown;
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
                spell.Cast();
            }
        }
    }

}