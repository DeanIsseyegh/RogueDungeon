using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollidingSpell : MonoBehaviour
{
    [SerializeField] private GameObject spellExplosionPrefab;

    protected abstract string CollidesWith();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CollidesWith()) || other.CompareTag("Door") || other.CompareTag("Wall") || other.CompareTag("Floor"))
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
}