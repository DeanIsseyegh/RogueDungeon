using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DOT Spell Trait", menuName = "DOT Spell Trait", order = 51)]
public class DOTTrait : SpellTrait
{

    public float damagePerSecond = 5;
    public float lifetime = 5;
    public Color flashEffectColor = Color.red;
    public float flashEffectFrequency = 0.25f;

    public override void ApplyEffects(GameObject spell, bool isFromPlayer)
    {
        DOTAttack dotAttack = spell.AddComponent<DOTAttack>();
        dotAttack.DamagePerSecond = damagePerSecond;
        dotAttack.LifeTime = lifetime;
        string triggersOn = isFromPlayer ? "Enemy" : "Player";
        dotAttack.TriggersOnTag = triggersOn;
        dotAttack.FlashEffectColor = flashEffectColor;
        dotAttack.FlashEffectFrequency = flashEffectFrequency;

    }

    public override string Name()
    {
        return "Damage Over Time";
    }

    public override string Value()
    {
        return $"{damagePerSecond}/s";
    }
}
