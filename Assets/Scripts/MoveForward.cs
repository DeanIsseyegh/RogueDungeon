using System;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }

    public void ModifySpeed(Func<float, float> speedModifier)
    {
        moveSpeed = speedModifier(moveSpeed);
    }
}
