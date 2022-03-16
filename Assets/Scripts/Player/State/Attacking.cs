using System;
using UnityEngine;

namespace Player.State
{
    public class Attacking : PlayerState
    {
        private readonly Spell _currentSpell;

        public Attacking(PlayerStateCtx ctx, Spell currentSpell) : base(ctx)
        {
            _currentSpell = currentSpell;
            Name = STATE.ATTACKING;
        }

        public override void Enter()
        {
            // Ctx.ThirdPersonController.LockCameraPosition = true;
            Ctx.ThirdPersonController.CanMove = false;

            _currentSpell.Cast(Ctx.Animator, Ctx.Inventory, Ctx.Mana);
            Ctx.Player.transform.rotation = Camera.main.gameObject.transform.rotation;
            base.Enter();
        }

        public override void Update()
        {
            if (!_currentSpell.IsCastingSpell)
            {
                // Ctx.ThirdPersonController.LockCameraPosition = false;
                Ctx.ThirdPersonController.CanMove = true;

                Vector3 velocity = Ctx.CharController.velocity;
                if (Ctx.InputsController.AnyAttack())
                {
                    var attackKeyPressed = AttackKeyPressed();
                    Spell spellToCast = Ctx.SpellManager.RetrieveSpell(attackKeyPressed);
                    NextState = new Attacking(Ctx, spellToCast);
                    Stage = EVENT.EXIT;
                }
                else if (Math.Abs(velocity.x) + Math.Abs(velocity.z) > 0.4)
                {
                    NextState = new Running(Ctx);
                    Stage = EVENT.EXIT;
                }
            }
        }

        public override void Exit()
        {
            Ctx.Animator.ResetTrigger(_currentSpell.data.animationName);
            Ctx.Animator.SetBool("idle", false);
            base.Exit();
        }
    }
}