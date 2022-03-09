using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damaging Spell Trait", menuName = "Damaging Spell Trait", order = 51)]
public class DamagingTrait : SpellTrait
{
    public float damage;

    public override void ApplyEffects(GameObject spell, bool isFromPlayer)
    {
        DamagingAttack damagingAttack = spell.AddComponent<DamagingAttack>();
        damagingAttack.Damage = damage;
        string triggersOn = isFromPlayer ? "Enemy" : "Player";
        damagingAttack.TriggersOnTag = triggersOn;
    }

    public override string Name()
    {
        return "Damage";
    }

    public override string Value()
    {
        return $"{damage}";
    }
}