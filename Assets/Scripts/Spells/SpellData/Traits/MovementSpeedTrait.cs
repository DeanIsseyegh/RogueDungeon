using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Speed Spell Trait", menuName = "Movement Speed Spell Trait", order = 51)]
public class MovementSpeedTrait : SpellTrait
{
    public float speed;
    
    public override void ApplyEffects(GameObject spell)
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
