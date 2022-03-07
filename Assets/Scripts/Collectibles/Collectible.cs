using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

[InlineEditor()]
public abstract class Collectible : ScriptableObject
{
    [PreviewField(60)]
    [HorizontalGroup("Split", 60)]
    public Sprite icon;

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

    public abstract CollectibleInfo Info();

}
