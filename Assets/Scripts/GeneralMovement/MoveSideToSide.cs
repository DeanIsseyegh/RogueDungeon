using System;
using UnityEngine;

public class MoveSideToSide : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float dir;
    [SerializeField] private String collidesWithTag;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = _rb.transform.right * dir * speed;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _rb.transform.right * dir * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag(collidesWithTag) || other.collider.CompareTag(gameObject.tag))
        {
            dir = -dir;
        }
    }

}
