using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damaging Spell Trait", menuName = "Damaging Spell Trait", order = 51)]
public class DamagingTrait : SpellTrait
{
    public bool doesDamagePlayer;
    public float damage;

    public override void ApplyEffects(GameObject spell)
    {
        DamagingAttack damagingAttack;
        if (doesDamagePlayer)
        {
            damagingAttack = spell.AddComponent<DamagingEnemyAttack>();
        }
        else
        {
            damagingAttack = spell.AddComponent<DamagingPlayerAttack>();
        }
        damagingAttack.Damage = damage;
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