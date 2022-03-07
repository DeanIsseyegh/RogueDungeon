using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Collectible Info", menuName = "Spell Collectible Info")]
public class SpellInfo : CollectibleInfo
{
    public void GenerateStats(CollectibleSpell spell)
    {
        string spellStats = "";
        SpellData data = spell.spellData;
        spellStats += $"Mana Cost: {data.manaCost}\n";
        spellStats += $"Cooldown: {data.spellCooldown}\n";
        foreach (SpellTrait trait in data.spellTraits)
        {
            spellStats += $"{trait.Name()}: {trait.Value()}\n";
        }

        base.stats = spellStats;
    }
}
