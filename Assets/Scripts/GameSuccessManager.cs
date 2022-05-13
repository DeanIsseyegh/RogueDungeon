using StarterAssets;
using UnityEngine;

public class GameSuccessManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerVictoryCame;

    private Animator _animator;
    private CharacterController _characterController;
    private ThirdPersonController _thirdPersonController;
    private bool _isVictory;
    private InputsController _inputsController;

    private void Awake()
    {
        _inputsController = player.GetComponent<InputsController>();
    }

    public void StartGameComplete()
    {
        _isVictory = true;
        victoryScreen.SetActive(true);
        playerVictoryCame.SetActive(true);
        _inputsController.cursorLocked = false;
    }

    public bool IsVictory()
    {
        return _isVictory;
    }
}
