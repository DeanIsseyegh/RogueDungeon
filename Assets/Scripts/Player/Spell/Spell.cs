using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
    public SpellScriptableObj SpellAttributes;

    private void Start()
    {
        MoveForward moveForward = GetComponent<MoveForward>();
        moveForward.moveSpeed = SpellAttributes.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
    
}
