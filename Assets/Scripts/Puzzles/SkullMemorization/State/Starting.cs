using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzles.SkullMemorization
{
    public class Starting : MemoryGameState
    {
        private List<Skull> _skullsPartOfGame;

        public Starting(MemoryGameStateCtx ctx) : base(ctx)
        {
        }

        public override void Enter()
        {
            base.Enter();
            DecideSkullsToBeHit();
            LightSkullsUp(_skullsPartOfGame);
        }

        public override void Update()
        {
            if (_skullsPartOfGame.All(it => it.HasLitUp))
            {
                Stage = EVENT.EXIT;
            }
        }

        public override void Exit()
        {
            Ctx.Skulls.ForEach(it => it.GetComponent<Skull>().MakeHittable());
            Ctx.Skulls.ForEach(it => it.GetComponent<Skull>().Register(Ctx.PuzzleManager));
            NextState = new Playable(Ctx);
            base.Exit();
        }
        
        private void DecideSkullsToBeHit()
        {
            List<GameObject> skullsNotPartOfGame = new List<GameObject>(Ctx.Skulls);
            _skullsPartOfGame = new List<Skull>();
            for (int i = 0; i < Ctx.SkullsToMemorize; i++)
            {
                int randomIndx = Random.Range(0, skullsNotPartOfGame.Count);
                GameObject randomSkull = skullsNotPartOfGame[randomIndx];
                skullsNotPartOfGame.Remove(randomSkull);
                Skull skull = randomSkull.GetComponent<Skull>();
                _skullsPartOfGame.Add(skull);
            }
        }

        private void LightSkullsUp(List<Skull> skullsPartOfGame)
        {
            float delay = Ctx.InitialDelay;
            skullsPartOfGame.ForEach(skull =>
            {
                Ctx.PuzzleManager.StartCoroutine(LightUpSkull(delay, skull));
                skull.HasLitUp = true;
                delay += Ctx.TimeBetweenEachSkullFlashSecs;
            });
        }
        
        private IEnumerator LightUpSkull(float delay, Skull skull)
        {
            yield return new WaitForSeconds(delay);
            skull.LightUp();
        }
    }
}