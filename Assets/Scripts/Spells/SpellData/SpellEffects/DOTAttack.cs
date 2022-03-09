using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTAttack : MonoBehaviour
{
    public float DamagePerSecond { set; private get; }
    public float LifeTime { set; private get; }
    public string TriggersOnTag { set; private get; }
    public Color FlashEffectColor { get; set; }
    public float FlashEffectFrequency { get; set; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TriggersOnTag))
        {
            DOTStatus dotStatus = other.gameObject.AddComponent<DOTStatus>();
            dotStatus.DamagePerSecond = DamagePerSecond;
            dotStatus.LifeTime = LifeTime;

            FlashingEffect flashingEffect = other.gameObject.AddComponent<FlashingEffect>();
            flashingEffect.StartFlashing(FlashEffectFrequency, FlashEffectColor, LifeTime);
        }
        
    }
}
