using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidingSpell : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

}
