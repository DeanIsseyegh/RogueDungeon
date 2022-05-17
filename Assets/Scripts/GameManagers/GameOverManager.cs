using System.Collections;
using Scene;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject playerDeathCam;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject restartInstructions;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private float timeForRestartToAppear;
    private bool _canPlayerRestart;

    public void StartGameOver(GameObject player)
    {
        playerDeathCam.SetActive(true);
        EnemyAI[] allEnemies = FindObjectsOfType<EnemyAI>();
        allEnemies.ForEach(enemy => enemy.enabled = false);
        deathScreen.SetActive(true);
        StartCoroutine(ShowRestartInstructions());
    }

    private IEnumerator ShowRestartInstructions()
    {
        yield return new WaitForSeconds(timeForRestartToAppear);
        _canPlayerRestart = true;
        restartInstructions.SetActive(true);
    }

    public void OnRestart()
    {
        if (!_canPlayerRestart) return;
        sceneLoader.LoadStartScreenScene();
    }
}