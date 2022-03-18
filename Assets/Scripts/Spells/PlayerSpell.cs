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

    protected override Quaternion CalculateSpellRotation(GameObject caster, Vector3 spellPos)
    {
        PlayerAiming playerAiming = caster.GetComponent<PlayerAiming>();
        Vector3 aimDirection = (playerAiming.MouseWorldPosition - spellPos).normalized;
        Quaternion spellRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
        return spellRotation;
    }
}