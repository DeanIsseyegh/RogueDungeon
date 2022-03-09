using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingPlayerSpell : CollidingSpell
{

    protected override string CollidesWith()
    {
        return "Enemy";
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
