using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private AnimationHandler _animationHandler;
    private bool _canPlayerMove = true;
    private PlayerMovement _playerMovement;
    private PlayerSpellManager _playerSpellManager;
    private Rigidbody _rb;

    private void Start()
    {
        _animationHandler = GetComponent<AnimationHandler>();
        _playerSpellManager = GetComponent<PlayerSpellManager>();
        _rb = GetComponent<Rigidbody>();
        // _playerMovement = new PlayerMovement(_navMeshAgent, clickIndicator);
    }

    void Update()
    {
        _canPlayerMove = !_playerSpellManager.IsCastingSpell();
        _animationHandler.CanMove = _canPlayerMove;
        // HandleMove();
        HandleSpell();
    }

    private void HandleMove()
    {
        if (_canPlayerMove)
        {

            
        }
    }

    private void HandleSpell()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            _playerSpellManager.HandleSpell();
        }
    }

}