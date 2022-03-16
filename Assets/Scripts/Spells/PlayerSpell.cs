using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSpell : Spell
{

    protected override void Update()
    {
        base.Update();
    }

    protected override void ApplyEffectsToSpell(GameObject spellPrefab, PlayerInventory playerInventory)
    {
        data.spellTraits.ForEach(trait => trait.ApplyEffects(spellPrefab, true));
        playerInventory.Items.ForEach(item => item.ApplyEffects(spellPrefab));
    }
}