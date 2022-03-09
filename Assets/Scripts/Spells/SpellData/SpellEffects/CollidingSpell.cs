using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollidingSpell : MonoBehaviour
{

    protected abstract string CollidesWith();
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CollidesWith()))
        {
            Destroy(this.gameObject);
        }
    }

}
