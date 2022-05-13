using Player.State;
using StarterAssets;
using UnityEngine;

public class PlayerStateControl : MonoBehaviour
{
    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private GameSuccessManager gameSuccessManager;
    private PlayerState _currentState;

    private void Start()
    {
        var animator = GetComponent<Animator>();
        var inputs = GetComponent<InputsController>();
        var playerStateCtx = new PlayerStateCtx(gameObject, animator, Camera.main, inputs, gameOverManager, gameSuccessManager);
        _currentState = new Player.State.Idle(playerStateCtx);
    }

    void Update()
    {
        _currentState = _currentState.Process();
    }

}