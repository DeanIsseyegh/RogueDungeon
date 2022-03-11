using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Freeze Spell Trait", menuName = "DOT Freeze Trait", order = 51)]
public class FreezeTrait : SpellTrait
{

    public float freezeLength = 2f;
    public Color freezeColor = Color.blue;
    
    public override void ApplyEffects(GameObject spell, bool isFromPlayer)
    {
        FreezeAttack freezeAttack = spell.AddComponent<FreezeAttack>();
        string triggersOn = isFromPlayer ? "Enemy" : "Player";
        freezeAttack.TriggersOnTag = triggersOn;
        freezeAttack.LifeTime = freezeLength;
        freezeAttack.FreezeEffectColor = freezeColor;
    }

    public override string Name()
    {
        return "Freeze Duration";
    }

    public override string Value()
    {
        return $"{freezeLength}s";
    }
}
