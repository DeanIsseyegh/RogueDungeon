using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
    public SpellScriptableObj SpellAttributes;
    private MoveForward _moveForward;

    private void Awake()
    {
        _moveForward = GetComponent<MoveForward>();
    }

    public void ModifySpeed(Func<float, float> speedModifier)
    {
        _moveForward.moveSpeed = speedModifier(_moveForward.moveSpeed);
        Debug.Log("Move speed is: " + _moveForward.moveSpeed);
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
