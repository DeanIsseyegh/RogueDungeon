using UnityEngine;

namespace Puzzles.SkullMemorization
{
    public class MemoryGameState
    {
        public STATE Name;
        protected EVENT Stage;
        protected MemoryGameState NextState;
        protected MemoryGameStateCtx Ctx;


        public enum STATE
        {
            STARTING,
            PLAYABLE,
            FINISHED
        }

        public enum EVENT
        {
            ENTER,
            UPDATE,
            EXIT
        }

        public MemoryGameState(MemoryGameStateCtx ctx)
        {
            Ctx = ctx;
        }

        public virtual void Enter()
        {
            Stage = EVENT.UPDATE;
        }

        public virtual void Update()
        {
            Stage = EVENT.UPDATE;
        }

        public virtual void Exit()
        {
            Stage = EVENT.EXIT;
        }

        public MemoryGameState Process()
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

    }
}