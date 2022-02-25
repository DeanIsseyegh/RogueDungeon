using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSpell : Spell
{
    private MousePositionTracker _mousePositionTracker;

    private void Start()
    {
        _mousePositionTracker =
            GameObject.FindWithTag("MousePositionTracker").GetComponent<MousePositionTracker>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Cast(NavMeshAgent navMeshAgent, Animator animator,
        PlayerInventory playerInventory, PlayerMana playerMana)
    {
        if (!IsOnCooldown() && playerMana.CurrentMana >= data.manaCost)
        {
            playerMana.UseMana(data.manaCost);
            navMeshAgent.gameObject.transform.LookAt(_mousePositionTracker.MousePosOnFloor());
            base.Cast(navMeshAgent, animator, playerInventory);
        }
    }
}