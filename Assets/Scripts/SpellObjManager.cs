using System.Collections.Generic;
using UnityEngine;

public class SpellObjManager : MonoBehaviour
{

    private List<Spell> _spells;

    private void Start()
    {
        _spells = new List<Spell>();
    }

    public void AddSpell(Spell spell)
    {
        _spells.Add(spell);
    }
    
    public void AddSpells(List<Spell> spells)
    {
        spells.AddRange(spells);
    }

    public Spell MakeSpellPrefab(Spell spell)
    {
        GameObject instantiatedSpell = Instantiate(spell.gameObject);
        return instantiatedSpell.GetComponent<Spell>();
    }
    
}
