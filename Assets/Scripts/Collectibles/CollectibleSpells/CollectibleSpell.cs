using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Collectible", menuName = "SpellCollectible")]
public class CollectibleSpell : Collectible
{
    public SpellData spellData;
    public SpellInfo info;

    private void OnEnable()
    {
        info.GenerateStats(this);
    }

    public override void ApplyEffects(PlayerSpellManager playerSpellManager)
    {
        GameObject spellObj = new GameObject(spellData.name);
        Spell spellToLearn = spellObj.AddComponent<PlayerSpell>();
        spellToLearn.data = spellData;
        playerSpellManager.AddSpell(spellToLearn, icon);
    }

    public override CollectibleInfo Info()
    {
        return info;
    }
    
}