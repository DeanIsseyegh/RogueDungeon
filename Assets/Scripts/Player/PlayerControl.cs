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
    private PlayerSpellCast _playerSpellCast;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerSpellCast = GetComponent<PlayerSpellCast>();
        _playerMovement = new PlayerMovement(mainCamera, _navMeshAgent, clickIndicator);
    }

    void Update()
    {
        _canPlayerMove = _playerSpellCast.IsNotCastingSpell();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerSpellCast.CastSpell(transform);
        }
    }

}