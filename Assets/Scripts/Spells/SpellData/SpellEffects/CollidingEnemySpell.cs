using System.Collections.Generic;
using UnityEngine;

public class CollidingEnemySpell : CollidingSpell
{
    private List<string> collidesWith;
    private void Awake()
    {
        collidesWith = new List<string> {"Player"};
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
