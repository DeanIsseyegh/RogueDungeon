using UnityEngine;

public class DamagingEnemyAttack : DamagingAttack
{
    protected override string TriggersOnTag()
    {
        return "Player";
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}