using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GameObject clickIndicator;
    [SerializeField] private MousePositionTracker mousePositionTracker;

    private NavMeshAgent _navMeshAgent;
    private AnimationHandler _animationHandler;
    private bool _canPlayerMove = true;
    private PlayerMovement _playerMovement;
    private PlayerSpellManager _playerSpellManager;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animationHandler = GetComponent<AnimationHandler>();
        _playerSpellManager = GetComponent<PlayerSpellManager>();
        _playerMovement = new PlayerMovement(_navMeshAgent, clickIndicator);
    }

    void Update()
    {
        _canPlayerMove = !_playerSpellManager.IsCastingSpell();
        _animationHandler.CanMove = _canPlayerMove;
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