
using System;
using UnityEngine;

namespace Player.State
{
    public class Idle : PlayerState
    {
        public Idle(PlayerStateCtx ctx) : base(ctx)
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
            
            Vector3 velocity = Ctx.CharController.velocity;
            if (Ctx.InputsController.AnyAttack())
            {
                var attackKeyPressed = AttackKeyPressed();
                Spell spellToCast = Ctx.SpellManager.RetrieveSpell(attackKeyPressed);
                NextState = new Attacking(Ctx, spellToCast);
                Stage = EVENT.EXIT;
            } else if (Math.Abs(velocity.x) + Math.Abs(velocity.z) > 0.4)
            {
                // _animationTriggersToResetOnMove.ForEach(Ctx.Animator.ResetTrigger);

                NextState = new Running(Ctx);
                Stage = EVENT.EXIT;
            }

        }

        public override void Exit()
        {
            Ctx.Animator.SetBool("idle", false);
            base.Exit();
        }
    }
}