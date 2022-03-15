using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Spin Spell Trait", menuName = "Spin Spell Trait", order = 51)]
public class SpinTrait : SpellTrait
{
    public float spinSpeed;
    
    public override void ApplyEffects(GameObject spell, bool isFromPlayer)
    {
        Spin spin = spell.AddComponent<Spin>();
        spin.Speed = spinSpeed;
    }

    public override string Name()
    {
        return "Spin Speed";
    }

    public override string Value()
    {
        return $"{spinSpeed}";
    }
}
