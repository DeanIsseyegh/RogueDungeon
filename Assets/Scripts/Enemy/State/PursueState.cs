using UnityEngine;
using UnityEngine.AI;

public class PursueState : EnemyState
{
    public PursueState(EnemyStateCtx ctx) : base(ctx)
    {
        Name = STATE.PURSUE;
    }

    public override void Enter()
    {
        Ctx.Animator.SetBool("running", true);
        base.Enter();
    }

    public override void Update()
    {
        Ctx.MeshAgent.SetDestination(Ctx.Player.transform.position);
        if (CanAttackPlayer())
        {
            NextState = new AttackState(Ctx);
            Stage = EVENT.EXIT;
        }
        else if (!CanSeePlayer())
        {
            NextState = new Patrol(Ctx);
            Stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        Ctx.MeshAgent.ResetPath();
        Ctx.Animator.SetBool("running", false);
        base.Exit();
    }
}