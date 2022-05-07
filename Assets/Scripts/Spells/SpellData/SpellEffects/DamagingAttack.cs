using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class DamagingAttack : MonoBehaviour
{
    [ShowInInspector]
    public float Damage { set; private get; }
    public string TriggersOnTag { set; private get; }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TriggersOnTag))
        {
            Health otherHealth = other.gameObject.GetComponent<Health>();
            otherHealth.TakeDamage(Damage);
        }
    }

    public void ModifyDamage(Func<float, float> damageModifier)
    {
        Damage = damageModifier(Damage);
    }

}