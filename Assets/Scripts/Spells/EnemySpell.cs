using UnityEngine;

public class EnemySpell : Spell
{

    protected override void Update()
    {
        base.Update();
    }

    protected override void ApplyEffectsToSpell(GameObject spellPrefab, PlayerInventory playerInventory)
    {
        data.spellTraits.ForEach(trait => trait.ApplyEffects(spellPrefab, false));
    }

    protected override Quaternion CalculateSpellRotation(GameObject caster, Vector3 spellPos)
    {
        return caster.transform.rotation;
    }
}
