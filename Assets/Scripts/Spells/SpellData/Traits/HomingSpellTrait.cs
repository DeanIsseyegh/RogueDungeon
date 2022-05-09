using UnityEngine;

[CreateAssetMenu(fileName = "New Homing Spell Trait", menuName = "Homing Spell Trait", order = 51)]
public class HomingSpellTrait : SpellTrait
{
    public float homingRotationSpeed;
    public Vector3 homingTargetOffset;
    
    public override void ApplyEffects(GameObject spell, bool isFromPlayer)
    {
        HomingAttack homingAttack = spell.AddComponent<HomingAttack>();
        string target = isFromPlayer ? "Enemy" : "Player";
        homingAttack.RotationSpeed = homingRotationSpeed;
        homingAttack.Target = GameObject.FindWithTag(target); //Can be easily optimized with Singleton pattern here
        homingAttack.TargetOffset = homingTargetOffset;
    }

    public override string Name()
    {
        return "Accuracy";
    }

    public override string Value()
    {
        return $"Homing Spell";
    }
}
