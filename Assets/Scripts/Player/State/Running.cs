using System;
using UnityEngine;

namespace Player.State
{
    public class Running : PlayerState
    {
        public Running(PlayerStateCtx ctx) : base(ctx)
        {
            Name = STATE.RUNNING;
        }

        public override void Enter()
        {
            Ctx.Animator.SetBool(Running, true);
            base.Enter();
        }

        public override void Update()
        {
            base.Update();
            Vector3 velocity = Ctx.CharController.velocity;
            if (Ctx.InputsController.AnyAttack())
            {
                var attackKeyPressed = AttackKeyPressed();
                Spell spellToCast = Ctx.SpellManager.RetrieveSpell(attackKeyPressed);
                if (spellToCast != null)
                {
                    NextState = new Attacking(Ctx, spellToCast);
                    Stage = EVENT.EXIT;
                }
            }
            else if (!IsMoving(velocity))
            {
                NextState = new Idle(Ctx);
                Stage = EVENT.EXIT;
            }
        }

        public override void Exit()
        {
            Ctx.Animator.SetBool(Running, false);
            base.Exit();
        }
    }
}