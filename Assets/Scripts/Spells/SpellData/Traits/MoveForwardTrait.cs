using UnityEngine;

[CreateAssetMenu(fileName = "New Move Forward Spell Trait", menuName = "Move Forward Spell Trait", order = 51)]
public class MoveForwardTrait : SpellTrait
{
    public float speed;
    
    public override void ApplyEffects(GameObject spell, bool isFromPlayer)
    {
        MoveForward moveForward = spell.AddComponent<MoveForward>();
        moveForward.moveSpeed = speed;
    }

    public override string Name()
    {
        return "Movement Speed";
    }

    public override string Value()
    {
        return $"{speed}";
    }
}
