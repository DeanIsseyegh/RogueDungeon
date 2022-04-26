using UnityEngine;

namespace Spells.SpellData.SpellEffects
{
    public class IntermittentDamagingAttack : MonoBehaviour
    {
        public float Damage { set; private get; }
        public string TriggersOnTag { set; private get; }
        public float DamageFrequency { set; private get; }

        private float _timeSinceLastTakenDamage;

        private void Update()
        {
            _timeSinceLastTakenDamage += Time.deltaTime;
        }
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            OnHit(other);
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            OnHit(other);
        }
        
        private void OnHit(Collider other)
        {
            if (other.CompareTag(TriggersOnTag) && _timeSinceLastTakenDamage >= DamageFrequency)
            {
                Health otherHealth = other.gameObject.GetComponent<Health>();
                otherHealth.TakeDamage(Damage);
                _timeSinceLastTakenDamage = 0;
            }
        }
    }
}