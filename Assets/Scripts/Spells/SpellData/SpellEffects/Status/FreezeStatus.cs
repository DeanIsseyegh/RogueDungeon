using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeStatus : MonoBehaviour, Status
{
    private Health _health;
    
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
        
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0)
        {
            Destroy(this);
        }
    }
}
