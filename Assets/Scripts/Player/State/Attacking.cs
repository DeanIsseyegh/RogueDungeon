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
            Ctx.ThirdPersonController.CanMove = false;
            _currentSpell.Cast(Ctx.Animator, Ctx.Inventory, Ctx.Mana);
            Ctx.Player.transform.rotation = Ctx.MainCamera.gameObject.transform.rotation;
            base.Enter();
        }

        public override void Update()
        {
            if (_currentSpell.IsCastingSpell) return;
            Ctx.ThirdPersonController.CanMove = true;
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
            else if (IsMoving(velocity))
            {
                NextState = new Running(Ctx);
                Stage = EVENT.EXIT;
            }
        }

        public override void Exit()
        {
            Ctx.Animator.ResetTrigger(_currentSpell.data.animationName);
            base.Exit();
        }
    }
}