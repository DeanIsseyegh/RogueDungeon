using UnityEngine;

public class MoveRight : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float dir;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = _rb.transform.right * dir * speed;
    }

}
