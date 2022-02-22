using UnityEngine;
using UnityEngine.AI;

public class PlayerSpell : Spell
{
    [SerializeField] protected MousePositionTracker mousePositionTracker;

    protected override void Update()
    {
        base.Update();
    }

    public override void Cast(NavMeshAgent navMeshAgent, Animator animator, 
        PlayerInventory playerInventory)
    {
        base.Cast(navMeshAgent, animator, playerInventory);
        navMeshAgent.gameObject.transform.LookAt(mousePositionTracker.MousePosOnFloor());
    }
}