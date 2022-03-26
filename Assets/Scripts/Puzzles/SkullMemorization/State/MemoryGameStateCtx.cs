using System.Collections.Generic;
using UnityEngine;

namespace Puzzles.SkullMemorization
{
    public class MemoryGameStateCtx
    {
        public List<GameObject> Skulls { get; }
        public int SkullsToMemorize { get; set; }
        public float InitialDelay { get; set; }
        public float TimeBetweenEachSkullFlashSecs { get; set; }
        public MemorizationPuzzleManager PuzzleManager { get; set; }

        public MemoryGameStateCtx(List<GameObject> skulls, int skullsToMemorize, float initialDelay, 
            float timeBetweenEachSkullFlashSecs, MemorizationPuzzleManager puzzleManager)
        {
            Skulls = skulls;
            SkullsToMemorize = skullsToMemorize;
            InitialDelay = initialDelay;
            TimeBetweenEachSkullFlashSecs = timeBetweenEachSkullFlashSecs;
            PuzzleManager = puzzleManager;
        }
    }
}