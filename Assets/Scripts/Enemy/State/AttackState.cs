public class AttackState : EnemyState
{

    public AttackState(EnemyStateCtx ctx) : base(ctx)
    {
        Name = STATE.ATTACK;
    }

    public override void Enter()
    {
        Ctx.MeshAgent.ResetPath();
        Ctx.EnemyAttack.DoAttack();
        base.Enter();
    }

    public override void Update()
    {
        RotateTowardsPlayer();
        if (!Ctx.EnemyAttack.IsAttacking())
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