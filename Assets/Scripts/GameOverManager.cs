using System.Collections;
using System.Collections.Generic;
using Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameObject playerDeathCam;

    public void StartGameOver(GameObject player)
    {
        playerDeathCam.SetActive(true);
        // sceneLoader.LoadStartScreenScene();    
    }
    
}
