using System.Collections.Generic;
using UnityEngine;

public class SpellObjManager : MonoBehaviour
{

    public List<Spell> Spells;

    public void AddSpell(Spell spell)
    {
        Spells.Add(spell);
    }
    
    public void AddSpells(List<Spell> spells)
    {
        spells.AddRange(spells);
    }
    
    
}
