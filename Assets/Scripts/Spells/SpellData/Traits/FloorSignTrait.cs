using UnityEngine;

[CreateAssetMenu(fileName = "New Floor Sign Spell Trait", menuName = "Floor Sign Spell Trait", order = 51)]
public class FloorSignTrait : SpellTrait
{
    public float lifetime = 3f;
    
    public override void ApplyEffects(GameObject spell, bool isFromPlayer)
    {
        FloorSignSpell floorSignSpell = spell.AddComponent<FloorSignSpell>();
        floorSignSpell.Lifetime = lifetime;
    }

    public override string Name()
    {
        return "";
    }

    public override string Value()
    {
        return "";
    }
}