using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingPlayerSpell : CollidingSpell
{
    private List<string> collidesWith;
    private void Awake()
    {
        collidesWith = new List<string>() {"Enemy", "Balloon"};
    }

    protected override List<string> CollidesWith()
    {
        return collidesWith;
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
