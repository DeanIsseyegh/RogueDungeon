using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Collectible : ScriptableObject
{
    public Sprite icon;
    [FormerlySerializedAs("collectibleInfo")] public CollectibleInfo info;

    public virtual void ApplyEffects(PlayerHealth playerHealth)
    {
        
    }

    public virtual void ApplyEffects(PlayerSpellManager playerSpellManager)
    {
        
    }

    public virtual void ApplyEffects(NavMeshAgent playerNavMeshAgent)
    {
        
    }

    public virtual void ApplyEffects(GameObject spellPrefab)
    {
        
    }

    public virtual void ApplyEffects(UIManager uiManager)
    {
        
    }

}
