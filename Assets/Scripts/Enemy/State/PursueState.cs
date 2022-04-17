using UnityEngine;
using UnityEngine.AI;

public class PursueState : EnemyState
{
    private readonly EnemyAttack _nextChosenAttack;
    public PursueState(EnemyStateCtx ctx) : base(ctx)
    {
        Name = STATE.PURSUE;
        _nextChosenAttack = Ctx.EnemyAttacks[Random.Range(0, Ctx.EnemyAttacks.Length)];
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
            NextState = new AttackState(Ctx, _nextChosenAttack);
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
    
    protected bool CanAttackPlayer()
    {
        float distance = Vector3.Distance(Ctx.Player.transform.position, Ctx.Enemy.transform.position);
        return distance < _nextChosenAttack.AttackDistance();
    }
}