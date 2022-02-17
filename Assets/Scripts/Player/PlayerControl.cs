using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject clickIndicator;

    private NavMeshAgent _navMeshAgent;
    private PlayerAnimation _playerAnimation;
    private bool _canPlayerMove = true;
    private PlayerMovement _playerMovement;
    private PlayerSpellManager _playerSpellManager;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerSpellManager = GetComponent<PlayerSpellManager>();
        _playerMovement = new PlayerMovement(mainCamera, _navMeshAgent, clickIndicator);
    }

    void Update()
    {
        _canPlayerMove = _playerSpellManager.IsNotCastingSpell();
        _playerAnimation.CanMove = _canPlayerMove;
        HandleMove();
        HandleSpell();
    }

    private void HandleMove()
    {
        if (_canPlayerMove && Input.GetKey(KeyCode.Mouse0))
        {
            _playerMovement.MovePlayer(Input.mousePosition);
        }
    }

    private void HandleSpell()
    {
        if (Input.anyKeyDown)
        {
            _playerSpellManager.HandleSpell(transform);
        }
    }

}