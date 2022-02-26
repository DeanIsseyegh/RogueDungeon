using UnityEngine;

public class DamagingPlayerAttack : DamagingAttack
{
    protected override string TriggersOnTag()
    {
        return "Enemy";
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}