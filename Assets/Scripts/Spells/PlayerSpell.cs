using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class PlayerSpell : Spell
{
    [SerializeField] protected MousePositionTracker mousePositionTracker;

    protected override void Update()
    {
        base.Update();
    }

    public override void Cast(NavMeshAgent navMeshAgent, AnimationHandler animationHandler, 
        PlayerInventory playerInventory)
    {
        base.Cast(navMeshAgent, animationHandler, playerInventory);
        navMeshAgent.gameObject.transform.LookAt(mousePositionTracker.MousePosOnFloor());
    }
}