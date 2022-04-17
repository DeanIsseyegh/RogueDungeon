using UnityEngine;

public class DamagingAttack : MonoBehaviour
{
    public float Damage { set; private get; }
    public string TriggersOnTag { set; private get; }
    
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TriggersOnTag))
        {
            Health otherHealth = other.gameObject.GetComponent<Health>();
            otherHealth.TakeDamage(Damage);
        }
    }
}