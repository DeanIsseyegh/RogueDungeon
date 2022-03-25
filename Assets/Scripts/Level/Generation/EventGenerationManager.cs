using System.Collections.Generic;
using Level.RoomEvents;
using UnityEngine;

namespace Level.Generation
{
    public class EventGenerationManager : MonoBehaviour
    {
        [SerializeField] private PuzzleEventGenerator puzzleEventGenerator;
        [SerializeField] private EnemyEventGenerator enemyEventGenerator;
        [SerializeField] private CollectibleEventGenerator collectibleEventGenerator;
        
        public void GenerateEvent(GeneratedRoom generatedRoom, GeneratedRoom previousRoom,
            List<GameObject> sideExits)
        {
            RoomData roomData = generatedRoom.RoomData;
            if (roomData.hasItem | roomData.hasSpell)
                collectibleEventGenerator.GenerateCollectibleEvent(generatedRoom, previousRoom, sideExits);
            else if (roomData.hasEnemies)
                enemyEventGenerator.GenerateEnemyEvent(generatedRoom, previousRoom, sideExits);
        }

        public void GenerateSideEvent(GeneratedRoom generatedRoom, bool isRightSideRoom)
        {
            RoomData roomData = generatedRoom.RoomData;
            if (roomData.isPuzzleRoom && roomData.isBalloonRoom)
            {
                puzzleEventGenerator.GeneratePuzzleEvent(generatedRoom, isRightSideRoom);
            }
        }
    }
}