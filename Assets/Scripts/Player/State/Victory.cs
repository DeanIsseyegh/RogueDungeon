namespace Player.State
{
    public class Victory : PlayerState
    {
        private readonly string VictoryAnimName = "Victory";
        public Victory(PlayerStateCtx ctx) : base(ctx)
        {
            Name = STATE.VICTORY;
        }
        
        public override void Enter()
        {
            Ctx.Animator.SetTrigger(VictoryAnimName);
            Ctx.Animator.applyRootMotion = true;
            Ctx.CharController.enabled = false;
            Ctx.ThirdPersonController.enabled = false;
            base.Enter();
        }


        public override void Update()
        {
            
        }

        public override void Exit()
        {
            base.Exit();
        }      
    }
}