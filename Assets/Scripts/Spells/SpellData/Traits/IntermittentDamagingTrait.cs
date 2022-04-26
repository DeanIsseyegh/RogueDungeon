using Spells.SpellData.SpellEffects;
using UnityEngine;

[CreateAssetMenu(fileName = "New Intermittent Damaging Spell Trait", menuName = "Intermittent Damaging Spell Trait", order = 51)]
public class IntermittentDamagingTrait : SpellTrait
{
    public float damage;
    public float damageFrequency = 0;

    public override void ApplyEffects(GameObject spell, bool isFromPlayer)
    {
        IntermittentDamagingAttack damagingAttack = spell.AddComponent<IntermittentDamagingAttack>();
        damagingAttack.Damage = damage;
        string triggersOn = isFromPlayer ? "Enemy" : "Player";
        damagingAttack.TriggersOnTag = triggersOn;
        damagingAttack.DamageFrequency = damageFrequency;
    }

    public override string Name()
    {
        return "Damage";
    }

    public override string Value()
    {
        return $"{damage} per {damageFrequency}s";
    }
}