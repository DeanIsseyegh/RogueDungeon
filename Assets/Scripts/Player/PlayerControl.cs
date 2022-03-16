using Player.State;
using StarterAssets;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private PlayerState _currentState;

    private void Start()
    {
        var animator = GetComponent<Animator>();
        var inputs = GetComponent<InputsController>();
        var playerStateCtx = new PlayerStateCtx(gameObject, animator, Camera.main, inputs);
        _currentState = new Player.State.Idle(playerStateCtx);
    }

    void Update()
    {
        _currentState = _currentState.Process();
    }


}