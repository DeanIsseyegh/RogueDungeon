using System.Collections;
using Scene;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSuccessManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject playerVictoryCame;
    [SerializeField] private GameObject restartInstructions;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private float timeForRestartToAppear;
    [SerializeField] private PlayerInput playerInput;

    private Animator _animator;
    private CharacterController _characterController;
    private ThirdPersonController _thirdPersonController;
    private bool _isVictory;
    private bool _canPlayerRestart;

    public void StartGameComplete()
    {
        playerInput.SwitchCurrentActionMap("GameOver");
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