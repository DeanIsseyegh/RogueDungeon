﻿
using Enemy.State;
using UnityEngine;

public class Idle : EnemyState
{
    public Idle(EnemyStateCtx ctx) : base(ctx)
    {
        Name = STATE.IDLE;
    }

    public override void Enter()
    {
        Ctx.Animator.SetBool("idle", true);
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        if (CanSeePlayer())
        {
            NextState = new PursueState(Ctx);
            Stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        Ctx.Animator.SetBool("idle", false);
        base.Exit();
    }
}