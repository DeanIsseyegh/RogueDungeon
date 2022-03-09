using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingEnemySpell : CollidingSpell
{
    protected override string CollidesWith()
    {
        return "Player";
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
