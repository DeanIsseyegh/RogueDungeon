using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Collectible : ScriptableObject
{
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

}
