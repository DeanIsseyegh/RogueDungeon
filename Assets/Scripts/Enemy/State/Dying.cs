using UnityEngine;

namespace Enemy.State
{
    public class Dying : EnemyState
    {
        private bool _hasStartedDeathAnim;
        private string _deathAnimName = "Death";

        public Dying(EnemyStateCtx ctx) : base(ctx)
        {
            Name = STATE.DYING;
        }
        
        public override void Enter()
        {
            Ctx.Animator.SetTrigger(_deathAnimName);
            Ctx.MeshAgent.enabled = false;
            Ctx.Animator.applyRootMotion = true;
            base.Enter();
        }
        public override void Update()
        {
            AnimatorStateInfo currentAnimatorStateInfo = Ctx.Animator.GetCurrentAnimatorStateInfo(0);
            if (currentAnimatorStateInfo.IsName(_deathAnimName))
            {
                _hasStartedDeathAnim = true;
            } else if (_hasStartedDeathAnim && !currentAnimatorStateInfo.IsName(_deathAnimName))
            {
                Ctx.Despawner.Despawn();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}