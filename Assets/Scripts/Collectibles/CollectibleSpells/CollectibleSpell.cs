using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Collectible", menuName = "SpellCollectible")]
public class CollectibleSpell : Collectible
{

    public Spell spell;
    
    public override void ApplyEffects(PlayerSpellManager playerSpellManager)
    {
        var spellToLearn = Instantiate(spell);
        playerSpellManager.AddSpell(spellToLearn);
    }

}