using UnityEngine;

public abstract class DamagingAttack : MonoBehaviour
{
    public float Damage { set; private get; }

    protected abstract string TriggersOnTag();
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TriggersOnTag()))
        {
            Health playerHealth = other.gameObject.GetComponent<Health>();
            playerHealth.TakeDamage(Damage);
        }
    }
}