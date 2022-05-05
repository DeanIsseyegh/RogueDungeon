using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTStatus : MonoBehaviour, Status
{
    private Health _health;

    public float DamagePerSecond { set; private get; }
    public float LifeTime { private get; set; }

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    void Update()
    {
        Apply();
    }

    public void Apply()
    {
        _health.TakeDamage(DamagePerSecond * Time.deltaTime);
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0)
        {
            Remove();
        }
    }

    public void Remove()
    {
        Destroy(this);
    }
}
