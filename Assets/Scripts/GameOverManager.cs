using Sirenix.Utilities;
using StarterAssets;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject playerDeathCam;
    [SerializeField] private GameObject deathScreen;

    public void StartGameOver(GameObject player)
    {
        player.GetComponent<InputsController>().cursorInputForLook = false;
        playerDeathCam.SetActive(true);
        EnemyAI[] allEnemies = FindObjectsOfType<EnemyAI>();
        allEnemies.ForEach(enemy => enemy.enabled = false);
        deathScreen.SetActive(true);
    }
    
}
