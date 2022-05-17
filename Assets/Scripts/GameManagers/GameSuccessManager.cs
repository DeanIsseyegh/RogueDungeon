using System.Collections;
using Scene;
using StarterAssets;
using UnityEngine;

public class GameSuccessManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject playerVictoryCame;
    [SerializeField] private GameObject restartInstructions;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private float timeForRestartToAppear;

    private Animator _animator;
    private CharacterController _characterController;
    private ThirdPersonController _thirdPersonController;
    private bool _isVictory;
    private bool _canPlayerRestart;

    public void StartGameComplete()
    {
        _isVictory = true;
        victoryScreen.SetActive(true);
        playerVictoryCame.SetActive(true);
        StartCoroutine(ShowRestartInstructions());
    }

    public bool IsVictory()
    {
        return _isVictory;
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