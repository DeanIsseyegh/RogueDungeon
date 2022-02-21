using System;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var vel = transform.forward * moveSpeed;
        _rb.velocity = vel;
    }

    public void ModifySpeed(Func<float, float> speedModifier)
    {
        moveSpeed = speedModifier(moveSpeed);
    }
}
