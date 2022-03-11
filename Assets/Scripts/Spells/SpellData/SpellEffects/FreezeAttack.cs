using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAttack : MonoBehaviour
{
    public float LifeTime { set; private get; }
    public string TriggersOnTag { set; private get; }
    public Color FreezeEffectColor { get; set; }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TriggersOnTag))
        {
            FreezeStatus existingFreeze = other.gameObject.GetComponent<FreezeStatus>();
            if (existingFreeze != null) return;

            FreezeStatus freezeStatus = other.gameObject.AddComponent<FreezeStatus>();
            freezeStatus.LifeTime = LifeTime;


            ColorFreezeEffect colorFreezeEffect = other.gameObject.AddComponent<ColorFreezeEffect>();
            colorFreezeEffect.StartColorFreeze(FreezeEffectColor, LifeTime);
        }
        
    }
}
