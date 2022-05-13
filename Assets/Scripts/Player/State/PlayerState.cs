using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.State
{
    public class PlayerState
    {
        protected static readonly int Running = Animator.StringToHash("running");

        public STATE Name;
        protected EVENT Stage;
        protected PlayerState NextState;
        protected PlayerStateCtx Ctx;

        public enum STATE
        {
            IDLE,
            RUNNING,
            ATTACKING,
            DYING
        }

        public enum EVENT
        {
            ENTER,
            UPDATE,
            EXIT
        }

        public PlayerState(PlayerStateCtx ctx)
        {
            Ctx = ctx;
            new List<string> {"BasicSpell"};
        }

        public virtual void Enter()
        {
            Stage = EVENT.UPDATE;
        }

        public virtual void Update()
        {
            Stage = EVENT.UPDATE;
            if (Ctx.Health.IsDepleted())
            {
                NextState = new Dying(Ctx);
                Stage = EVENT.EXIT;
            }
        }

        public virtual void Exit()
        {
            Stage = EVENT.EXIT;
        }

        public PlayerState Process()
        {
            if (Stage == EVENT.ENTER) Enter();
            if (Stage == EVENT.UPDATE) Update();
            if (Stage == EVENT.EXIT)
            {
                Exit();
                return NextState;
            }

            return this;
        }

        protected void RotatePlayerTowardsPlayer()
        {
            Ctx.Animator.gameObject.transform.rotation = Ctx.MainCamera.transform.rotation;
        }

        protected KeyCode AttackKeyPressed()
        {
            KeyCode attackKeyPressed = KeyCode.None;
            if (Ctx.InputsController.attack1)
            {
                attackKeyPressed = KeyCode.Alpha1;
            }
            else if (Ctx.InputsController.attack2)
            {
                attackKeyPressed = KeyCode.Alpha2;
            }
            else if (Ctx.InputsController.attack3)
            {
                attackKeyPressed = KeyCode.Alpha3;
            }
            else if (Ctx.InputsController.attack4)
            {
                attackKeyPressed = KeyCode.Alpha4;
            }

            return attackKeyPressed;
        }

        protected static bool IsMoving(Vector3 velocity)
        {
            return Math.Abs(velocity.x) + Math.Abs(velocity.z) > 0.4;
        }
    }
}