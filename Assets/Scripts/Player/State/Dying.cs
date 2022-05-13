using UnityEngine;

namespace Player.State
{
    public class Dying : PlayerState
    {
        private const string DeathAnimName = "Death";
        private bool _hasStartedDeathAnim;

        public Dying(PlayerStateCtx ctx) : base(ctx)
        {
            Name = STATE.DYING;
        }
        
        public override void Enter()
        {
            Ctx.Animator.SetTrigger(DeathAnimName);
            Ctx.Animator.applyRootMotion = true;
            Ctx.CharController.enabled = false;
            Ctx.ThirdPersonController.enabled = false;
            base.Enter();
        }
        
        public override void Update()
        {
            AnimatorStateInfo currentAnimatorStateInfo = Ctx.Animator.GetCurrentAnimatorStateInfo(0);
            if (!_hasStartedDeathAnim && currentAnimatorStateInfo.IsName(DeathAnimName))
            {
                _hasStartedDeathAnim = true;
            } else if (_hasStartedDeathAnim && currentAnimatorStateInfo.normalizedTime >= 0.99f)
            {
                Ctx.GameOverManager.StartGameOver(Ctx.Player);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }       
    }
}