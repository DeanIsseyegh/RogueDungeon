using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CollidingSpell : MonoBehaviour
{
    [SerializeField] private GameObject spellExplosionPrefab;

    protected abstract List<string> CollidesWith();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (CollidesWith().Any(it => other.CompareTag(it)) || other.CompareTag("Door") || other.CompareTag("Wall") || other.CompareTag("Floor"))
        {
            SpellDestroyEffect();
        }
    }

    private void SpellDestroyEffect()
    {
        Destroy(this.gameObject);
        if (spellExplosionPrefab != null)
        {
            GameObject createdSpell =
                Instantiate(spellExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(createdSpell, 4f);
        }
    }
}