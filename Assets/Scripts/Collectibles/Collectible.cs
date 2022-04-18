using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

[InlineEditor()]
public abstract class Collectible : ScriptableObject
{
    [PreviewField(60)]
    [HorizontalGroup("Split", 60)]
    public Sprite icon;

    public virtual void ApplyEffects(PlayerHealth playerHealth)
    {
        
    }
    
    public virtual void ApplyEffects(PlayerMana playerMana)
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
