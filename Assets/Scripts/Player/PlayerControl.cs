using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GameObject clickIndicator;

    private NavMeshAgent _navMeshAgent;
    private PlayerAnimation _playerAnimation;
    private bool _canPlayerMove = true;
    private PlayerMovement _playerMovement;
    private PlayerSpellManager _playerSpellManager;
    [SerializeField] private MousePositionTracker mousePositionTracker;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerSpellManager = GetComponent<PlayerSpellManager>();
        _playerMovement = new PlayerMovement(_navMeshAgent, clickIndicator);
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
            Vector3 mousePosOnFloor = mousePositionTracker.MousePosOnFloor();
            _playerMovement.MovePlayer(mousePosOnFloor);
        }
    }

    private void HandleSpell()
    {
        if (Input.anyKeyDown)
        {
            _playerSpellManager.HandleSpell();
        }
    }

}