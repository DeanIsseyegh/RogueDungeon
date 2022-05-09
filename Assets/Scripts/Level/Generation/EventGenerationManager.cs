using System.Collections.Generic;
using Level.Generation.EventGenerators;
using Level.RoomEvents;
using UnityEngine;

namespace Level.Generation
{
    public class EventGenerationManager : MonoBehaviour
    {
        [SerializeField] private PuzzleEventGenerator puzzleEventGenerator;
        [SerializeField] private EnemyEventGenerator enemyEventGenerator;
        [SerializeField] private CollectibleEventGenerator collectibleEventGenerator;
        [SerializeField] private BossEventGenerator bossEventGenerator;

        public void GenerateEvent(GeneratedRoom generatedRoom, GeneratedRoom previousRoom,
            List<GameObject> sideExits)
        {
            RoomData roomData = generatedRoom.RoomData;
            if (roomData.hasItem | roomData.hasSpell)
                collectibleEventGenerator.GenerateCollectibleEvent(generatedRoom, previousRoom, sideExits);
            else if (roomData.hasEnemies)
                enemyEventGenerator.GenerateEnemyEvent(generatedRoom, previousRoom, sideExits);
            else if (roomData.isBossRoom)
                bossEventGenerator.GenerateBossEvent(generatedRoom);
        }

        public void GenerateSideEvent(GeneratedRoom generatedRoom, bool isRightSideRoom)
        {
            RoomData roomData = generatedRoom.RoomData;
            if (roomData.isPuzzleRoom)
            {
                if (roomData.isBalloonRoom)
                {
                    puzzleEventGenerator.GenerateBalloonEvent(generatedRoom, isRightSideRoom);
                } else if (roomData.isSkullRoom)
                {
                    puzzleEventGenerator.GenerateSkullEvent(generatedRoom, isRightSideRoom);
                }
            }
        }
    }
}