using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Collectible", menuName = "SpellCollectible")]
public class CollectibleSpell : Collectible
{

    public SpellData spellData;
    private GameObject _spellObj;

    private void OnEnable()
    {
        _spellObj = new GameObject(spellData.name);
    }

    public override void ApplyEffects(PlayerSpellManager playerSpellManager)
    {
        Spell spellToLearn = _spellObj.AddComponent<PlayerSpell>();
        spellToLearn.data = spellData;
        playerSpellManager.AddSpell(spellToLearn, icon);
    }

}