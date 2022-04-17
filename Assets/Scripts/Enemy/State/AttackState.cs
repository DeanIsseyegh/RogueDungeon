using UnityEngine;

public class AttackState : EnemyState
{
    private readonly EnemyAttack _attack;
    public AttackState(EnemyStateCtx ctx, EnemyAttack nextChosenAttack) : base(ctx)
    {
        Name = STATE.ATTACK;
        _attack = nextChosenAttack;
    }

    public override void Enter()
    {
        Ctx.MeshAgent.ResetPath();
        _attack.DoAttack();
        base.Enter();
    }

    public override void Update()
    {
        RotateTowardsPlayer();
        if (!_attack.IsAttacking())
        {
            NextState = new PursueState(Ctx);
            Stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}